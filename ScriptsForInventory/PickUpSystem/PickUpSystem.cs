using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PickUpSystem : MonoBehaviour
{
    #region public
    #endregion

    #region private
    [SerializeField] private InventorySO inventoryData;
    private Item itemTarget;
    private bool pickUpAllow;
    #endregion

    private void Update()
    {
        if (pickUpAllow == true && Input.GetKey(KeyCode.E))
        {
            PickUpItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        itemTarget = collision.GetComponent<Item>();
        if (itemTarget != null)
        {
            pickUpAllow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        itemTarget = collision.GetComponent<Item>();
        if (itemTarget != null)
        {
            pickUpAllow = false;
        }
    }

    private void PickUpItem()
    {
        // T?o bi?n remainder th? hi?n cho s? l??ng item d? trên m?t ??t sau khi nh?t và add Item vào Inventory
        int remainder = inventoryData.AddItem(itemTarget.item, itemTarget.quantity); 

        if (remainder == 0) // N?u s? item d? = 0 thì xóa item ?ó trên m?t ??t
        {
            itemTarget.DestroyItem();
        }
        else // N?u s? item d? >0 thì s? d? còn trên m?t ??t là remainder
        {
            itemTarget.quantity = remainder;
        }
    }
}
