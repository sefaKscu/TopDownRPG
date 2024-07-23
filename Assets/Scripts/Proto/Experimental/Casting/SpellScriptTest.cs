using Experimental.SpellSystem;
using UnityEngine;

namespace Experimental.Casting
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SpellScriptTest : MonoBehaviour
    {
        private enum SpellType
        {
            Projectile,
            AOE,
            Beam,
            Chain,
            Channeling
        }

        #region References
        private DamagerSpell damagerSpell;
        private Rigidbody2D rb2D;
        private Transform source;
        private Transform aim;
        #endregion

        #region Projectile Values
        private float projectileSpeed;
        private Vector2 projectileDirection;
        private float projectileAngle;
        private SpellType type;
        #endregion

        #region Spell Effect Values
        private float damage; // this is for further improvement, I'm gonna use it when i implement 'calculation of spell effect values' to spell caster character
        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            rb2D = gameObject.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            DamagerSpellTypeLogic();
        }

        #endregion

        #region Initial Methods

        public void Initialize(DamagerSpell _spell, Transform _source, Transform _aim, Vector2 _targetedDirection)
        {
            CheckInitialSpellType(_spell);
            this.source = _source;
            this.damagerSpell = _spell;
            this.aim = _aim;
        }

        private void CheckInitialSpellType(DamagerSpell _spell)
        {
            BeamSpell _beamSpell = _spell as BeamSpell;
            if (_beamSpell != null)
            {
                // do something with the beam spell
                type = SpellType.Beam;
            }

            AOESpell _aOESpell = _spell as AOESpell;
            if (_aOESpell != null)
            {
                // do something with the aoe spell
                type = SpellType.AOE;
            }

            ProjectileSpell _projectileSpell = _spell as ProjectileSpell;
            if (_projectileSpell != null)
            {
                // do something with the projectile spell
                type = SpellType.Projectile;
            }
        }

        #endregion

        #region Logic & Control

        private void DamagerSpellTypeLogic()
        {
            // Beam Logic
            if (type == SpellType.Beam)
                AimBeam();
            // Projectile Logic


            // AOE Logic


            // Chaining Logic


            // Channelling Logic
        }

        private void AimBeam()
        {
            // this section should have lerping in order to smoothen beam movement
            this.transform.position = aim.position;
            this.transform.rotation = aim.rotation;
        }

        #endregion

        #region Useless For Now

        /// <summary>
        /// This method is going to probably be changed
        /// </summary>
        /// <param name="_targetedDirection"></param>
        private void CalculateBeam(Vector2 _targetedDirection)
        {

            //calculates the spells direction
            projectileDirection = _targetedDirection - new Vector2(transform.position.x, transform.position.y);
            //calculates the rotation of path
            projectileAngle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        }

        private void CalculateProjectilePath(Vector2 _targetedDirection)
        {

        }

        #endregion

        #region Collision

        private void OnTriggerEnter2D(Collider2D _other)
        {
            var damageble = _other.gameObject.GetComponent<IDamageble>();
            if (damageble != null)
            {
                damageble.TakeDamage(damagerSpell.Damage, source);
            }
        }

        #endregion
    }
}
