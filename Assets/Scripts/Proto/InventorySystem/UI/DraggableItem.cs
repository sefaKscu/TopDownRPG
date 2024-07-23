using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class DraggableItem : MonoBehaviour
    {
        #region References
        [SerializeField] private Image draggableImage;
        [SerializeField] private Text amountText;
        #endregion

        #region Conditions
        public bool IsActive;
        #endregion


        /// <summary>
        /// Sets the amountText value automatically and enables or disables text accordingly
        /// </summary>
        private int amount
        {
            set
            {
                if (amountText != null)
                {
                    amountText.enabled = IsActive && value > 1; // Enable if draggable item is active and amount is bigger then 1.
                    if (amountText.enabled)
                        amountText.text = value.ToString(); // If text is enabled change it's value.
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            GetNecessaryComponents();
        }
#endif

        private void Awake()
        {
            Hide();
        }

        private void GetNecessaryComponents()
        {
            if (draggableImage == null)
                draggableImage= GetComponentInChildren<Image>();

            if (amountText == null)
                amountText= GetComponentInChildren<Text>();
        }

        public void Show(Sprite _icon, int _amount = 1)
        {
            IsActive = true;
            amount = _amount;
            draggableImage.sprite = _icon;
            if (draggableImage.sprite != null)
                draggableImage.enabled = true;
        }

        public void Hide()
        {
            IsActive = false;
            amount = 0;
            draggableImage.enabled = false;
            draggableImage.sprite = null;
        }

    }
}
