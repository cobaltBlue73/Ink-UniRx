using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utility.General;

namespace Utility.TextMeshPro
{
    [RequireComponent(typeof(TMP_Text)), DisallowMultipleComponent]
    public abstract class TMPTextEventTriggerBase<T> : MonoBehaviour where T: struct
    {
        #region Inspector

        [SerializeField] private TMP_Text text;
        [SerializeField] private UnityEvent<T> onPointerEnter = new UnityEvent<T>();
        [SerializeField] private UnityEvent<T> onPointerExit = new UnityEvent<T>();
        [SerializeField] private UnityEvent<T> onPointerClicked = new UnityEvent<T>();

        #endregion

        #region Properties

        public TMP_Text Text => text ??= this.GetOrAddComponent<TMP_Text>();

        public UnityEvent<T> ONPointerEnter => onPointerEnter;

        public UnityEvent<T> ONPointerExit => onPointerExit;

        public UnityEvent<T> ONPointerClicked => onPointerClicked;

        #endregion

        #region Member Variables

        protected Camera Camera;
        
        private int _lastSelectedIndex = -1;

        #endregion


        #region Unity Callbacks

        private void OnValidate()
        {
            if (!Text) text = this.GetOrAddComponent<TMP_Text>();
        }
        
        private void Awake()
        {
            if (Text.GetType() == typeof(TextMeshProUGUI))
            {
                var canvas = gameObject.GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    Camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? 
                        null : canvas.worldCamera;
                }
            }
            else
            {
                Camera = Camera.main;
            }

            this.LateUpdateAsObservable()
                .Subscribe(ONLateUpdate)
                .AddTo(this);

            this.GetOrAddComponent<ObservableEventTrigger>()
                .OnPointerClickAsObservable()
                .Subscribe(ONPointerClick)
                .AddTo(this);
        }

        private void ONLateUpdate(Unit _)
        {
            if (!TMP_TextUtilities.IsIntersectingRectTransform(Text.rectTransform, Input.mousePosition, Camera))
            {
                if (_lastSelectedIndex != -1)
                {
                    ONPointerExit?.Invoke(GetTextElementInfo(_lastSelectedIndex));
                }
                _lastSelectedIndex = -1;
                return;
            }

            var elementIdx = FindIntersectingTextElement();
            
            if(_lastSelectedIndex == elementIdx) return;
            
            if (_lastSelectedIndex != -1)
            {
                onPointerExit?.Invoke(GetTextElementInfo(_lastSelectedIndex));
            }
            
            if (elementIdx != -1)
            {
                onPointerEnter?.Invoke(GetTextElementInfo(elementIdx));
            }

            _lastSelectedIndex = elementIdx;
        }
        
        protected abstract int FindIntersectingTextElement();

        protected abstract T GetTextElementInfo(int index);
        
        private void ONPointerClick(PointerEventData eventData)
        {
            var elementIdx = FindIntersectingTextElement();
            
            if (elementIdx <= -1) return;
            
            onPointerClicked?.Invoke(GetTextElementInfo(elementIdx));
        }

        #endregion
    }
}