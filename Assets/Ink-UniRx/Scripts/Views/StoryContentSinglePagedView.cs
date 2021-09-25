using UnityEngine;

namespace InkUniRx.Views
{
    public class StoryContentSinglePagedView: StoryContentPagedView
    {
        [SerializeField] private StoryTextView textView;

        public override string ContentText => textView.Text;
        public override bool IsEmpty => textView.IsEmpty;
        
        public override void ClearContent()
        {
            textView.ClearText();
            textView.MaxVisibleCharacters = 0;
            
        }

        public override void AddContent(string contentText)
        {
            throw new System.NotImplementedException();
        }
    }
}