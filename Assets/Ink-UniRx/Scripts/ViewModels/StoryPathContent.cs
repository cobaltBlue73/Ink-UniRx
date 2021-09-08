using System.Collections.Generic;
using Ink.Runtime;
using Sirenix.Utilities;

namespace InkUniRx.ViewModels
{
    public class StoryPathContent: StoryPathElement
    {
        public StoryPathContent(Story story, int lineNumber, bool isPartial = true) 
            : base(story)
        {
            Text = story.currentText;
            Tags = story.currentTags.ToHashSet();
            LineNumber = lineNumber;
            IsPartial = isPartial;
            IsBeginning = !isPartial || lineNumber == 1;
            IsEnding = !isPartial || !story.canContinue;
        }
        
        public string Text { get; private set; }
        public bool IsPartial { get; private set; }
        public HashSet<string> Tags { get; private set; }
        public int LineNumber { get; private set; }
        public bool IsBeginning { get; private set; } 
        public bool IsEnding { get; private set; }
        
    }
}