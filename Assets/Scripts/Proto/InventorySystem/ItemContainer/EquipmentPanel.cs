using System;
using InventorySystem.ItemSystem;
using UnityEngine;

namespace InventorySystem
{

    public class EquipmentPanel : MonoBehaviour
    {
        #region References
        [SerializeField] Transform equipmentSlotParent;
        [SerializeField] EquipmentSlot[] equipmentSlots;
        #endregion

        #region Events
        public event Action<ItemSlot> OnPointerEnterEvent;
        public event Action<ItemSlot> OnPointerExitEvent;
        public event Action<ItemSlot> OnRightClickEvent;
        public event Action<ItemSlot> OnBeginDragEvent;
        public event Action<ItemSlot> OnEndDragEvent;
        public event Action<ItemSlot> OnDragEvent;
        public event Action<ItemSlot> OnDropEvent;
        #endregion

        #region MonoBehaviour Methods
        private void OnValidate()
        {
            equipmentSlots = gameObject.GetComponentsInChildren<EquipmentSlot>();
        }

        private void Start()
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                equipmentSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
                equipmentSlots[i].OnPointerExitEvent += OnPointerExitEvent;
                equipmentSlots[i].OnRightClickEvent += OnRightClickEvent;
                equipmentSlots[i].OnBeginDragEvent += OnBeginDragEvent;
                equipmentSlots[i].OnEndDragEvent += OnEndDragEvent;
                equipmentSlots[i].OnDragEvent += OnDragEvent;
                equipmentSlots[i].OnDropEvent += OnDropEvent;
            }
        }
        #endregion

        #region Add & Remove Item
        public bool AddItem(EquippableItem _item, out EquippableItem _previousItem)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].EquipmentType == _item.EquipmentType)
                {
                    _previousItem = (EquippableItem)equipmentSlots[i].Item;
                    equipmentSlots[i].Item = _item;
                    equipmentSlots[i].Amount = 1;
                    return true;
                }
            }
            //else
            _previousItem = null;
            return false;
        }


        public bool RemoveItem(EquippableItem _item)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].Item == _item)
                {
                    equipmentSlots[i].Item = null;
                    equipmentSlots[i].Amount = 0;
                    return true;
                }
            }
            //else
            return false;
        }
        #endregion
    }
}