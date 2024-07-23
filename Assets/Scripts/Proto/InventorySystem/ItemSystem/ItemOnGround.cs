using UnityEngine;

namespace InventorySystem.ItemSystem
{

    [RequireComponent(typeof(BoxCollider2D))]
    public class ItemOnGround : MonoBehaviour, ICollectable
    {
        [SerializeField] private Item item;
        BoxCollider2D itemCollider;
        private SpriteRenderer itemSprite;

        private void OnValidate()
        {
            if (itemSprite == null)
            {
                itemSprite = GetComponent<SpriteRenderer>();
            }
        }

        //private void OnValidate()
        //{
        //    itemCollider = gameObject.GetComponent<BoxCollider2D>();
        //    itemCollider.isTrigger= true;

        //}

        public bool Collect(Inventory _inventory)
        {
            bool itemCollected = false;

            if (item != null && _inventory != null)
            {
                if (item.Collect(_inventory))
                {
                    itemCollected = true;
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory is full");
                }

            }
            else if (_inventory == null)
            {
                Debug.Log("inventory is null");
            }

            return itemCollected;
        }
    }
}