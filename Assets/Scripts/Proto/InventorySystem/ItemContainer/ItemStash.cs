// ItemStash v0.1 by Sefa Kuşçu

/* Version History
 * v0.1
 * - Inherited from ItemContainer
 * - It is a static stash for now.
 */

using UnityEngine;

namespace InventorySystem
{
    public class ItemStash : ItemContainer
    {
        #region Editor

        protected override void OnValidate()
        {
            GetSlots();
        }

        #endregion

        #region MonoBehaviour Methods

        protected override void Start()
        {
            base.Start();
        }
        #endregion
    }
}
