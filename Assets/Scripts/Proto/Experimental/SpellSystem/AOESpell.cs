using UnityEngine;

namespace Experimental.SpellSystem
{
    [CreateAssetMenu(fileName = "NewAOESpell", menuName = "Spell/AOESpell")]
    class AOESpell : DamagerSpell
    {
        #region Editor Value Input
        [Header("AOE")]
        [SerializeField] private float radius;
        #endregion

        #region Public Getters
        public float Radius { get { return radius; } }
        #endregion
    }
}
