using InventorySystem.ItemSystem;

namespace InventorySystem
{
    public interface IItemContainer
    {
        int ItemCount(string _itemID);
        Item RemoveItem(string _itemID);
        bool ContainsItem(Item _item);
        bool RemoveItem(Item _item);
        bool AddItem(Item _item, int _amount = 1);
        bool CanAddItem(Item _item, int _amount = 1);
    }
}
