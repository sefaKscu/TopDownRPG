/* ToolTip Script v0.2 By Sefa Kuþçu
 */


using System.Text;
using InventorySystem.ItemSystem;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        #region References
        [SerializeField] Text itemName;
        [SerializeField] Text itemType;
        [SerializeField] Text itemModifiers;
        [SerializeField] Text itemDescription;
        #endregion

        #region Utility
        private StringBuilder sb = new StringBuilder();
        #endregion

        #region Monobehaviour Methods
        public void Start()
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region Tooltip UI Actions
        public void ShowToolTip(Item _item)
        {
            itemName.text = _item.ItemName;
            sb.Length = 0;

            EquippableItem _equipment = _item as EquippableItem;
            if (_equipment != null)
            {
                EquippableItemTooltipCreator(_equipment);
            }
            else if (_item != null)
            {
                OtherItemTooltipCreator(_item);
            }

            itemModifiers.text = sb.ToString();

            //This line of code will change when the startingItems have a description field.?
            itemDescription.text = _item.description;
            gameObject.SetActive(true);
        }

        public void HideToolTip()
        {
            gameObject.SetActive(false);
        }


        private void OtherItemTooltipCreator(Item _item)
        {
            itemType.text = "Other Item";
            sb.Append("Place Holder Stats" + '\n' + "for Other Items");
            // string buildier commands here
        }

        private void EquippableItemTooltipCreator(EquippableItem _item)
        {
            itemType.text = _item.EquipmentType.ToString();

            AddModifier(_item.vitalityFlat, "Vitality");
            AddModifier(_item.vitalityPercent, "Vitality", true);

            AddModifier(_item.mojoFlat, "Mojo");
            AddModifier(_item.mojoPercent, "Mojo", true);


        }

        private void AddModifier(float _value, string _modifier, bool isPercent = false)
        {
            //formating the modifier texts here if value is different than zero
            if (_value != 0)
            {
                if (sb.Length > 0) { sb.AppendLine(); } // If there is a character it means it's not the first modifier. Then we add a line.
                if (_value > 0) { sb.Append("+"); } // If modifier is positive we add a plus sign. If value is negative it will allready have a minus sign.
                sb.Append(_value); // Printing the value
                if (isPercent) { sb.Append("%"); } // If it is a percent modifier add % sign
                sb.Append(" "); // Leaving space
                sb.Append(_modifier); // Printing modifier name
            }
        }
        #endregion
    }
}
