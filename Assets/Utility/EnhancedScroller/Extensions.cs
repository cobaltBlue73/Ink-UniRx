using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;

namespace Utility.EnhancedScrollerV2
{
    public static class Extensions
    {
        public static UniTask JumpToIndexAsync(this EnhancedScroller enhancedScroller, int index, float scrollerOffset = 0, float cellOffset = 0, bool useSpacing = true, EnhancedScroller.TweenType tweenType = EnhancedScroller.TweenType.immediate, float tweenTime = 0)
        {
            var tcs = new UniTaskCompletionSource();
            enhancedScroller.JumpToDataIndex(index, scrollerOffset, cellOffset, useSpacing, tweenType, tweenTime, ()=> tcs.TrySetResult());
            return tcs.Task;
        }
    }
}