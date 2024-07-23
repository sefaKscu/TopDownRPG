using UnityEngine;


namespace InventorySystem.ItemSystem
{

    [CreateAssetMenu(fileName = "NewHealingEffect", menuName = "Inventory/Usable Effect/Healing Effect")]
    public class HealingEffect : UsableItemEffect
    {
        public int healAmount;

        public override void ExecuteEffect(UsableItem _source, Character _character)
        {
            _character.Heal(healAmount);
        }
    }
}
