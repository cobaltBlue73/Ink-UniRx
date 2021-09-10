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

        public sealed override string Text { get; protected set; }

        public bool IsPartial { get; private set; }
        public HashSet<string> Tags { get; private set; }
        public int LineNumber { get; private set; }
        public bool IsBeginning { get; private set; } 
        public bool IsEnding { get; private set; }
        
    }
}