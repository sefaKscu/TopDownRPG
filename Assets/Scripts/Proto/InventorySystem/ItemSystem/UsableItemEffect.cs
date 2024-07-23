using UnityEngine;

namespace InventorySystem.ItemSystem
{
    public abstract class UsableItemEffect : ScriptableObject
    {
        public abstract void ExecuteEffect(UsableItem _source, Character _character);
    }
}
