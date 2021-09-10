using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Ink.Runtime;
using InkUniRx.Presenters.Interfaces;
using InkUniRx.Settings;
using InkUniRx.ViewModels;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utility.DoTweenPro;
using Utility.TextMeshPro;
using Utility.UniRx;

namespace InkUniRx.Presenters
{
    public class DialogBoxStoryPresenter : MonoBehaviour, IStoryPathPreStartPresenter, IStoryContentPresenter, IStoryPathPostEndPresenter, IStoryChoicesPresenter
    {
        [SerializeField] private StoryPresenterSettings settings;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button continueButton;
        [SerializeField] private bool trim;
        
        private IObservable<TMP_LinkInfo> _whenLinkClicked;
        private bool _autoContinue;
        private float _autoContinueDelay;
        private float _textAnimSpd;
        private Ease _textAnimEase;

        private void Awake()
        {
            text.overflowMode = TextOverflowModes.Page;
            _whenLinkClicked = text.textInfo.OnPointerClickedLinkAsObservable();

            if (settings)
            {
                settings.AutoContinue.SetAndSubscribe(ref _autoContinue,
                    val => _autoContinue = val).AddTo(this);
                settings.AutoContinueDelay.SetAndSubscribe(ref _autoContinueDelay,
                    val => _autoContinueDelay = val).AddTo(this);

                settings.TextAnimationSpeed.SetAndSubscribe(ref _textAnimSpd,
                    val => _textAnimSpd = val).AddTo(this);
                settings.TextAnimationEasing.SetAndSubscribe(ref _textAnimEase,
                    ease => _textAnimEase = ease);
            }
        }

        private async UniTask AnimateTextAsync(CancellationToken ct)
        {
            text.maxVisibleCharacters = text.textInfo.characterCount;
            var pageInfo = text.textInfo.pageInfo[text.pageToDisplay - 1];
            var from =  pageInfo.firstCharacterIndex;
            var to = text.pageToDisplay >= text.textInfo.pageCount? 
                text.textInfo.characterCount: pageInfo.lastCharacterIndex + 1;

            text.maxVisibleCharacters = from;
            var tween = text.DOVisibleCharacters(to, _textAnimSpd)
                .SetEase(_textAnimEase).SetSpeedBased().SetRecyclable(); 
            await tween.WithCancellation(ct).SuppressCancellationThrow();
            
            if (ct.IsCancellationRequested)
            {
                text.maxVisibleCharacters = text.textInfo.characterCount;
            }
        }
        
        public UniTask OnShowStoryPathPreStartAsync(Story story, CancellationToken ct)
        {
            continueButton.gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public async UniTask OnShowStoryContentAsync(StoryContent storyContent, CancellationToken ct)
        {
            text.text = trim? storyContent.Text.Trim(): storyContent.Text;
            text.ForceMeshUpdate();

            for (int page = 1; page <= text.textInfo.pageCount && !ct.IsCancellationRequested; page++)
            {
                text.pageToDisplay = page;
                await AnimateTextAsync(ct);
                
                if (!storyContent.IsEnding && text.pageToDisplay >= text.textInfo.pageCount) break;

                if (!_autoContinue)
                {
                    await continueButton.OnClickAsync(ct);
                    continue;
                }

                await UniTask.WhenAny(UniTask.Delay(TimeSpan.FromSeconds(_autoContinueDelay),
                        false, PlayerLoopTiming.Update, ct),
                    continueButton.OnClickAsync(ct));

            }
        }

        public UniTask OnShowStoryPathPostEndAsync(Story story, CancellationToken ct)
        {
            continueButton.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }

        public async UniTask OnShowStoryChoicesAsync(StoryChoice[] storyChoices, CancellationToken ct)
        {
            foreach (var choice in storyChoices)
            {
                text.text += $"\n<align=center><link={choice.Index}>{choice.Text}</link></align>";
            }
            text.ForceMeshUpdate();
            text.maxVisibleCharacters = text.textInfo.characterCount;
            var linkInfo = await _whenLinkClicked.ToUniTask(true);
            var choiceIdx = int.Parse(linkInfo.GetLinkID());
            storyChoices[choiceIdx].Select();
        }
    }
}