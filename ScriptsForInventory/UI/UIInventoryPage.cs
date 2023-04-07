using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        #region public
        public List<UIInventoryItem> listOfUIItems;
        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
        public event Action<int, int> OnSwapItems;
        #endregion

        #region private
        [SerializeField] private UIInventoryItem uiItemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private UIInventoryDescription inventoryDescription;
        [SerializeField] private MouseFollower mouseFollower;
        [SerializeField] private UIItemActionPanel actionPanel;
        private int currentlyDraggedItemIndex = -1;
        #endregion

        private void Awake()
        {
            listOfUIItems = new List<UIInventoryItem>();
            contentPanel = FindInActiveObjectByName("Content").GetComponent<RectTransform>();
            inventoryDescription = FindInActiveObjectByName("InventoryDescription").GetComponent<UIInventoryDescription>();
            mouseFollower = FindInActiveObjectByName("MouseFollower").GetComponent<MouseFollower>();
            actionPanel = FindInActiveObjectByName("ItemActionPanel").GetComponent<UIItemActionPanel>();

            mouseFollower.Toggle(false);
            inventoryDescription.ResetDescription();

            Hide();
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

        // Ham tao danh sach slot trong Inventory
        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem = Instantiate(uiItemPrefab);
                uiItem.transform.localScale = Vector3.one;
                uiItem.transform.SetParent(contentPanel, false);
                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnRightMouseButtonClick += HandleShowItem;
            }
        }

        // Ham update data cho item trong inventory
        public void UpdateData(int itemIndex, Sprite itemSprite, int quantity)
        {
            if (itemIndex < listOfUIItems.Count)
            {
                listOfUIItems[itemIndex].SetData(itemSprite, quantity);
            }
        }

        // Ham chon Item de hien len Description cua Item duoc chon
        private void HandleItemSelection(UIInventoryItem obj)
        {
            int index = listOfUIItems.IndexOf(obj);
            if (index == -1) return;

            OnDescriptionRequested?.Invoke(index);
        }

        // Ham Swap vi tri cua 2 item voi nhau
        private void HandleSwap(UIInventoryItem obj)
        {
            int index = listOfUIItems.IndexOf(obj);
            if (index == -1)
            {
                return;
            }

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);

            HandleItemSelection(obj);
        }

        // Ham thuc hien keo Item duoc chon
        private void HandleBeginDrag(UIInventoryItem obj)
        {
            int index = listOfUIItems.IndexOf(obj); // Lay so thu tu cua item trong danh sach UIItems
            if (index == -1) return;
            currentlyDraggedItemIndex = index; // Lay so thu tu cua item duoc chon de keo

            HandleItemSelection(obj);
            OnStartDragging?.Invoke(index);
        }

        // Ham thuc hien tha Item duoc keo
        private void HandleEndDrag(UIInventoryItem obj)
        {
            ResetDraggedItem();
        }

        // Hàm xử lý sử dụng Item
        private void HandleShowItem(UIInventoryItem obj)
        {
            int index = listOfUIItems.IndexOf(obj);
            if (index == -1) return;

            OnItemActionRequested?.Invoke(index);
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }

        // Ham tao ra hinh anh item khi item duoc keo di
        public void CreateDraggedItem(Sprite itemSprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(itemSprite, quantity);
        }

        // Ham de reset lai Item dang duoc keo
        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        // Ham reset lai inventory
        public void ResetSelection()
        {
            inventoryDescription.ResetDescription();
            DeselectedAllItems();
        }

        // Ham huy chon tat ca cac item
        public void DeselectedAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);
        }

        // Hàm cập nhật thông tin của Item lên DescriptionPanel để hiện thông tin lên UI
        public void UpdateDescription(int itemIndex, Sprite itemImage, string itemName, string itemDescription)
        {
            inventoryDescription.SetDescription(itemImage, itemName, itemDescription);
            DeselectedAllItems();
            listOfUIItems[itemIndex].Select();
        }

        // Ham mo inventory va Show cac item trong iventory
        public void Show()
        {
            gameObject.SetActive(true);

            ResetSelection();
        }

        // Ham tat inventory
        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}