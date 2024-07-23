using System;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;
    public static SpellBook MyInstance 
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }
            return instance; 
        } 
    }



    [SerializeField] private Spell[] spells;

    public Spell CastSpell(string index)
    {
        Spell spell = Array.Find(spells, x => x.MyName == index);

        return spell;
    }

    public Spell GetSpell(string index)
    {
        Spell spell = Array.Find(spells, x => x.MyName == index);
        return spell;
    }
}
