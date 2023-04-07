using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO: ScriptableObject
    {
        #region public
        [field: SerializeField]
        public int size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
        #endregion

        #region private
        [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>(); // Tạo danh sách item hiện đang có trong Inventory
        #endregion

        // Ham khoi tao cac Item trong Inventory khi dung inventorySO
        public void Initialize()
        {
            for (int i = 0; i < size; i++)
            {
                if (inventoryItems.Count == size) return;

                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        // Ham set Data Item theo vi tri Item trong danh sach inventory
        public int AddItem(ItemSO item, int quantity)
        {
            if (item.isStackable == false) // Kiểm tra xem item có thể Stack số lượng được hay ko. Ở đây là nếu ko thể stack
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && IsInventoryFull() == false) // Nếu mà số lượng item nhặt được lớn hơn 0 và inventory chưa full
                    {
                        // Dòng code này là add Item ko thể Stack vào inventory
                        quantity -= AddItemToFirstFreeSlot(item, 1); // số lượng item nhặt sẽ trừ đi số lượng item được add vào inventory
                    }
                    InformAboutChange(); // Thông báo có sự thay đổi trong Inventory

                    return quantity; // Trả về số lượng item còn lại sau khi đã được nhặt
                }
            }
            quantity = AddStackableItem(item, quantity);

            InformAboutChange();

            return quantity; // Trả về số lượng item còn lại sau khi đã được nhặt
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity) // Hàm Add Item được nhặt vào Slot free trong inventory
        {
            InventoryItem newItem = new InventoryItem // Khởi tạo item mới bằng InventorySO
            {
                item = item,
                quantity = quantity,
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull() // Hàm kiểm tra xem Inventory đã full hay chưa
            => inventoryItems.Where(item => item.isEmpty).Any() == false;
            // Code kiểm tra slot item còn trống hay ko. Nếu ko trống thì code trả về false => Inventory chưa full => hàm trả về true
        
        private int AddStackableItem(ItemSO item, int quantity) // Hàm add Item có thể stack số lượng vào inventory
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty) continue; // Kiểm tra xem slot còn trống ko. nếu trống thì tiếp tục

                if (inventoryItems[i].item.ID == item.ID) // Check xem item trong inventory có trùng ID với ID của item được nhặt
                {
                    int amountPossibleToTake = inventoryItems[i].item.maxStackableSize - inventoryItems[i].quantity; // Tính toán số lượng còn có thể nhặt được Stackable Item

                    if (quantity > amountPossibleToTake) // Nếu số lượng item dưới đất > số lượng có thể nhặt
                    {
                        // Tổng số lượng nhặt vào + số lượng có trong inventory = số lượng Max có thể nhặt => Quantity của Item trong inventory lúc này sẽ là max
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.maxStackableSize);
                        quantity -= amountPossibleToTake; // số lượng còn lại trên mặt đất = số trên mặt đất - số có thể nhặt
                    }
                    else // Nếu số lượng item dưới đất >= số lượng có thể nhặt 
                    {
                        // Số lượng nhặt vào + số lượng có sẵn = số lượng mới sau khi nhặt
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                        InformAboutChange(); // thông báo có sự thay đổi
                        return 0; // trả về 0 => ko còn Item dư trên mặt đất
                    }
                }
            }

            while (quantity > 0 && IsInventoryFull() == false) // Code được sử dụng để xử lý phần dư của số lượng item còn trên mặt đất
            {
                int newQuantity = Math.Clamp(quantity, 0, item.maxStackableSize); // Số lượng mới là x; 0<x<Max; nếu x<0 thì x = min; nếu x>Max thì x = max;
                quantity -= newQuantity; // Số còn lại sẽ = số dư trên mặt đất - số được nhặt vào (newQuantity)
                AddItemToFirstFreeSlot(item, newQuantity); // Add item dư vào slot mới
            }

            return quantity;
        }

        // Duyet, kiem tra, tra ve toan bo trang thai hien tai cua Inventory
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty) continue;

                returnValue[i] = inventoryItems[i];
            }

            return returnValue;
        }

        // Ham tra ve item theo vi tri cua Item do trong list inventoryItems
        public InventoryItem GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        // Ham Swap 2 item
        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            if (itemIndex_1 == -1) return;

            InventoryItem item1 = inventoryItems[itemIndex_1];
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange(); // Thong bao cho InventoryCtrl biet co su thay doi trong Inventory
        }

        // Ham Thong bao cho InventoryCtrl biet de thay doi Update inventory
        public void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        // Hàm xóa Item
        public void RemoveItem(int itemIndex, int amount)
        {
            if (itemIndex < inventoryItems.Count) // Chỉ thực hiện được khi Item nằm trong danh sách Item trong Inventory
            {
                if (inventoryItems[itemIndex].isEmpty) return; // Nếu Item rỗng thì ra khỏi method

                int remainder = inventoryItems[itemIndex].quantity - amount; // Số dư còn lại của Item trong Inventory sau khi được sử dụng
                if (remainder <= 0)
                {
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem(); // Nếu số dư < 0 thì gọi hàm GetEmptyItem để trả về item rỗng
                }
                else
                {
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(remainder); // Nếu số dư > 0 thì số dư mới sẽ là remainder
                }

                InformAboutChange();
            }
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        #region public
        public int quantity;
        public ItemSO item;
        public bool isEmpty => item == null;
        #endregion

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
            };
        }

        public static InventoryItem GetEmptyItem()
            => new InventoryItem
            {
                item = null,
                quantity = 0,
            };
    }
}

