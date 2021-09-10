using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.ViewModels;
using InlUniRx.Views;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class StoryScrollViewContentPresenter : MonoBehaviour, IStoryContentPresenter
    {
        #region Inspector

        [SerializeField] private StoryScrollView scrollView;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!scrollView)
                scrollView = GetComponentInChildren<StoryScrollView>();
        }

        #endregion
        public UniTask OnShowStoryContentAsync(StoryContent storyContent, CancellationToken ct)
        {
            var cell = scrollView.AddStoryElement(storyContent);
            return UniTask.CompletedTask;
        }
    }
}