namespace InkUniRx.Presenters.Events
{
    public class ContinueStory: ReaderInputEvent
    {
        private static ContinueStory s_default;
        public static ContinueStory Default => s_default ?? new ContinueStory();
        private ContinueStory() {}
    }
}