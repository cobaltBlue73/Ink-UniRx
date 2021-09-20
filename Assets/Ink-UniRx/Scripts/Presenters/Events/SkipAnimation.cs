namespace InkUniRx.Presenters.Events
{
    public class SkipAnimation: ReaderInputEvent
    {
        private static SkipAnimation s_default;
        public static SkipAnimation Default => s_default ?? new SkipAnimation();
        private SkipAnimation() {}
    }
}