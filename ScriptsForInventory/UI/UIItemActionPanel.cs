using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIItemActionPanel : MonoBehaviour
    {
        #region public
        #endregion

        #region private
        [SerializeField] private GameObject buttonPrefab;
        #endregion

        public void AddButton(string name, Action onClickAction)
        {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.GetComponent<Button>().onClick.AddListener(() => onClickAction());
            button.GetComponentInChildren<Text>().text = name;
        }

        public void Toggle(bool val)
        {
            if (val == true)
            {
                RemoveOldButton();
            }
            gameObject.SetActive(val);
        }

        public void RemoveOldButton()
        {
            foreach (Transform transformChildObjects in transform)
            {
                Destroy(transformChildObjects.gameObject);
            }
        }
    }
}