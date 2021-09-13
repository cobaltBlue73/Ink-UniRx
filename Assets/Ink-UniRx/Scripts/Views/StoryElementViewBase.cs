using InkUniRx.ViewModels;
using TMPro;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryElementViewBase : MonoBehaviour
    {
        public abstract StoryElement StoryElement { get; }
        public abstract void SetStoryElement(StoryElement element);
    }
}