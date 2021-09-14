using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.ViewModels;
using InkUniRx.Views;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class ScrollViewStoryChoicesPresenter : MonoBehaviour, IStoryChoicesPresenter
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
        public async UniTask OnShowStoryChoicesAsync(StoryChoices storyChoices, CancellationToken ct)
        {
            var cell = scrollView.AddStoryElement(storyChoices);
            await scrollView.JumpToCellAsync(cell.Index, 1, 1);
            await storyChoices.WhenChoiceSelected.ToUniTask(true);
            //scrollView.RemoveLastCell();
        }
    }
}