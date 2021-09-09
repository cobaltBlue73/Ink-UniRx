using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.ViewModels;
using InlUniRx.ViewControllers;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class ScrollableStoryPathChoicesPresenter : MonoBehaviour, IStoryPathChoicesPresenter
    {
        #region Inspector

        [SerializeField] private ScrollViewController scrollViewController;
        [SerializeField] private StoryChoicesCellViewController choicesCellViewPrefab;

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!scrollViewController)
                scrollViewController = GetComponentInChildren<ScrollViewController>();
        }

        #endregion
        public async UniTask OnShowStoryPathChoicesAsync(StoryPathChoices storyPathChoices, CancellationToken ct)
        {
            var cell = new ScrollViewCell(storyPathChoices, choicesCellViewPrefab);
            scrollViewController.AddCell(cell);
            await storyPathChoices.WhenSelected.ToUniTask(true);
            scrollViewController.RemoveLastCell(false);
            
        }
    }
}