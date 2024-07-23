using UnityEngine;
using Experimental.SpellSystem;
using System;
using System.Reflection;

namespace Experimental.Casting
{
    public class SpellBookTest : MonoBehaviour
    {
        [SerializeField] private SpellTest[] spells;

        public SpellTest GetSpell(string _title)
        {
            SpellTest _spell = Array.Find(spells, x => x.Title == _title);

            return _spell;
        }

        public SpellTest GetRandomSpell()
        {
            float randomSpellIndex = UnityEngine.Random.Range(0f, spells.Length);
            SpellTest _spell = spells[(int)randomSpellIndex];
            return _spell;
        }
    }
}

