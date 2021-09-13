using System.Collections.Generic;
using Ink.Runtime;
using Sirenix.Utilities;

namespace InkUniRx.ViewModels
{
    public class StoryContent: StoryElement
    {
        public StoryContent(Story story, int lineNumber, bool isPartial = true) : base(story)
        {
            Text = story.currentText;
            Tags = story.currentTags.ToHashSet();
            LineNumber = lineNumber;
            IsPartial = isPartial;
            IsBeginning = !isPartial || lineNumber == 1;
            IsEnding = !isPartial || !story.canContinue;
        }

        public string Text { get; }
        public bool IsPartial { get; }
        public HashSet<string> Tags { get; }
        public int LineNumber { get; }
        public bool IsBeginning { get; } 
        public bool IsEnding { get; }
        
    }
}