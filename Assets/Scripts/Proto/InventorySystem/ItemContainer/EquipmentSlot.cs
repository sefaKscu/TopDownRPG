using InventorySystem.ItemSystem;

namespace InventorySystem
{

    public class EquipmentSlot : ItemSlot
    {
        public EquipmentType EquipmentType;

        protected override void OnValidate()
        {
            base.OnValidate();
            gameObject.name = EquipmentType.ToString() + " Slot";
        }

        public override bool CanRecieveItem(Item _item)
        {
            if (_item == null)
                return true; // when swapping item with an empty item slot it needs to return true

            EquippableItem equippableItem = _item as EquippableItem;
            return (equippableItem != null && equippableItem.EquipmentType == EquipmentType);
        }
    }
}