using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryContentMultiElementScrollView: StoryContentScrollView
    {
        #region Internal

        private class ElementViewPool: ObjectPool<StoryTextElementView>
        {
            private readonly StoryContentMultiElementScrollView _owner;
            private readonly StoryTextElementView _template;
            private readonly Transform _parent;
            
            public ElementViewPool(StoryContentMultiElementScrollView owner)
            {
                _owner = owner;
                this.AddTo(owner);
                _template = _owner.elementViewTemplate;
                _template.ClearText();
                _template.gameObject.SetActive(false);
                _parent = owner.scrollRect.content;
            }

            protected override void OnBeforeRent(StoryTextElementView instance)
            {
                _owner._activeViews.Add(instance);
                instance.ClearText();
                instance.MaxVisibleCharacters = 0;
                base.OnBeforeRent(instance);
            }

            protected override void OnBeforeReturn(StoryTextElementView instance)
            {
                base.OnBeforeReturn(instance);
                instance.ClearText();
            }

            protected override StoryTextElementView CreateInstance()
            {
                var elementViewInstance = Instantiate(_template, _parent);
                return elementViewInstance;
            }

            public void ReturnAll()
            {
                _owner._activeViews.ForEach(Return);
                _owner._activeViews.Clear();
            }
        }
        
        private struct ElementViewData
        {
            public string Text;
        }

        #endregion
        
        #region Inspector

        [SerializeField, Required, InlineEditor]
        private StoryTextElementView elementViewTemplate;

        #endregion

        #region Properties

        public override string ContentText => _text;
        public override bool IsEmpty => _elements.Count <= 0;

        #endregion

        #region Variables

        private string _text = string.Empty;
        private string _whiteSpaceBuffer = string.Empty;
        private ElementViewPool _elementViewPool;
        private int _visibleElementsCount;
        private readonly List<StoryTextElementView> _activeViews = new List<StoryTextElementView>();
        private readonly List<StoryTextElementView> _animatingViews = new List<StoryTextElementView>();
        private readonly List<ElementViewData> _elements = new List<ElementViewData>();

        #endregion

        #region Methods

        #region Unity Callbacks

        protected override void Awake()
        {
            _elementViewPool = new ElementViewPool(this);
            base.Awake();
        }

        #endregion

        #region Public

        public override void AddContent(string contentText)
        {
            _text += contentText;

            if (string.IsNullOrWhiteSpace(contentText))
            {
                
                _whiteSpaceBuffer += string.IsNullOrEmpty(_whiteSpaceBuffer)? 
                    contentText: $"\n{contentText}";
                return;
            }

            var newText = string.Empty;
            
            if (!string.IsNullOrEmpty(_whiteSpaceBuffer))
            {
                newText = $"{_whiteSpaceBuffer}\n";
                _whiteSpaceBuffer = string.Empty;
            }

            newText += contentText;
            
            _elements.Add(new ElementViewData
            {
                Text = newText,
            });
        }

        public override void ClearContent()
        {
            base.ClearContent();
            _text = string.Empty;
            _elementViewPool.ReturnAll();
            _elements.Clear();
            _visibleElementsCount = 0;
        }

        #endregion

        #region Protected

        protected override UniTask ShowNewTextAsync(CancellationToken animationCancelToken)
        {
            if (_visibleElementsCount >= _elements.Count)
                return UniTask.CompletedTask;
            
            _animatingViews.Clear();
            for (; _visibleElementsCount < _elements.Count; ++_visibleElementsCount)
            {
                var element = _elements[_visibleElementsCount];
                var view = _elementViewPool.Rent();
                view.Text = element.Text;
                view.ForceTextUpdate();
                _animatingViews.Add(view);
            }

            return UniTask.WhenAll(_animatingViews.Select(view =>
                view.AnimateTextAsync(0, view.CharacterCount - 1, animationCancelToken)));
        }

        #endregion
        
        #endregion
    }
}