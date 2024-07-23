using UnityEditor;
using UnityEngine;

namespace InventorySystem.ItemSystem
{

    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
    public class Item : ScriptableObject, ICollectable
    {
        #region ID
        [SerializeField]
        private string id;
        public string ID { get { return id; } }
        #endregion

        #region Base Properties
        [Header("General Definition")]
        public string ItemName;
        public Sprite Icon;
        public GameObject itemPrefab;
        [Range(1, 999)] public int maximumStack = 1;
        [TextArea]
        public string description = "This is the default description of an equippable item. It will roar like a bear or it will hum like a bird";
        #endregion

        public bool Collect(Inventory _inventory)
        {
            // add to player inventory
            if (_inventory.CanAddItem(GetCopy()))
            {
                return _inventory.AddItem(this.GetCopy());
            }
            else return false;
        }

        public virtual Item GetCopy()
        {
            return this;
        }

        public virtual void DestroyItem()
        {

        }


        private void OnValidate()
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);
#endif
        }
    }
}