using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experimental.SpellSystem
{
    [CreateAssetMenu(fileName = "NewBeamSpell", menuName = "Spell/BeamSpell")]
    class BeamSpell : DamagerSpell
    {
        #region Editor Value Input
        [Header("Beam")]
        [SerializeField] private float beamLength;
        [SerializeField] private float beamWidth;
        #endregion

        #region Public Getter
        public float BeamLength { get { return beamLength; } }
        public float BeamWidth { get { return beamWidth; } }
        #endregion
    }
}

