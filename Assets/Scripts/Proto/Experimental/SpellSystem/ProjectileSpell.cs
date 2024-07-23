using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental.SpellSystem
{
    [CreateAssetMenu(fileName = "NewProjectileSpell", menuName = "Spell/ProjectileSpell")]
    class ProjectileSpell : DamagerSpell
    {
        #region Editor Value Input
        [Header("Projectile")]
        [SerializeField] private float projectileSpeed;
        #endregion

        #region Public Getters
        public float ProjectileSpeed { get { return projectileSpeed; } }
        #endregion
    }
}


