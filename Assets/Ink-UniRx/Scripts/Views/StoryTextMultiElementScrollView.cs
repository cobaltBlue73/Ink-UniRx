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
    public class StoryTextMultiElementScrollView: StoryTextScrollView
    {
        #region Internal

        private class ElementViewPool: ObjectPool<StoryTextScrollElementView>
        {
            private readonly StoryTextMultiElementScrollView _owner;
            private readonly StoryTextScrollElementView _template;
            private readonly Transform _parent;
            
            public ElementViewPool(StoryTextMultiElementScrollView owner)
            {
                _owner = owner;
                this.AddTo(owner);
                _template = _owner.elementViewTemplate;
                _template.ClearText();
                _template.gameObject.SetActive(false);
                _parent = owner.scrollRect.content;
            }

            protected override void OnBeforeRent(StoryTextScrollElementView instance)
            {
                _owner._activeViews.Add(instance);
                instance.ClearText();
                base.OnBeforeRent(instance);
            }

            protected override void OnBeforeReturn(StoryTextScrollElementView instance)
            {
                base.OnBeforeReturn(instance);
                instance.ClearText();
            }

            protected override StoryTextScrollElementView CreateInstance()
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
            public int ViewIndex;
        }

        #endregion
        
        #region Inspector

        [SerializeField, Required, InlineEditor]
        private StoryTextScrollElementView elementViewTemplate;

        #endregion

        #region Properties

        public override string Text => _text;
        public override bool IsEmpty => _elements.Count <= 0;

        #endregion

        #region Variables

        private string _text = string.Empty;
        private string _whiteSpaceBuffer = string.Empty;
        private ElementViewPool _elementViewPool;
        private int _visibleElementsCount;
        private readonly List<StoryTextScrollElementView> _activeViews = new List<StoryTextScrollElementView>();
        private readonly List<StoryTextScrollElementView> _animatingViews = new List<StoryTextScrollElementView>();
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

        public override void AddText(string text)
        {
            _text += text;

            if (string.IsNullOrWhiteSpace(text))
            {
                _whiteSpaceBuffer += $"\n{text}";
                return;
            }

            var newText = string.Empty;
            
            if (_whiteSpaceBuffer.Length > 0)
            {
                newText = $"{_whiteSpaceBuffer}\n";
                _whiteSpaceBuffer = string.Empty;
            }

            newText += text;
            
            _elements.Add(new ElementViewData
            {
                Text = newText,
                ViewIndex = -1
            });
        }

        public override void ClearText()
        {
            base.ClearText();
            _text = string.Empty;
            _elementViewPool.ReturnAll();
            _elements.Clear();
            _visibleElementsCount = 0;
        }

        #endregion

        #region Protected

        protected override UniTask PlayTextAnimationsAsync(CancellationToken cancelAnimationToken)
        {
            _animatingViews.Clear();
            for (; _visibleElementsCount < _elements.Count; ++_visibleElementsCount)
            {
                var element = _elements[_visibleElementsCount];
                var view = _elementViewPool.Rent();
                view.AddText(element.Text);
                view.TextMesh.ForceMeshUpdate();
                _animatingViews.Add(view);
            }

            return UniTask.WhenAll(_animatingViews.Select(view =>
                view.PlayTextAnimationsAsync(0, view.CharacterCount - 1, cancelAnimationToken)));
        }

        #endregion
        
        #endregion
    }
}