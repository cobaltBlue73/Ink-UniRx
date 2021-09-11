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
        public async UniTask OnShowStoryChoicesAsync(StoryChoice[] storyChoices, CancellationToken ct)
        {
            var cells = new List<ScrollViewCell>();
            
            for (int i = 0; i < storyChoices.Length; i++)
            {
                await scrollView.AddStoryElementAsync(storyChoices[i]);
            }
           
            await storyChoices.First().Story.OnMakeChoiceAsObservable().ToUniTask(true);
            for (int i = 0; i < storyChoices.Length; i++)
            {
                await scrollView.RemoveLastCellAsync(false);
            }
        }
    }
}