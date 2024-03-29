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

        public override int LastDisplayedPage => _lastDisplayedPage;

        #endregion

        #region Variables
        
        private StoryTextLinkedView _firstLinkedView;
        private StoryTextLinkedView _lastLinkedView;
        private int _curViewIndex = 0;
        private int _lastDisplayedPage = 1;
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
            _lastDisplayedPage = 1;
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
            _firstLinkedView.Text += contentText;
            ExpandLinkedViews();
        }

        public override async UniTask ShowNewContentAsync(CancellationToken animationCancelToken)
        {
            var lastDisplayedView = _linkedViews[_lastDisplayedPage - 1];

            var from = lastDisplayedView.MaxVisibleCharacters;
            var to = lastDisplayedView.CharacterCount - 1;

            if (PageCount > 1)
            {
                if (lastDisplayedView.MaxVisibleCharacters >=
                    lastDisplayedView.CharacterCount)
                {
                    if (_lastDisplayedPage >= PageCount)
                        return;

                    ++_lastDisplayedPage;
                    lastDisplayedView = _linkedViews[_lastDisplayedPage - 1];
                    from = lastDisplayedView.PrevTextView.CharacterCount;
                    to = lastDisplayedView.CharacterCount - 1;
                }

                SetPage(_lastDisplayedPage);
            }
            
            lastDisplayedView.MaxVisibleCharacters = to + 1;

            paginationView.Interactable = false;
            await lastDisplayedView.AnimateTextAsync(from, to, animationCancelToken);
            paginationView.Interactable = true;
        }

        #endregion

        #region Private

        protected override void OnPageSelected(int page)
        {
            var newIndex = page - 1;
            if(newIndex < 0 || 
               newIndex >= _linkedViews.Count || 
               newIndex == _curViewIndex) return;
            
            _linkedViews[_curViewIndex].Alpha = 0;
            _curViewIndex = newIndex;
            _linkedViews[_curViewIndex].Alpha = 1;
        }

        protected override void SetPage(int page)
        {
            base.SetPage(page);
            OnPageSelected(page);
        }

        private void ExpandLinkedViews()
        {
            _linkedViews.ForEach(view=> view.ForceTextUpdate());
            
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