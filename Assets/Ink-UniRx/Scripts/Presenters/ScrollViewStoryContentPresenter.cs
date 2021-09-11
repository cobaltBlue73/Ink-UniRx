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
            await scrollView.AddStoryElementAsync(storyContent);
        }
    }
}