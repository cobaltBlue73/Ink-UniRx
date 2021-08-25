using Ink.Runtime;
using UnityEngine;

namespace InkUniRx
{
    [CreateAssetMenu(fileName = nameof(StoryVM), menuName = "Ink-UniRx/StoryVM", order = 0)]
    public class StoryVM : ScriptableObject
    {
        #region Inspector

        [SerializeField] private TextAsset inkStoryTextAsset;

        #endregion

        #region Properties

        public Story Story => inkStoryTextAsset? _story ??= new Story(inkStoryTextAsset.text): null;

        #endregion

        #region Member Variables

        private Story _story;

        #endregion
       
        
    }
}