using InkUniRx.ViewModels;

namespace InkUniRx.Views
{
    public class StoryContentCellView: ScrollViewWrapperCellView<StoryContentCell, StoryContent, StoryContentView>
    {
        protected override void SetSubView(StoryContent subViewModel) => 
            subView.SetStoryElement(subViewModel);
    }
}