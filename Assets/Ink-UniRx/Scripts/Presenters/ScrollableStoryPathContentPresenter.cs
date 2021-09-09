using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.ViewModels;
using InlUniRx.ViewControllers;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class ScrollableStoryPathContentPresenter : MonoBehaviour, IStoryPathContentPresenter
    {
        #region Inspector

        [SerializeField] private ScrollViewController scrollViewController;
        [SerializeField] private StoryContentCellViewController contentCellViewPrefab;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!scrollViewController)
                scrollViewController = GetComponentInChildren<ScrollViewController>();
        }

        #endregion
        public UniTask OnShowStoryPathContentAsync(StoryPathContent storyPathContent, CancellationToken ct)
        {
            var cell = new ScrollViewCell(storyPathContent, contentCellViewPrefab);
            scrollViewController.AddCell(cell);
            return UniTask.CompletedTask;
        }
    }
}