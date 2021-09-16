using InkUniRx.ViewModels;
using TMPro;
using UnityEngine;

namespace InkUniRx.Views
{
    public abstract class StoryElementView<TViewModel> : MonoBehaviour where TViewModel: StoryElement
    {
        public abstract TViewModel StoryElement { get; }
        public abstract void SetStoryElement(TViewModel element);
    }
}