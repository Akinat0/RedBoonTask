using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Trading.UI
{
    public class UIDropZone : MonoBehaviour, IDropHandler
    {
        public event Action<GameObject> OnDropGameObject;

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag == null)
                return;

            OnDropGameObject?.Invoke(eventData.pointerDrag);
        }
    }
}