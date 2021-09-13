using System;
using System.Collections.Generic;
using Ink.Runtime;
using InkUniRx.ViewModels;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public class StoryChoicesView : StoryElementViewBase
    {
       #region Internals

        private class ChoiceViewPool: ObjectPool<StoryChoiceView>
        {
            private readonly StoryChoicesView _owner;
            private readonly StoryChoiceView _template;
            private readonly Transform _root;

            public ChoiceViewPool(StoryChoicesView owner)
            {
                this.AddTo(owner);
                _owner = owner;
                _template = _owner.choiceViewTemplate;
                _template.gameObject.SetActive(false);
                _root = _owner.childRoot;
            }
            
            protected override StoryChoiceView CreateInstance() => 
                Instantiate(_template, _root);
        }

        #endregion
        
        #region Inspector
        
        [SerializeField] private StoryChoiceView choiceViewTemplate;
        [SerializeField] private Transform childRoot;
        [SerializeField] private CanvasGroup canvasGroup;
        
        #endregion

        #region Properties

        public override StoryElement StoryElement => _storyChoices;

        #endregion

        #region Member Variables

        #region Private

        private StoryChoices _storyChoices;
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
            
            if (!childRoot)
            {
                var layout = GetComponentInChildren<LayoutGroup>();
                if(layout) 
                    childRoot = layout.transform;
            }
        }

        private void Awake()
        {
            _choiceViewPool = new ChoiceViewPool(this);
            _disposables.AddTo(this);
        }

        private void OnDisable()
        {
            ClearAll(); 
        }

        #endregion

        #region Methods

        #region Public

        public override void SetStoryElement(StoryElement element)
        {
            ClearAll();
            if(!(element is StoryChoices choices)) return;

            _storyChoices = choices;
            IObservable<int> whenSelected = null;
            
            for (int i = 0; i < choices.Count; i++)
            {
                var view = _choiceViewPool.Rent();
                
                view.SetChoice(choices[i]);
                _choiceViews.Add(view);
                
                whenSelected = whenSelected != null ? 
                    whenSelected.Merge(view.WhenSelected) : 
                    view.WhenSelected;
            }
            
            whenSelected.First().Subscribe(choices.SelectChoice)
                .AddTo(_disposables);
        }

        #endregion

        #region Private

        private void ClearAll()
        {
            _storyChoices = null;
            _disposables.Clear();
            _choiceViews.ForEach(_choiceViewPool.Return);
            _choiceViews.Clear();
        }

        #endregion

        #endregion
        
       
    }
}