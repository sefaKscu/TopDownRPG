using System;
using UnityEngine;

namespace InventorySystem.UI
{
    public class QuestionDialogue : MonoBehaviour
    {
        //[SerializeField] private Text dialogueText;
        //[SerializeField, TextArea] private string questionString = "Are you sure?";
              

        //private void Start()
        //{
        //    if (dialogueText != null) 
        //        dialogueText.text = questionString;
        //}

        public event Action OnYesEvent;
        public event Action OnNoEvent;

        public void OnYesButtonClick() 
        { 
            if (OnYesEvent != null) OnYesEvent(); 
            Hide(); 
        }
        public void OnNoButtonClick() 
        { 
            if (OnNoEvent != null) OnNoEvent(); 
            Hide(); 
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            OnYesEvent = null;
            OnNoEvent = null;
            gameObject.SetActive(false);
        }
    }
}
