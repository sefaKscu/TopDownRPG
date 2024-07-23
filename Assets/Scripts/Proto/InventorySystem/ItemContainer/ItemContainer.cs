// ItemContainer Script v0.3

/*
 * Version History
 * v0.3
 * - It is now base class of inventory and stash
 * - itemSlots array moved to here
 * - ItemContainer methods are now a member of IItemContainer interface
 * - Events declaration and assignement moved to here
 */



using System;
using InventorySystem.ItemSystem;
using UnityEngine;


namespace InventorySystem
{
    public abstract class ItemContainer : MonoBehaviour, IItemContainer
    {

        [SerializeField] protected ItemSlot[] itemSlots;
        [SerializeField] protected Transform itemsParent; // This is the parents of the slots. We take its children and add to the itemSlots array.

        #region Event Declarations
        public event Action<ItemSlot> OnPointerEnterEvent;
        public event Action<ItemSlot> OnPointerExitEvent;
        public event Action<ItemSlot> OnRightClickEvent;
        public event Action<ItemSlot> OnBeginDragEvent;
        public event Action<ItemSlot> OnEndDragEvent;
        public event Action<ItemSlot> OnDragEvent;
        public event Action<ItemSlot> OnDropEvent;
        #endregion

        protected virtual void OnValidate()
        {

        }

        protected void GetSlots()
        {
            if (itemsParent != null)
            {
                itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
            }
        }

        protected virtual void Start()
        {
            InitializeEvents();
        }

        protected void InitializeEvents()
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].OnPointerEnterEvent += _slot => EventHelper(_slot, OnPointerEnterEvent);
                itemSlots[i].OnPointerExitEvent += _slot => EventHelper(_slot, OnPointerExitEvent);
                itemSlots[i].OnRightClickEvent += _slot => EventHelper(_slot, OnRightClickEvent);
                itemSlots[i].OnBeginDragEvent += _slot => EventHelper(_slot, OnBeginDragEvent);
                itemSlots[i].OnEndDragEvent += _slot => EventHelper(_slot, OnEndDragEvent);
                itemSlots[i].OnDragEvent += _slot => EventHelper(_slot, OnDragEvent);
                itemSlots[i].OnDropEvent += _slot => EventHelper(_slot, OnDropEvent);
            }
        }

        private void EventHelper(ItemSlot _itemSlot, Action<ItemSlot> _action)
        {
            if(_action != null)
            {
                _action(_itemSlot);
            }
        }


        public virtual bool CanAddItem(Item _item, int _amount = 1)
        {
            int freeSpaces = 0;
            foreach (ItemSlot _slot in itemSlots)
            {
                if (_slot.Item == null || _slot.Item.ID == _item.ID)
                    freeSpaces += _item.maximumStack - _slot.Amount;
            }
            return freeSpaces >= _amount;
        }

        public virtual bool AddItem(Item _item, int _amount = 1)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].CanAddStacks(_item))
                {
                    itemSlots[i].Amount += _amount;
                    return true;
                }
            }
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == null)
                {
                    itemSlots[i].Item = _item;
                    itemSlots[i].Amount++;
                    return true;
                }
            }
            return false;
        }

        public virtual bool RemoveItem(Item _item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == _item)
                {
                    itemSlots[i].Amount--;
                    if (itemSlots[i].Amount == 0)
                    {
                        itemSlots[i].Item = null;
                    }
                    return true;
                }
            }
            return false;
        }

        public virtual Item RemoveItem(string _itemID)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Item _item = itemSlots[i].Item;
                if (_item != null && _item.ID == _itemID)
                {
                    itemSlots[i].Amount--;
                    if (itemSlots[i].Amount == 0)
                    {
                        itemSlots[i].Item = null;
                    }
                    return _item;
                }
            }
            return null;
        }

        public virtual bool ContainsItem(Item _item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == _item)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual int ItemCount(string _itemID)
        {
            int itemCount = 0;
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Item _item = itemSlots[i].Item;
                if (_item != null && _item.ID == _itemID)
                {
                    itemCount++;
                }
            }
            return itemCount;
        }

        /// <summary>
        /// Clear all items in the refered item container.
        /// Use this when setting starting items.
        /// </summary>
        public void ClearItems()
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Item _item = itemSlots[i].Item;
                if (_item != null)
                {
                    RemoveItem(_item.ID);
                }
            }
        }
    }
}


