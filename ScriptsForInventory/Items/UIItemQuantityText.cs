using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemQuantityText : MonoBehaviour
{
    #region public
    public float distanceX;
    public float distanceY;
    #endregion

    #region private
    private Item item;
    private Canvas canvas;
    [SerializeField] private Transform itemObj;
    [SerializeField] private Text quantityText;
    #endregion

    private void Awake()
    {
        itemObj = transform.parent.transform;
        quantityText = GetComponentInChildren<Text>();
        item = GetComponentInParent<Item>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        FollowItem();

        if (item.quantity == 1)
        {
            quantityText.text = "";
        }
        else
        {
            quantityText.text = item.quantity + "";
        }
    }

    private void FollowItem()
    {
        quantityText.transform.position = new Vector2(itemObj.position.x + distanceX, itemObj.position.y - distanceY);
    }
}
