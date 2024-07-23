using System;
using UnityEngine;

/* Experimental Spell class as part of the Experimental.SpellSystem
 * This system is a ScriptableObject based system that you can create tru "create asset menu" and assign values from the inspector
 * And you can directly reference spells of this systems from spell book on the character
 */

namespace Experimental.SpellSystem
{
    [Serializable]
    public class SpellTest : ScriptableObject
    {
        #region Editor Value Input
        [Header("Spell Definition")]
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject spellPrefab;
        #endregion

        #region Public Getters
        public string Title { get { return title; } }
        public string Description { get { return description; } }
        public Sprite Icon { get { return icon; } }
        public GameObject SpellPrefab { get { return spellPrefab; } }
        #endregion
    }
}


