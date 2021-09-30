using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using TMPro;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryContentMultiPagedView: StoryContentPagedView
    {
        #region Internal 

        private class LinkedTextViewPool: ObjectPool<StoryTextLinkedView>
        {
            private readonly StoryContentMultiPagedView _owner;
            private readonly StoryTextLinkedView _template;
            private readonly Transform _parent;

            public LinkedTextViewPool(StoryContentMultiPagedView owner)
            {
                _owner = owner;
                this.AddTo(owner);
                _template = owner.linkedTextViewTemplate;
                _template.ClearText();
                _template.gameObject.SetActive(false);
                _parent = _template.transform.parent;
            }

            protected override void OnBeforeRent(StoryTextLinkedView instance)
            {
                instance.ClearText();
                instance.MaxVisibleCharacters = 0;
                base.OnBeforeRent(instance);
            }

            protected override void OnBeforeReturn(StoryTextLinkedView instance)
            {
                base.OnBeforeReturn(instance);
                instance.UnlinkAllViews();
            }

            protected override StoryTextLinkedView CreateInstance()
            {
                var instance = Instantiate(_template, _parent);
                return instance;
            }
        }

        #endregion
        
        #region Inspector

        [SerializeField] private StoryTextLinkedView linkedTextViewTemplate;

        #endregion

        #region Properties

        public override string ContentText => _firstLinkedView.Text;
        public override bool IsEmpty => _firstLinkedView.IsEmpty;

        public override int PageCount => _linkedViews.Count;

        public override int CurrentPage => _curViewIndex + 1;

        public override int LastDisplayedPage => _displayedPagesCount;

        #endregion

        #region Variables
        
        private StoryTextLinkedView _firstLinkedView;
        private StoryTextLinkedView _lastLinkedView;
        private int _curViewIndex = 0;
        private int _displayedPagesCount = 1;
        private readonly List<StoryTextLinkedView> _linkedViews = new List<StoryTextLinkedView>();
        private LinkedTextViewPool _textViewPool;

        #endregion

        #region Methods
        #region Unity Callbacks

        protected override void Reset()
        {
            base.Reset();
            
            if (!linkedTextViewTemplate)
                linkedTextViewTemplate = GetComponentInChildren<StoryTextLinkedView>();
        }

        protected override void Awake()
        {
            base.Awake();
            
            _textViewPool = new LinkedTextViewPool(this);
            _firstLinkedView = _lastLinkedView = _textViewPool.Rent();
            _linkedViews.Add(_firstLinkedView);
            _firstLinkedView.name = "Page_1";
        }

        #endregion

        #region Public

        public override void ClearContent()
        {
            _displayedPagesCount = 1;
            _curViewIndex = 0;
            for (int i = 1; i < _textViewPool.Count; i++)
            {
                _textViewPool.Return(_linkedViews[i]);
            }
            _firstLinkedView.ClearText();
            _firstLinkedView.ForceTextUpdate();
            _lastLinkedView = _firstLinkedView;
        }

        public override void AddContent(string contentText)
        {
            _firstLinkedView.Text += IsEmpty ? contentText : $"\n{contentText}";
            ExpandLinkedViews();
        }

        public override UniTask ShowNewContentAsync(CancellationToken animationCancelToken)
        {
            var lastDisplayedView = _linkedViews[_displayedPagesCount - 1];
            lastDisplayedView.ForceTextUpdate();

            if (lastDisplayedView.MaxVisibleCharacters >=
                lastDisplayedView.CharacterCount)
            {
                if (_displayedPagesCount >= PageCount)
                    return UniTask.CompletedTask;

                ++_displayedPagesCount;
            }

            if (_curViewIndex != _displayedPagesCount - 1)
            {
                _linkedViews[_curViewIndex].Alpha = 0;
                _curViewIndex = _displayedPagesCount - 1;
            }

            var curView = _linkedViews[_curViewIndex];
            curView.Alpha = 1;
            curView.ForceTextUpdate();
            
            var from = curView.PrevTextView? 
                Mathf.Max(curView.PrevTextView.CharacterCount, 
                    curView.MaxVisibleCharacters):
                curView.MaxVisibleCharacters;
            var to = curView.CharacterCount - 1;
            curView.MaxVisibleCharacters = curView.CharacterCount;
            
            return curView.AnimateTextAsync(from, to, animationCancelToken);
        }

        #endregion

        #region Private

        protected override void OnPageSelected(int page)
        {
            
        }

        private void ExpandLinkedViews()
        {
            _lastLinkedView.ForceTextUpdate();
            while (_lastLinkedView.IsTextOverflowing)
            {
                _lastLinkedView.LinkNextTextView(_textViewPool.Rent());
                _linkedViews.Add(_lastLinkedView.NextTextView);
                _lastLinkedView = _lastLinkedView.NextTextView;
                _lastLinkedView.name = $"Page_{_linkedViews.Count}";
                _lastLinkedView.Alpha = 0;
            }
        }
        
        #endregion
        
        #endregion
    }
}