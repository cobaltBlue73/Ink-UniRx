using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.ViewModels;
using InlUniRx.Views;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class StoryScrollViewChoicesPresenter : MonoBehaviour, IStoryChoicesPresenter
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
        public async UniTask OnShowStoryChoicesAsync(StoryChoice[] storyChoices, CancellationToken ct)
        {
            var cells = new List<StoryScrollViewCell>();
            
            for (int i = 0; i < storyChoices.Length; i++)
            {
                cells.Add(scrollView.AddStoryElement(storyChoices[i], i >= storyChoices.Length - 1));
            }
           
            await storyChoices.First().Story.OnMakeChoiceAsObservable().ToUniTask(true);
            for (int i = 0; i < storyChoices.Length; i++)
            {
                scrollView.RemoveLastCell(false);
            }
        }
    }
}