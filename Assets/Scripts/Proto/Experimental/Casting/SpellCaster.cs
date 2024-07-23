// Experimental SpellCaster v0,15


/* Future Plans
 * 
 */

/* Version History
 * v0,15
 * - Spell instantiation & initialization are improved
 * - Aiming system is now omited from SpellCaster and have it's own script
 * - Sight direction is now depricated and removed
 * - exitPoints[] is now depricated and removed
 * - MovementInput is now depricated and removed
 * - Camera Reference is now Serialized
 * - AimSpell method is now depricated
 * 
 * v0,11
 * - Temporary TakeDamage method implemented
 * - CoolDown has Drafted
 * - CastLogic has drafted
 */


using System.Collections;
using Experimental.SpellSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Experimental.Casting
{

    [RequireComponent(typeof(SpellBookTest))]
    public class SpellCaster : MonoBehaviour, IDamageble
    {
        #region References
        private SpellBookTest spellBook;

        [SerializeField] private Camera cam;
        [SerializeField] private Transform spellExit;

        Coroutine castRoutine;

        #endregion

        #region Conditions
        private bool isCasting= false;
        #endregion

        #region Cooldown Values
        private float cooldownCount;
        protected float CooldownCount
        {
            get { return cooldownCount; }
            private set
            {
                if (value > 0) cooldownCount = value;
                else cooldownCount = 0;
            }            
        }
        #endregion

        #region MonoBehaviour Methods
        private void OnValidate()
        {
            spellBook = gameObject.GetComponent<SpellBookTest>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            GetInput();
        }
        #endregion

        #region Input
        public void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CastLogic(spellBook.GetSpell("Beam Of Purity"));
            }
        }

        #endregion

        #region Casting
        public void CastLogic(SpellTest _spell)
        {
            // Checking if the spell is damagerSpell if it is cast it as damagerspell
            DamagerSpell damagerSpell = _spell as DamagerSpell;
            if (damagerSpell != null)
            {
                if (!DamagerSpellLogic())
                {
                    return;
                }
                castRoutine = StartCoroutine(CastDamagerSpell(damagerSpell));
            }
        }

        private IEnumerator CastDamagerSpell(DamagerSpell _spell)
        {
            isCasting = true;

            #region Get Mouse Input and Calculate Target Direction
            // This region should be extracted as another method
            Vector3 targetDirection = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position; // Calculate target direction
            //float projectileAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg; // Calculate direction in degrees (this is depricated)
            //Quaternion quatAngle = Quaternion.Euler(0, 0, projectileAngle); // Convert degrees to Quaternion (this is depricated)
            #endregion

            // Calculate damage
            // Calculate spell modifiers like proj speed, aoe, etc.
            // Calculate mana cost
            // Spend some amount of Mojo

            yield return new WaitForSecondsRealtime(_spell.CastTime);

            #region Instantiate & Initialize
            SpellScriptTest s = Instantiate(_spell.SpellPrefab, spellExit.position, spellExit.rotation).GetComponent<SpellScriptTest>();
            s.Initialize(_spell, this.transform, spellExit, targetDirection);
            #endregion


            StopCasting();
        }

        private void StopCasting()
        {
            isCasting = false;
            if(castRoutine != null)
            {
                StopCoroutine(castRoutine);
            }
        }

        /// <summary>
        /// Cooldown counter. It counts down seconds
        /// </summary>
        public void CountCooldown()
        {
            // not implemented
            if(CooldownCount != 0)
            {
                CooldownCount -= Time.deltaTime;
            }
        }

        public bool DamagerSpellLogic()
        {
            
            if(isCasting)
            {
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Temporary TakeDamage function
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="source"></param>
        public void TakeDamage(float damage, Transform source)
        {
            Debug.Log(source.gameObject.name + " dealt " + damage + " to " + gameObject.name);
        }

    }
}

