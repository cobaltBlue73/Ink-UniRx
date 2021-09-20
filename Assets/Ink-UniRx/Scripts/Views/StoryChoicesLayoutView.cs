using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public class StoryChoicesLayoutView: StoryChoicesView
    {
         #region Internals

        private class ChoiceViewPool: ObjectPool<StoryChoiceView>
        {
            private readonly StoryChoicesLayoutView _owner;
            private readonly StoryChoiceView _template;
            private readonly Transform _parent;

            public ChoiceViewPool(StoryChoicesLayoutView owner)
            {
                this.AddTo(owner);
                _owner = owner;
                _template = _owner.choiceViewTemplate;
                _template.gameObject.SetActive(false);
                _parent = _owner.choiceViewsLayout.transform;
            }
            
            protected override StoryChoiceView CreateInstance() => 
                Instantiate(_template, _parent);

            protected override void OnBeforeRent(StoryChoiceView instance)
            {
                instance.transform.SetAsLastSibling();
                base.OnBeforeRent(instance);
                _owner._choiceViews.Add(instance);
            }

            public void ReturnAll()
            {
                _owner._choiceViews.ForEach(Return);
                _owner._choiceViews.Clear();
            }
        }

        #endregion
        
        #region Inspector
        
        [SerializeField, Required] private StoryChoiceView choiceViewTemplate;
        [SerializeField, Required] private LayoutGroup choiceViewsLayout;
        [SerializeField, Required] private CanvasGroup canvasGroup;
        
        #endregion

        #region Properties

        public override bool Interactable
        {
            get => canvasGroup.interactable;
            set => canvasGroup.interactable = value;
        }

        public override IObservable<Choice> WhenChoiceSelected => 
            _choiceViews.Select(cv => cv.WhenSelected)
                .Merge().First();

        #endregion

        #region Member Variables

        #region Private
        
        private ChoiceViewPool _choiceViewPool;
        private readonly List<StoryChoiceView> _choiceViews = new List<StoryChoiceView>();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        #endregion
        
        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!choiceViewTemplate)
                choiceViewTemplate = GetComponentInChildren<StoryChoiceView>();

            if (!canvasGroup)
                canvasGroup = GetComponent<CanvasGroup>();

            if (!choiceViewsLayout)
                choiceViewsLayout = GetComponentInChildren<LayoutGroup>();
        }

        private void Awake()
        {
            _choiceViewPool = new ChoiceViewPool(this);
            _disposables.AddTo(this);
        }

        private void OnDisable() => ClearAll();

        #endregion

        #region Methods

        #region Public
        
        public override void SetChoices(IEnumerable<Choice> choices)
        {
            ClearAll();
            foreach (var choice in choices)
            {
                var view = _choiceViewPool.Rent();
                view.SetChoice(choice);
            }
        }

        #endregion

        #region Private

        private void ClearAll()
        {
            canvasGroup.interactable = false;
            _disposables.Clear();
            _choiceViewPool.ReturnAll();
        }

        #endregion

        #endregion
    }
}