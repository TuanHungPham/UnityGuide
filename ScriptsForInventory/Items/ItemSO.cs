using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public bool isStackable { get; set; }

        [field: SerializeField]
        public int maxStackableSize { get; set; }

        [field: SerializeField]
        public string itemName { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string itemDescription { get; set; }

        [field: SerializeField]
        public Sprite itemImage { get; set; }

        public int ID => GetInstanceID();

    }
}