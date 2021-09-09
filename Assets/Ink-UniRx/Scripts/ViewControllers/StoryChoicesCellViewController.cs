using System;
using System.Collections.Generic;
using Ink.Runtime;
using InkUniRx.ViewModels;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace InlUniRx.ViewControllers
{
    public class StoryChoicesCellViewController : StoryElementCellViewController
    {
        #region Internals

        private class ChoiceViewPool: ObjectPool<StoryChoiceViewController>
        {
            private readonly Transform _parent;
            private readonly StoryChoiceViewController _template;

            public ChoiceViewPool(StoryChoiceViewController template, Transform parent)
            {
                _template = template;
                _template.gameObject.SetActive(false);
                _parent = parent;
            }
            
            protected override StoryChoiceViewController CreateInstance() => 
                Instantiate(_template, _parent);
        }

        #endregion
        
        #region Inspector
        
        [SerializeField] private StoryChoiceViewController choiceViewTemplate;
        [SerializeField] private LayoutGroup choiceViewsLayout;
        [SerializeField, HideInInspector] private RectTransform layoutRectTrans;
        
        #endregion

        #region Member Variables

        #region Private

        private ChoiceViewPool _choiceViewPool;
        private readonly List<StoryChoiceViewController> _choiceViews = new List<StoryChoiceViewController>();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        #endregion
        
        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!choiceViewsLayout)
                choiceViewsLayout = GetComponentInChildren<LayoutGroup>();

            if (!choiceViewTemplate)
                choiceViewTemplate = GetComponentInChildren<StoryChoiceViewController>();
            
            if (choiceViewsLayout)
                layoutRectTrans = choiceViewsLayout.transform as RectTransform;
        }

        private void Awake()
        {
            _choiceViewPool = new ChoiceViewPool(choiceViewTemplate, layoutRectTrans);
            _choiceViewPool.AddTo(this);
            _disposables.AddTo(this);
        }

        private void OnDisable()
        {
            _disposables.Clear();
            ClearChoiceViews();
        }

        #endregion

        public override void SetCell(ScrollViewCell cell)
        {
            var choices = cell.StoryPathElement as StoryPathChoices;
            _disposables.Clear();
            ClearChoiceViews();
            if(choices == null) return;
            
            IObservable<Choice> whenSelected = null;
            
            foreach (var choice in choices.Choices)
            {
                var choiceView = _choiceViewPool.Rent();
                choiceView.SetChoice(choice);
                _choiceViews.Add(choiceView);
                switch (whenSelected)
                {
                    case null:
                        whenSelected = choiceView.WhenChoiceSelected;
                        break;
                    default:
                        whenSelected = whenSelected.Merge(choiceView.WhenChoiceSelected);
                        break;
                }
            }

            whenSelected.First()
                .Subscribe(c => choices.Select(c.index))
                .AddTo(_disposables);
        }

        public override float GetCellViewSize()
        {
            if (!layoutRectTrans) return 0;
            
            Canvas.ForceUpdateCanvases();

            return layoutRectTrans.rect.height;
        }

        private void ClearChoiceViews()
        {
            _choiceViews.ForEach(_choiceViewPool.Return);
            _choiceViews.Clear();
        }
    }
}