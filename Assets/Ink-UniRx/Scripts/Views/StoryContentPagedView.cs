namespace InkUniRx.Views
{
    public abstract class StoryContentPagedView: StoryContentView
    {
        #region Properties
        public abstract int PageCount { get; }
        public abstract int CurrentPage { get; }

        #endregion
    }
}