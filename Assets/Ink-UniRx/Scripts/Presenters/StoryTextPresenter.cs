using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utility.UniRx;

namespace InkUniRx
{
    public class StoryTextPresenter : MonoBehaviour, INewLineTransition
    {
        #region Inspector

        [SerializeField] private StoryTextPresenterSettings settings;
        [SerializeField] private StoryPlayer player;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private Button continueButton;

        #endregion

        #region Member Variables

        private StoryTextPresenterSettings.AnimationStyleType _animationStyle = 
            StoryTextPresenterSettings.AnimationStyleType.None;

        private float _animationSpd;
        private string _targetText;

        #endregion
       

        #region Unity Callbacks

        private void Awake()
        {
            _animationStyle = settings.AnimationStyle;
            settings.TextAnimationSpeed.SetAndSubscribe(ref _animationSpd,
                val => _animationSpd = val).AddTo(this);
        }

        private void Start()
        {
            player.WhenNewLine.Subscribe(_ => OnNewLine()).AddTo(this);
            player.SubmitNewLineTransition(this);
        } 

        #endregion

        #region Private Methods

        private void OnNewLine()
        {
            text.text = "";
            if (_animationStyle == StoryTextPresenterSettings.AnimationStyleType.None)
            {
                text.text = player.StoryVM.Story.currentText;
            }
        }
        
        private async UniTask AnimateTextByLetterAsync(CancellationToken ct)
        {
            var targetChars = player.StoryVM.Story.currentText.ToCharArray();
            var i = 0; 
        }
        
        private async UniTask AnimateTextByWordAsync(CancellationToken ct)
        {
            
        }
        
        #endregion

        #region Interface Methods

        public async UniTask PlayNewLineTransitionAsync(CancellationToken ct)
        {
            switch (_animationStyle)
            {
                case StoryTextPresenterSettings.AnimationStyleType.ByLetter:
                    await AnimateTextByLetterAsync(ct);
                    break;
                case StoryTextPresenterSettings.AnimationStyleType.ByWord:
                    await AnimateTextByWordAsync(ct);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}