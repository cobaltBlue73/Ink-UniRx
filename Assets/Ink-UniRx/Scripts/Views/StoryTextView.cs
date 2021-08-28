using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using Utility.UniRx;

namespace InkUniRx
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class StoryTextView : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private StoryTextViewSettings settings; 
        [SerializeField] private TextMeshProUGUI text;

        #endregion

        #region Member Variables

        private StoryTextViewSettings.AnimationStyleType _animationStyle = 
            StoryTextViewSettings.AnimationStyleType.None;

        private bool _trim;
        private float _animationSpd;
        private string _targetTxt;
        private TweenParams _params;
        
        #endregion
        
        #region Unity Callbacks

        private void Awake()
        {
            _animationStyle = settings.AnimationStyle;
            _trim = settings.Trim;
            settings.TextAnimationSpeed.SetAndSubscribe(ref _animationSpd,
                val => _animationSpd = val).AddTo(this);
            _params = new TweenParams().SetEase(settings.Easing)
                .SetSpeedBased().SetRecyclable();
        }

        #endregion
        
        #region Public Methods

        public void SetText(string storyText)
        {
            switch (_animationStyle)
            {
                case StoryTextViewSettings.AnimationStyleType.None:
                    text.text = _trim? storyText.Trim(): storyText;
                    break;
                case StoryTextViewSettings.AnimationStyleType.ByLetter:
                case StoryTextViewSettings.AnimationStyleType.ByWord:
                    text.text = "";
                    _targetTxt = _trim? storyText.Trim(): storyText;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
           
        }

        public async UniTask AnimateTextAsync(CancellationToken ct)
        {
            switch (_animationStyle)
            {
                case StoryTextViewSettings.AnimationStyleType.ByLetter:
                    await AnimateTextByLetterAsync(ct);
                    break;
                case StoryTextViewSettings.AnimationStyleType.ByWord:
                    await AnimateTextByWordAsync(ct);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion

        #region Private Methods

        private async UniTask AnimateTextByLetterAsync(CancellationToken ct)
        {
            var tweener = text.DOText(_targetTxt, _animationSpd).SetAs(_params);

            var isCancelled = await tweener.WithCancellation(ct)
                .SuppressCancellationThrow();

            if(isCancelled) tweener.Complete();
        }
        
        private async UniTask AnimateTextByWordAsync(CancellationToken ct)
        {
            var words = _targetTxt.Split(' ');
            var i = 0;
            var tweener = DOTween.To(() => i, next =>
            {
                i = next;
                text.text = string.Concat(text.text,
                    string.Concat(words[i], ' '));
            }, words.Length, _animationSpd).SetAs(_params);

            var isCancelled = await tweener.WithCancellation(ct)
                .SuppressCancellationThrow();

            if(isCancelled) tweener.Complete();
        }
        
        #endregion
    }
}