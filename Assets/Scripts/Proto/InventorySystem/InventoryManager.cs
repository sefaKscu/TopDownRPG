// InventoryManager v0.37

/*  Version History
 *  
 *   v0.37
 * - Fixed the bug: Item modifiers does not remove when deleting directly from equipment slot.
 * - Fixed the bug: when you drag the item amount wasn't transferred to the draggedItem
 *  
 *   v0.36
 * - Stash System fully implemented
 * - Fixed the bug: Items disappearing when you right click to equip and drag to unequip.
 * - Fixed the bug: Stash event subscriptions wasn't working properly. 
 *  
 *   v0.35
 * - Stash System added.
 * 
 */

using InventorySystem.ItemSystem;
using InventorySystem.UI;
using UnityEngine;


namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        #region References
        [Header("Dependency References")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private EquipmentPanel equipmentPanel;
        [SerializeField] private ItemStash stash;
        [SerializeField] private ItemTooltip itemTooltip;
        [SerializeField] private DraggableItem draggableItem;
        [SerializeField] private DropItemArea dropItemArea;
        [SerializeField] private QuestionDialogue questionDialogue;
        [SerializeField] private Player player;
        private ItemSlot draggedSlot;
        #endregion

        #region MonoBehaviour Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (itemTooltip == null)
            {
                itemTooltip = FindObjectOfType<ItemTooltip>();
            }
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            }
        }
#endif

        private void Awake()
        {
            SubscribeToEvents();
            ReferDraggableItemAndAmount();
        }

        private void ReferDraggableItemAndAmount()
        {
            if (draggableItem == null)
            {
                draggableItem = GameObject.FindGameObjectWithTag("Draggable Item").GetComponent<DraggableItem>();
            }
        }

        private void SubscribeToEvents()
        {
            Debug.Log("SubscribeToEvents");
            // *** Setup Events ***
            // Right Click
            inventory.OnRightClickEvent += InventoryRightClick;
            equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
            // Pointer Enter
            inventory.OnPointerEnterEvent += ShowTooltip;
            equipmentPanel.OnPointerEnterEvent += ShowTooltip;
            // Pointer Exit
            inventory.OnPointerExitEvent += HideTooltip;
            equipmentPanel.OnPointerExitEvent += HideTooltip;
            // Begin Drag
            inventory.OnBeginDragEvent += BeginDrag;
            equipmentPanel.OnBeginDragEvent += BeginDrag;
            // End Drag
            inventory.OnEndDragEvent += EndDrag;
            equipmentPanel.OnEndDragEvent += EndDrag;
            // Drag
            inventory.OnDragEvent += Drag;
            equipmentPanel.OnDragEvent += Drag;
            // Drop
            inventory.OnDropEvent += Drop;
            equipmentPanel.OnDropEvent += Drop;
            dropItemArea.OnDropEvent += DropToDestroyItem;
        }




        private void Start()
        {
            HideInventoryUIElements();
        }

        private void HideInventoryUIElements()
        {
            draggableItem.Hide(); // this isn't necessary as it hides itself on the awake in draggableItem script 
            itemTooltip.gameObject.SetActive(false);
        }

        #endregion

        #region Stash Open/Close

        public void SetSubscriptionsOnOpenItemStash()
        {
            Debug.Log("SetSubscriptionsOnOpenItemStash");
            // Right Click Functionality
            inventory.OnRightClickEvent -= InventoryRightClick;
            inventory.OnRightClickEvent += TransferItemsToStashFromInventory;
            stash.OnRightClickEvent += TransferItemsToInventoryFromStash;

            // Other Events
            stash.OnPointerEnterEvent += ShowTooltip;
            stash.OnPointerExitEvent += HideTooltip;
            stash.OnBeginDragEvent += BeginDrag;
            stash.OnEndDragEvent += EndDrag;
            stash.OnDragEvent += Drag;
            stash.OnDropEvent += Drop;
        }

        public void SetSubscriptionsOnCloseItemStash()
        {
            Debug.Log("SetSubscriptionsOnCloseItemStash");
            // Right Click Functionality
            inventory.OnRightClickEvent += InventoryRightClick;
            inventory.OnRightClickEvent -= TransferItemsToStashFromInventory;
            stash.OnRightClickEvent -= TransferItemsToInventoryFromStash;

            // Other Events
            stash.OnPointerEnterEvent -= ShowTooltip;
            stash.OnPointerExitEvent -= HideTooltip;
            stash.OnBeginDragEvent -= BeginDrag;
            stash.OnEndDragEvent -= EndDrag;
            stash.OnDragEvent -= Drag;
            stash.OnDropEvent -= Drop;
        }
        #endregion

        #region Methods Triggered by Events

        public void InventoryRightClick(ItemSlot _itemSlot)
        {            
            if (_itemSlot.Item is EquippableItem _equipabbleItem)
            {
                EquipFromInventory(_equipabbleItem);
            }
            else if (_itemSlot.Item is UsableItem _usable)
            {
                _usable.Use(player);
                if (_usable.isConsumable)
                {
                    _itemSlot.Amount--;
                    //_usable.DestroyItem(); //don't know why we do this
                }
            }
        }

        public void EquipmentPanelRightClick(ItemSlot _itemSlot)
        {
            if (_itemSlot.Item is EquippableItem _equippableItem) // Check if item is equippable
            {
                UnequipToInventory(_equippableItem); // UnequipUI equippable
            }
        }

        private void TransferItemsToInventoryFromStash(ItemSlot _itemSlot)
        {
            Item _item = _itemSlot.Item;
            if(_item != null && inventory.CanAddItem(_item))
            {
                inventory.AddItem(_item);
                //stash.RemoveItem(_item);
                _itemSlot.Amount--;
            }            
        }

        private void TransferItemsToStashFromInventory(ItemSlot _itemSlot)
        {
            Item _item = _itemSlot.Item;
            if (_item != null && stash.CanAddItem(_item))
            {
                stash.AddItem(_item);
                //inventory.RemoveItem(_item);
                _itemSlot.Amount--;
            }
        }

        #region Tooltip Management
        private void ShowTooltip(ItemSlot _itemSlot)
        {
            if (itemTooltip != null && _itemSlot.Item != null)
            {
                itemTooltip.ShowToolTip(_itemSlot.Item);
                itemTooltip.transform.position = Input.mousePosition;
            }
        }

        private void HideTooltip(ItemSlot _itemSlot)
        {
            if (itemTooltip != null && itemTooltip.gameObject.activeSelf)
            {
                itemTooltip.HideToolTip();
            }
        }
        #endregion

        #region Drag & Drop
        private void BeginDrag(ItemSlot _itemSlot)
        {
            if (_itemSlot.Item != null)
            {
                draggedSlot = _itemSlot;
                draggableItem.transform.position = Input.mousePosition;
                draggableItem.Show(_itemSlot.Item.Icon, _itemSlot.Amount);
                //draggableItemImage.sprite = _itemSlot.Item.Icon;
                //draggableItemImage.enabled = true;
            }
        }

        private void EndDrag(ItemSlot _itemSlot)
        {
            draggedSlot = null;
            draggableItem.Hide();
        }

        private void Drag(ItemSlot _itemSlot)
        {
            if (draggableItem.IsActive)
            {
                draggableItem.transform.position = Input.mousePosition;
            }
        }

        private void Drop(ItemSlot _droppedItemSlot)
        {
            if (draggedSlot == null)
            {
                return;
            }
            if (_droppedItemSlot.CanAddStacks(draggedSlot.Item))
            {
                AddToStack(_droppedItemSlot);
                Debug.Log("addStacks");
            }
            else if (_droppedItemSlot.CanRecieveItem(draggedSlot.Item) && draggedSlot.CanRecieveItem(_droppedItemSlot.Item))
            {
                SwapItems(_droppedItemSlot);
            }
        }

        private void DropToDestroyItem()
        {
            if (draggedSlot == null)
                return;

            ItemSlot holdedDraggedSlot = draggedSlot;
            questionDialogue.Show();
            questionDialogue.OnYesEvent += () => DestroyDialogueOnYes(holdedDraggedSlot);
        }

        private void DestroyDialogueOnYes(ItemSlot _tmpSlot)
        {
            if (_tmpSlot != null)
            {
                UnequipOnDestroyItem(_tmpSlot);

                _tmpSlot.Item.DestroyItem();
                _tmpSlot.Item = null;
            }
        }



        #endregion

        #endregion

        #region Item Management

        private void SwapItems(ItemSlot _droppedItemSlot)
        {
            EquippableItem dragItem = draggedSlot.Item as EquippableItem;
            EquippableItem dropItem = _droppedItemSlot.Item as EquippableItem;

            if (draggedSlot is EquipmentSlot)
            {
                Debug.Log("dragged slot is equipment slot");
                if (dragItem != null) dragItem.Unequip(player);
                if (dropItem != null)
                {
                    dropItem.Equip(player);
                    Debug.Log("swapItem");
                }
            }
            else if (_droppedItemSlot is EquipmentSlot)
            {
                Debug.Log("dropped slot is equipment slot");
                if (dragItem != null) dragItem.Equip(player);
                if (dropItem != null)
                {
                    dropItem.Unequip(player);
                    Debug.Log("swapItem");
                }
            }

            Item draggedItemTmp = draggedSlot.Item;
            int draggedItemAmountTmp = draggedSlot.Amount;

            draggedSlot.Item = _droppedItemSlot.Item;
            draggedSlot.Amount = _droppedItemSlot.Amount;

            _droppedItemSlot.Item = draggedItemTmp;
            _droppedItemSlot.Amount = draggedItemAmountTmp;
        }

        private void AddToStack(ItemSlot _droppedItemSlot)
        {
            int _canRecieveAmount = _droppedItemSlot.Item.maximumStack - _droppedItemSlot.Amount;
            int _amountToAdd = Mathf.Min(_canRecieveAmount, draggedSlot.Amount);
            _droppedItemSlot.Amount += _amountToAdd;
            draggedSlot.Amount -= _amountToAdd;
        }

        #endregion

        #region Equip & Unequip Methods

        public void EquipFromInventory(EquippableItem _item)
        {
            if (inventory.RemoveItem(_item))
            {
                EquippableItem _previousItem;
                if (equipmentPanel.AddItem(_item, out _previousItem))
                {
                    if (_previousItem != null)
                    {
                        if (inventory.AddItem(_previousItem))
                        {
                            _previousItem.Unequip(player);
                            _item.Equip(player);
                            Debug.Log(_item.name + " equipped");
                        }
                    }
                    else
                    {
                        _item.Equip(player);
                        Debug.Log(_item.name + " equipped");
                    }

                }
                else
                {
                    inventory.AddItem(_item);
                }
            }
        }


        public void UnequipToInventory(EquippableItem _item)
        {
            if (inventory.CanAddItem(_item) && equipmentPanel.RemoveItem(_item))
            {
                if (inventory.AddItem(_item))
                {
                    //Debug.Log(_item.name + " added to your inventory");
                    _item.Unequip(player);
                }
                else
                {
                    Debug.Log("Inventory is full");
                }
            }
        }

        private void UnequipOnDestroyItem(ItemSlot _tmpSlot)
        {
            // Check if the slot is equipment slot, if so unequip the item
            EquipmentSlot _tmpEquipmentSlot = _tmpSlot as EquipmentSlot;
            if (_tmpEquipmentSlot == null)
                return; // if the slot is not equipment slot return early

            EquippableItem _deletedEquipment = _tmpSlot.Item as EquippableItem;

            _deletedEquipment.Unequip(player);
        }

        #endregion
    }
}