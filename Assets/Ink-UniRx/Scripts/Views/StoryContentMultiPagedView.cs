using System.Threading;
using Cysharp.Threading.Tasks;

namespace InkUniRx.Views
{
    public class StoryContentMultiPagedView: StoryContentPagedView
    {
        #region Internals

        

        #endregion
        
        #region Inspector

        

        #endregion

        #region Properties

        public override string ContentText { get; }
        
        public override bool IsEmpty { get; }
        
        public override int PageCount { get; }
        
        public override int CurrentPage { get; }

        #endregion

        #region Variables

        

        #endregion

        #region Methods

        #region Unity Callbacks

        

        #endregion

        #region Public

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

        #endregion

        #region Private

        

        #endregion

        #endregion
    }
}