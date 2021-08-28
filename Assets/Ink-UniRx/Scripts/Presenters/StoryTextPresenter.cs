using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        private Ease _ease = Ease.Linear;

        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            _animationStyle = settings.AnimationStyle;
            _ease = settings.Easing;
            settings.TextAnimationSpeed.SetAndSubscribe(ref _animationSpd,
                val => _animationSpd = val).AddTo(this);
        }

        private void Start()
        {
            player.WhenNewLine.Subscribe(_ => OnNewLine())
                .AddTo(this);
            player.SubmitNewLineTransition(this);
            
            if (continueButton)
            {
                continueButton.OnClickAsObservable()
                    .Subscribe(_ => player.ContinueNext())
                    .AddTo(this);
            }
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

            if (continueButton)
            {
                continueButton.gameObject.SetActive(player.StoryVM.Story.canContinue);
            }
        }
        
        private async UniTask AnimateTextByLetterAsync(CancellationToken ct)
        {
            var tweener = text.DOText(player.StoryVM.Story.currentText, _animationSpd)
                .SetEase(_ease).SetSpeedBased().SetRecyclable();

            var isCancelled = await tweener.WithCancellation(ct)
                .SuppressCancellationThrow();

            if(isCancelled) tweener.Complete();
        }
        
        private async UniTask AnimateTextByWordAsync(CancellationToken ct)
        {
            var words = player.StoryVM.Story.currentText.Split(' ');
            var i = 0;
            var tweener = DOTween.To(()=> i, next => {
                    i = next;
                    text.text = string.Concat(text.text, 
                        string.Concat(words[i], ' '));
                }, words.Length, _animationSpd)
                .SetEase(_ease).SetSpeedBased().SetRecyclable();

            var isCancelled = await tweener.WithCancellation(ct)
                .SuppressCancellationThrow();

            if(isCancelled) tweener.Complete();
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