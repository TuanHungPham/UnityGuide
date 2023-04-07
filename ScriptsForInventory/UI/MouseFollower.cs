using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    #region public
    #endregion

    #region private
    [SerializeField] private Canvas canvas;
    [SerializeField] private UIInventoryItem item;
    #endregion

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<UIInventoryItem>();
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);

        transform.position = canvas.transform.TransformPoint(position);
    }

    // Set hinh anh, so luong cua item duoc chon de keo/tha
    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }

    // Bat/Tat UI Item khi keo/tha Item
    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
