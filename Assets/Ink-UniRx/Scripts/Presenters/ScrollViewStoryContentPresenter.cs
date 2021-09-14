using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.ViewModels;
using InkUniRx.Views;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class ScrollViewStoryContentPresenter : MonoBehaviour, IStoryContentPresenter
    {
        #region Inspector

        [SerializeField] private ScrollView scrollView;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!scrollView)
                scrollView = GetComponentInChildren<ScrollView>();
        }

        #endregion
        public async UniTask OnShowStoryContentAsync(StoryContent storyContent, CancellationToken ct)
        {
            var cell = scrollView.AddStoryElement(storyContent);
            if (storyContent.IsBeginning)
                await scrollView.JumpToCellAsync(cell.Index);
            else
                await scrollView.JumpToCellAsync(cell.Index, 1, 1);
        }
    }
}