using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        #region public
        #endregion

        #region private
        [SerializeField] private Image itemDescriptionImage;
        [SerializeField] private Text itemDescription;
        [SerializeField] private Text title;
        #endregion

        private void Awake()
        {
            itemDescriptionImage = FindInActiveObjectByName("ImageDescription").GetComponent<Image>();
            itemDescription = FindInActiveObjectByName("Description").GetComponent<Text>();
            title = FindInActiveObjectByName("Title").GetComponent<Text>();

            ResetDescription();
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

        // Ham Reset Description ve mac dinh
        public void ResetDescription()
        {
            itemDescriptionImage.gameObject.SetActive(false);
            itemDescription.text = "";
            title.text = "";
        }

        // Ham thiet lap Description cho Item duoc chon de hien thong tin
        public void SetDescription(Sprite sprite, string itemName, string description)
        {
            itemDescriptionImage.gameObject.SetActive(true);
            itemDescriptionImage.sprite = sprite;
            title.text = itemName;
            itemDescription.text = description;
        }
    }
}