using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "Items/EdibleItemSO")]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField] private List<ModifierData> modifierData = new List<ModifierData>();
        public string ActionName => "Consume";

        public bool PerformAction(GameObject character)
        {
            foreach (ModifierData data in modifierData)
            {
                data.statsModifier.AffectCharacter(character, data.value);
            }
            return true;
        }
    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }

        bool PerformAction(GameObject character);
    }

    [Serializable]

    public class ModifierData
    {
        public CharacterStatsModifierSO statsModifier;
        public float value;
    }
}