using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.ItemSystem
{

    public enum UsableType
    {
        Potion,
        Tool,
        Talisman
    }

    [CreateAssetMenu(fileName = "NewUsableItem", menuName = "Inventory/Usable Item")]
    public class UsableItem : Item
    {
        public UsableType Type;
        public bool isConsumable = false;

        [Header("Item Effects")]
        public List<UsableItemEffect> Effects;
        public virtual void Use(Character _character)
        {
            foreach (UsableItemEffect effect in Effects)
            {
                effect.ExecuteEffect(this, _character);
            }
        }
    }
}