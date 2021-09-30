using System;
using UniRx;
using UnityEngine;
using Utility.General.UI;

namespace InkUniRx.Views
{
    public abstract class StoryContentPagedView: StoryContentView
    {
        #region Inspector

        [SerializeField] protected PaginationView paginationView;

        #endregion
        #region Properties
        public abstract int PageCount { get; }
        public abstract int CurrentPage { get; }
        
        public abstract int LastDisplayedPage { get; }
        #endregion

        #region Unity CallBacks

        protected virtual void Reset()
        {
            if (!paginationView)
                paginationView = GetComponentInChildren<PaginationView>();
        }

        protected virtual void Awake()
        {
            if (paginationView)
                paginationView.WhenPageSelected
                    .Subscribe(OnPageSelected)
                    .AddTo(this);
        }

        #endregion

        #region Methods

        protected virtual void SetPage(int page)
        {
            if(paginationView)
                paginationView.SetCurrentPage(page);
        }

        protected abstract void OnPageSelected(int page);

        #endregion
    }
}