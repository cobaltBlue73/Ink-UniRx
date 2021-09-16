using InkUniRx.ViewModels;

namespace InkUniRx.Views
{
    public class StoryChoicesCellView: ScrollViewWrapperCellView<StoryChoicesCell, StoryChoices, StoryChoicesView>
    {
        protected override void SetSubView(StoryChoices subViewModel) => 
            subView.SetStoryElement(subViewModel);
    }
}