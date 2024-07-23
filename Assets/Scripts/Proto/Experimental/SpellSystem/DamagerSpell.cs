using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental.SpellSystem
{
    public enum DamageType
    {
        Physical,
        Fire,
        Cold,
        Lightning,
        Chaos
    }

    public class DamagerSpell : SpellTest
    {
        #region Editor Value Input
        [Header("Spell Properties")]
        [SerializeField] private DamageType damageType;
        [SerializeField] private float mojoCost;
        [SerializeField] private float damage;
        [SerializeField] private float castTime;
        [SerializeField] private float cooldownTime;
        [SerializeField] private float range;
        #endregion

        #region Public Getters
        public DamageType DamageType { get { return damageType; } }
        public float MojoCost { get { return mojoCost; } }
        public float Damage { get { return damage; } }
        public float CastTime { get { return castTime; } }
        public float Cooldown { get { return cooldownTime; } }
        public float Range { get { return range; } }
        #endregion
    }
}

