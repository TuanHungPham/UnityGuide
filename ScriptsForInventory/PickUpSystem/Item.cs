using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region public
    public int quantity;
    [field: SerializeField] public ItemSO item;
    #endregion

    #region private
    [field: SerializeField] private float duration;
    #endregion

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
    }

    public void DestroyItem()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
    }
}
