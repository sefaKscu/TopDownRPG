﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace InventorySystem.UI
{
    public class DropItemArea : MonoBehaviour, IDropHandler
    {
        public event Action OnDropEvent;

        public void OnDrop(PointerEventData eventData)
        {
            if (OnDropEvent != null)
            {
                OnDropEvent();
            }
        }
    }
}
