using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        #region public
        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseButtonClick;
        #endregion

        #region private
        [SerializeField] private Image border;
        [SerializeField] private Image itemImage;
        [SerializeField] private Text itemQuantity;
        [SerializeField] private Image backgroundText;
        private bool empty = true;
        #endregion

        private void Awake()
        {
            border = FindInActiveObjectByName("Border").GetComponent<Image>();
            itemImage = FindInActiveObjectByName("ItemImage").GetComponent<Image>();
            itemQuantity = FindInActiveObjectByName("ItemQuantity").GetComponent<Text>();
            backgroundText = FindInActiveObjectByName("BackGroundText").GetComponent<Image>();

            ResetData();
            Deselect();
        }

        GameObject FindInActiveObjectByName(string name)
        {
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].hideFlags == HideFlags.None)
                {
                    if (objs[i].name == name)
                    {
                        return objs[i].gameObject;
                    }
                }
            }
            return null;
        }

        // Ham Reset Data cua slot
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        // Ham bo chon slot
        public void Deselect()
        {
            border.gameObject.SetActive(false);
        }

        // Ham set data cua item cho slot chua item do
        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            itemQuantity.text = quantity + "";
            empty = false;
            if (quantity == 1)
            {
                backgroundText.gameObject.SetActive(false);
            }
            else
            {
                backgroundText.gameObject.SetActive(true);
            }
        }

        // Ham chon item trong slot
        public void Select()
        {
            border.gameObject.SetActive(true);
        }

        // Ham Click chuot
        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseButtonClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        // Ham bat dau thuc hien keo item
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) return;

            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        // Ham tha item
        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}