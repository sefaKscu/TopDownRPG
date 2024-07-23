using System;
using UnityEngine;

// InScope style spell script

[Serializable]
public class Spell : IUsable
{
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    [SerializeField] private int damage;
    [SerializeField] private int mojoCost;
    [SerializeField] private float speed;
    [SerializeField] private float castTime;
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Color barColor;

    public string MyName
    {
        get { return name; }
    }
    public Sprite MyIcon
    {
        get { return icon; }
    }
    public int MyDamage
    {
        get { return damage; }
    }
    public int MyMojoCost
    {
        get { return mojoCost; }
    }
    public float MySpeed 
    { 
        get { return speed; } 
    }
    public float MyCastTime
    {
        get { return castTime; }
    }
    public float MyCooldown
    {
        get { return cooldown; }
    }
    public GameObject MySpellPrefab 
    { 
        get { return spellPrefab; } 
    }
    public Color MyBarColor
    {
        get { return barColor; }
    }

    public void Use()
    {
        Player.MyInstance.CastSpell(MyName);
    }
}