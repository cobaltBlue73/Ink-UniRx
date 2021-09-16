using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Ink.Runtime;
using InkUniRx.ViewModels;
using TMPro;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace InkUniRx.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public class StoryChoicesView : StoryElementView<StoryChoices>
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
        
        [SerializeField] private StoryChoiceView choiceViewTemplate;
        [SerializeField] private Transform childRoot;
        [SerializeField] private CanvasGroup canvasGroup;
        
        #endregion

        #region Properties

        public override StoryChoices StoryElement => _storyChoices;

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

        public override void SetStoryElement(StoryChoices element)
        {
            ClearAll();
            _storyChoices = element;
           
            for (var i = 0; i < _storyChoices.Count; i++)
            {
                var view = _choiceViewPool.Rent();
                view.SetChoice(_storyChoices[i]);
            }

            if (_storyChoices.SelectedChoice != null) return;

            canvasGroup.interactable = true;
            _choiceViews.Select(view=> view.WhenSelected)
                .Merge().First().Subscribe(index=> {
                    canvasGroup.interactable = false;
                    _storyChoices.SelectChoice(index);
                }).AddTo(_disposables);
        }

        #endregion

        #region Private

        private void ClearAll()
        {
            canvasGroup.interactable = false;
            _storyChoices = null;
            _disposables.Clear();
            _choiceViewPool.ReturnAll();
        }

        #endregion

        #endregion
        
       
    }
}