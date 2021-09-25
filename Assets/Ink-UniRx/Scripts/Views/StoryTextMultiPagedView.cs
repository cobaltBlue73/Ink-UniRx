namespace InkUniRx.Views
{
    public class StoryTextMultiPagedView: StoryContentPagedView
    {
        public override string ContentText { get; }
        public override bool IsEmpty { get; }
        public override void ClearContent()
        {
            throw new System.NotImplementedException();
        }

        public override void AddContent(string contentText)
        {
            throw new System.NotImplementedException();
        }
    }
}