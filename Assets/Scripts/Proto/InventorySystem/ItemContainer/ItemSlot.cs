// ItemSlot v0.52 by Sefa Kuscu

/* Future Plans
 * - Need to find a way to check pointer is staying on top of the item slot
 */


/* Version History
 *   v0.52
 * - Fixed the bug: when you drag the item amount wasn't transferred to the draggedItem
 * 
 *   v0.51
 * - Right Click now can not drag and item.
 * - Set the itemSlot's sprite to transparent On the start of the item drag,
 *   and set to Opaque if the item in the slot isn't null when drag ended.
 * - Fixed the bug: where the item tooltip isn't closing or refreshing when the item in the slot is altered.
 * 
 *   v0.5
 * - PointerHandler implemented for showing the item tooltip
 */


using System;
using InventorySystem.ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem
{
    
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        #region References
        [SerializeField]
        private Image Image;
        [SerializeField]
        private Text amountText;

   

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

        #region Item
        #region SpriteColor
        private Color normalColor = Color.white;
        private Color disabledColor = new(1, 1, 1, 0);
        #endregion

        [SerializeField] private Item item;

        public Item Item
        {
            get { return item; }
            set
            {
                item = value;

                if (item == null)
                {
                    // disable color if there is no item
                    if (Amount != 0) { Amount = 0; }
                    Image.color = disabledColor;
                    Image.sprite = null;
                }

                else
                {
                    // get and set item icon as the slot sprite
                    Image.sprite = item.Icon;
                    Image.color = normalColor;
                }

                if(isPointerOver)
                {
                    UpdatePointerHover();
                }

            }
        }



        [SerializeField] private int amount;
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (amount <= 0) { this.Item = null; }
                AmountTextEnableLogic();
            }
        }

        public void AmountTextEnableLogic()
        {
            if (amountText != null)
            {
                amountText.enabled = item != null && item.maximumStack > 1 && amount > 1;

                if (amountText.enabled) { amountText.text = amount.ToString(); }
            }
        }

        #endregion

        #region Conditions

        private bool isPointerOver= false;

        public virtual bool CanRecieveItem(Item _item)
        {
            return true;
        }

        public bool CanAddStacks(Item _item, int _amount = 1)
        {
            if (_item == null)
                return true;
            return ((Item != null) && (Item.ID == _item.ID)) && (Amount + _amount <= _item.maximumStack);
        }
        #endregion

        #region Monobehaviour Methods
        /// <summary>
        /// this will only run in the editor
        /// </summary>
        protected virtual void OnValidate()
        {
            if (Image == null)
            {
                Image = GetComponent<Image>();
            }
            if (amountText == null)
            {
                amountText = GetComponentInChildren<Text>();
            }
        }

        private void Start()
        {

        }

        private void OnDisable()
        {
            if (isPointerOver)
            {
                OnPointerExit(null);
            }
        }

        #endregion

        #region Pointer Events
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
            {
                if (OnRightClickEvent != null)
                {
                    OnRightClickEvent(this);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnPointerEnterEvent != null)
            {
                OnPointerEnterEvent(this);
            }
            isPointerOver= true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnPointerExitEvent != null)
            {
                OnPointerExitEvent(this);
            }
            isPointerOver= false;
        }

        private void UpdatePointerHover()
        {
            OnPointerExit(null);
            OnPointerEnter(null);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (OnBeginDragEvent != null && eventData.button == PointerEventData.InputButton.Left)
            {
                OnBeginDragEvent(this);
                Image.color = disabledColor; // Make the item slot transparent when you start dragging the item
                EquipmentSlot _equipmentSlot = this as EquipmentSlot;
                if (_equipmentSlot == null)
                    amountText.enabled = false; // Disable amount text when you start dragging the item

                UIManager.MyInstance.OpenMenu(4); // Activates ItemDropArea
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (OnEndDragEvent != null)
            {
                OnEndDragEvent(this);

                if (Item != null)
                {
                    Image.color = normalColor; // If there is an item on the slot make the item slot opaque again when you end dragging the item
                    AmountTextEnableLogic(); // there is an item on the slot make the amount text visible
                }
                UIManager.MyInstance.CloseMenu(4); // Deactivates ItemDropArea
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragEvent != null)
            {
                OnDragEvent(this);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (OnDropEvent != null)
            {
                OnDropEvent(this);
            }
        }
        #endregion
    }
}