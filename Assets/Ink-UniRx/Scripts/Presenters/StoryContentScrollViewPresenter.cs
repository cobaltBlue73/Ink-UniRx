using System;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Events;
using InkUniRx.Views;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class StoryContentScrollViewPresenter: StoryContentViewPresenter<StoryContentScrollView>
    {
        protected override async UniTask<Unit> OnNewStoryContentAsync(StoryPathNewContent newStoryContent)
        {
            if (newStoryContent.Story.CurrentTextIsWhiteSpace())
            {
                if(!ignoreWhiteSpaceText)
                    contentView.AddContent(newStoryContent.Story.currentText);
                
                return Unit.Default;
            }
             
            contentView.AddContent(trim? newStoryContent.Story.currentText.Trim(): 
                newStoryContent.Story.currentText);

            await contentView.ShowNewContentAsync(newStoryContent.CancelAnimationToken);

            await WaitForContinueAsync(newStoryContent);
            
            return Unit.Default;
        }
    }
}