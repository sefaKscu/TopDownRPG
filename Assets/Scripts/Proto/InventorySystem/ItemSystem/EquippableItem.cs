using UnityEngine;

namespace InventorySystem.ItemSystem
{
    public enum EquipmentType
    {
        Helmet,
        Chest,
        Gloves,
        Boots,
        Trousers,
        Weapon,
        Shield,
        Ring,
        Amulet,
        Belt
    }

    [CreateAssetMenu(fileName = "NewEquippableItem", menuName = "Inventory/Equippable Item")]
    public class EquippableItem : Item
    {
        #region Modifiers
        public EquipmentType EquipmentType;
        [Header("Vitality")]
        public int vitalityFlat;
        public int vitalityPercent;
        public float vitalityRegenFlat;
        public float vitalityRegenPercent;
        [Header("Mojo")]
        public int mojoFlat;
        public int mojoPercent;
        public float mojoRegenFlat;
        public float mojoRegenPercent;
        [Header("Damage")]
        public int damage;
        public int damagePercent;
        #endregion

        #region Instance

        public override Item GetCopy()
        {
            return Instantiate(this);
        }

        public override void DestroyItem()
        {
            Destroy(this);
        }

        #endregion

        #region Equip & Unequip
        public void Equip(Character c)
        {
            if (vitalityFlat != 0)
            {
                c.Vitality.ModValueFlatAdd += vitalityFlat;
            }
            if (vitalityPercent != 0)
            {
                c.Vitality.ModValuePercAdd += vitalityPercent;
            }


            if (mojoFlat != 0)
            {
                c.Mojo.ModValueFlatAdd += mojoFlat;
            }
            if (mojoPercent != 0)
            {
                c.Mojo.ModValuePercAdd += mojoPercent;
            }
        }

        public void Unequip(Character c)
        {
            if (vitalityFlat != 0)
            {
                c.Vitality.ModValueFlatAdd -= vitalityFlat;
            }
            if (vitalityPercent != 0)
            {
                c.Vitality.ModValuePercAdd -= vitalityPercent;
            }


            if (mojoFlat != 0)
            {
                c.Mojo.ModValueFlatAdd -= mojoFlat;
            }
            if (mojoPercent != 0)
            {
                c.Mojo.ModValuePercAdd -= mojoPercent;
            }
        }
        #endregion
    }
}