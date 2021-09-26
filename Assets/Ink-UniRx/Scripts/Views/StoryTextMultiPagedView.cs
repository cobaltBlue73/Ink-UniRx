using System.Threading;
using Cysharp.Threading.Tasks;

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

        public override UniTask ShowNewContentAsync(CancellationToken animationCancelToken)
        {
            throw new System.NotImplementedException();
        }

        public override int PageCount { get; }
        public override int CurrentPage { get; }
    }
}