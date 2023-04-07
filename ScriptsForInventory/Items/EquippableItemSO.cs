using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Items/EquippableItemSO")]
    public class EquippableItemSO : ItemSO , IDestroyableItem, IItemAction
    {
        [SerializeField] private List<ModifierData> modifiersData = new List<ModifierData>();
        public string ActionName => "Equip";

        public bool PerformAction(GameObject character)
        {
            foreach (ModifierData data in modifiersData)
            {
                data.statsModifier.AffectCharacter(character, data.value);
            }
            return true;
        }
    }
}