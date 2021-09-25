using System;
using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Events;
using InkUniRx.Views;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace InkUniRx.Presenters
{
    public class StoryTextScrollViewPresenter: StoryTextViewPresenter<StoryContentScrollView>
    {
        protected override async UniTask<Unit> OnNewStoryTextAsync(StoryPathNewText newStoryText)
        {
            if (newStoryText.Story.CurrentTextIsWhiteSpace())
            {
                if(!ignoreWhiteSpaceText)
                    textView.AddContent(newStoryText.Story.currentText);
                
                return Unit.Default;
            }
             
            textView.AddContent(trim? newStoryText.Story.currentText.Trim(): 
                newStoryText.Story.currentText);

            await textView.ShowNewTextAsync(newStoryText.CancelAnimationToken);

            await WaitForContinueAsync(newStoryText);
            
            return Unit.Default;
        }
    }
}