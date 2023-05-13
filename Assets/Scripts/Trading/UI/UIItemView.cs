using System;
using TMPro;
using Trading.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Trading.UI
{
    public class UIItemView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Factory
        
        static ObjectPool<UIItemView> pool;
        
        public static UIItemView Create(ItemController item, Transform parent, Action onBeginDrag, Action onEndDrag)
        {
            if (pool == null)
            {
                UIItemView CreateItem()
                {
                    UIItemView itemView = Instantiate(Resources.Load<UIItemView>("UI/UIItemView"));
                    itemView.gameObject.SetActive(false);
                    return itemView;
                }
                
                void OnItemGet(UIItemView itemView)
                {
                    itemView.gameObject.SetActive(true);
                }
                
                void OnItemRelease(UIItemView itemView)
                {
                    itemView.transform.SetParent(null, false);
                    itemView.gameObject.SetActive(false);
                }


                pool = new ObjectPool<UIItemView>(CreateItem, OnItemGet, OnItemRelease);
            }

            UIItemView itemView = pool.Get();
            itemView.transform.SetParent(parent, false);
            itemView.Initialize(item, onBeginDrag, onEndDrag);
            return itemView;
        }

        public static void Release(UIItemView itemView)
        {
            pool.Release(itemView);
        }
        
        #endregion

        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] TextMeshProUGUI itemCost;
        [SerializeField] CanvasGroup canvasGroup;

        RectTransform rectTransform;
        RectTransform RectTransform => rectTransform ? rectTransform : rectTransform = (RectTransform)transform;

        Canvas canvas;
        Canvas Canvas => canvas ? canvas : canvas = GetComponentInParent<Canvas>();
        
        RectTransform canvasTransform;
        RectTransform CanvasTransform => canvasTransform ? canvasTransform : canvasTransform = Canvas.GetComponent<RectTransform>();

        public ItemController ItemController { get; private set; }

        UIItemView dragObject;

        Action OnBeginDrag { get; set; }
        Action OnEndDrag { get; set; }

        public void Initialize(ItemController item, Action onBeginDrag, Action onEndDrag)
        {
            ItemController = item;
            itemName.text = ItemController.Name;
            itemCost.text = ItemController.Cost.ToString();

            OnBeginDrag = onBeginDrag;
            OnEndDrag = onEndDrag;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            dragObject = Create(ItemController, CanvasTransform, null, null);
            dragObject.canvasGroup.blocksRaycasts = false;
            
            dragObject.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, RectTransform.rect.width);
            dragObject.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, RectTransform.rect.height);

            dragObject.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            dragObject.RectTransform.anchorMin = new Vector2(0.5f, 0.5f);

            canvasGroup.alpha = 0.3f;

            OnBeginDrag?.Invoke();
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            dragObject.canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            Release(dragObject);
            
            OnEndDrag?.Invoke();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTransform, eventData.position, null,
                out Vector2 pos);
            dragObject.RectTransform.localPosition = pos;
        }
    }
}