// Inventory Script v0.3

/*
 * Version History
 * v0.3
 * - Inventory class is now inherited from ItemContainer class
 * - itemSlots array moved to the parent class
 * - Events are removed from this script and moved to the parent class
 */


using System.Collections.Generic;
using InventorySystem.ItemSystem;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : ItemContainer
    {
        #region References
        [SerializeField] private List<Item> startingItems = new List<Item>();

        #endregion

        #region Editor

        protected override void OnValidate()
        {
            GetSlots();
        }

        #endregion

        #region Monobehaviour Methods

        protected override void Start()
        {
            base.Start();
            SetStartingItems();

        }

        /// <summary>
        /// Gives items to the player when inventory is loaded
        /// </summary>
        private void SetStartingItems()
        {
            ClearItems();

            foreach (Item _item in startingItems)
            {
                AddItem(_item.GetCopy());
            }
            //int i = 0;
            //for (; i < startingItems.Count && i < itemSlots.Length; i++)
            //{
            //    itemSlots[i].Item = Instantiate(startingItems[i]);
            //    itemSlots[i].Amount = 1;
            //}
            //for (; i < itemSlots.Length; i++)
            //{
            //    itemSlots[i].Item = null;
            //    itemSlots[i].Amount = 0;
            //}
        }
        #endregion
    }
}