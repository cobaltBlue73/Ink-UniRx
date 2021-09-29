using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utility.General.UI
{
    [RequireComponent(typeof(CanvasGroup),
        typeof(LayoutGroup),
        typeof(ToggleGroup))]
    public class PaginationView : MonoBehaviour
    {
        #region Internals

        private class ElementPool: ObjectPool<PaginationElementView>
        {
            private readonly PaginationView _owner;
            private readonly PaginationElementView _template;
            private readonly Transform _parent;
            
            public ElementPool(PaginationView owner)
            {
                _owner = owner;
                this.AddTo(owner);
                _template = owner.elementTemplate;
                _template.gameObject.SetActive(false);
                _parent = owner.layoutGroup.transform;
            }
            
            protected override PaginationElementView CreateInstance()
            {
                var instance = Instantiate(_template, _parent);
                instance.Toggle.isOn = false;
                instance.WhenSelected.Subscribe(_owner.onPageSelected.Invoke).AddTo(_owner);
                return instance;
            }
        }

        #endregion
        
        #region Inspector

        [SerializeField, Required, InlineEditor] 
        private PaginationElementView elementTemplate;
        [SerializeField, Required] private CanvasGroup canvasGroup;
        [SerializeField, Required] private LayoutGroup layoutGroup;
        [SerializeField, Required] private ToggleGroup toggleGroup;
        [SerializeField] private UnityEvent<int> onPageSelected = new UnityEvent<int>();

        #endregion

        #region Properties

        public IObservable<int> WhenPageSelected => 
            onPageSelected.AsObservable();

        public bool Interactable
        {
            get => canvasGroup.interactable;
            set => canvasGroup.interactable = value;
        }

        #endregion

        #region Variables
        
        private ElementPool _elementPool;
        private readonly List<PaginationElementView> _elements = new List<PaginationElementView>();

        #endregion

        #region Unity Callbacks

        private void Reset()
        {
            if (!elementTemplate)
                elementTemplate = GetComponentInChildren<PaginationElementView>();

            if (!canvasGroup)
                canvasGroup = GetComponent<CanvasGroup>();

            if (!layoutGroup)
                layoutGroup = GetComponent<LayoutGroup>();

            if (!toggleGroup) 
                toggleGroup = GetComponent<ToggleGroup>();
        }

        private void OnValidate()
        {
            if (toggleGroup && !toggleGroup.allowSwitchOff)
                toggleGroup.allowSwitchOff = true;
        }

        private void Awake()
        {
            _elementPool = new ElementPool(this);
        }

        #endregion

        #region Methods

        #region Public

        public void SetPageCount(int pageCount)
        {
            if (pageCount < 0) return;
            
            for (int i = 0; i < pageCount; i++)
            {
                GetElement(i).SetPageNo(i + 1);    
            }

            for (int i = pageCount; i < _elements.Count; i++)
            {
                ReturnElement(i);
            }
        }

        public void ResetPageCount() => SetPageCount(0);

        #endregion

        #region Private

        private PaginationElementView GetElement(int index)
        {
            if (index < _elements.Count)
                return _elements[index];

            var newElement = _elementPool.Rent();
            newElement.transform.SetAsLastSibling();
            _elements.Add(newElement);
            
            return newElement;
        }

        private void ReturnElement(int index)
        {
            _elementPool.Return(_elements[index]);
        }

        #endregion

        #endregion
    }
}