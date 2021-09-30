using Cysharp.Threading.Tasks;
using InkUniRx.Presenters.Events;
using InkUniRx.Views;
using UniRx;

namespace InkUniRx.Presenters
{
    public class StoryContentPagedViewPresenter: StoryContentViewPresenter<StoryContentPagedView>
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

            do
            {
                await contentView.ShowNewContentAsync(newStoryContent.CancelAnimationToken);
                
                await WaitForContinueAsync(newStoryContent);
                
            } while (contentView.LastDisplayedPage < contentView.PageCount && 
                     !newStoryContent.CancelStoryToken.IsCancellationRequested);
            
            return Unit.Default;
        }
    }
}