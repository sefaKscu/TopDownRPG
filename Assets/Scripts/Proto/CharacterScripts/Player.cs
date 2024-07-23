// Player Script v0.5 inherited from Character Script v0.25 designed by Sefa Kuscu

/***Future Plans
 * Find a way to remove player singleton
 * 
 * Seperate this script to other components;
 * -InputReader
 * -SpellCaster
 * -WorldInteractionHandler
 * -SpellBook
 * -StatHandler
 */

/*** Version History
 * 
 * v0,25
 * - Nested if statements are replace with newly implemented CastLogic method.
 * - World Interactions region improved by implementing Interfaces. (IPresenceSensitive, ICollectable, IInteractable)
 * - World Interactions are now limited to capsule collider.
 * - Efficiency improvements
 */


using System.Collections;
using UnityEngine;
using InventorySystem;


public class Player : Character
{
    #region Player Singleton
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    #endregion

    #region Fields and Properties

    [Header("Spell Exits")]
    [SerializeField]    private Block[] blocks;

    [SerializeField]    private Transform[] exitPoints;
    
                        private int sightDirectionIndex = 2; //inScope named this "exitIndex"

                        private SpellBook spellBook;
    [Space]
    [SerializeField]    private Inventory inventory;
                        public Inventory Inventory { get { return inventory; } }

    #endregion

    #region Debug States

    [Space]
    [SerializeField]    private bool debugMode= false; //for debug

                        private bool isImmortal= false;  //for debug

    public bool DebugMode { get { return DebugMode; } }

    private Coroutine defaultAttackRoutine;

    #endregion

    #region Monobehaviour Methods


    protected override void Start()
    {
        debugMode = false;
        spellBook = GetComponent<SpellBook>();
        sightDirectionIndex = 2;//makes sure that sight direction starts down
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Debug.Log(LayerMask.GetMask("Blocks"));
        GetInput();
        RegenerateStats();

        //Debug.Log(Vitality.CurrentValue.ToString()+ "/" + Vitality.MaxValue.ToString());

        if (IsMoving && isCasting)
        {
            StopCasting();
        }
        //this line refers to old code
        //RegenerateStats();
        base.Update();
    }

    #endregion

    #region Input

    /// <summary>
    /// This method gets the control input
    /// </summary>
    public void GetInput()
    {
        #region Debug Inputs Region
        //--------------------------
        //THIS IS FOR DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.F1)) { debugMode = !debugMode; }
        if (debugMode)
        {
            //debug, heal
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Heal(10f);
            }
            //debug, gather mojo
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                GainMojo(10f);
            }

            //debug, take damage
            if (Input.GetKeyDown(KeyCode.F3))
            {
                TakeDamage(35f, transform);
            }

            //debug, spend mana
            if (Input.GetKeyDown(KeyCode.F4))
            {
                SpendMojo(30f);
            }
            //debug, immortal
            if (Input.GetKeyDown(KeyCode.F2))
            {
                isImmortal = !isImmortal;
                IsAlive = true;
                myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            if (isImmortal)
            {
                Vitality.CurrentValue = Vitality.MaxValue;
            }
        }

        //getting mouse position and printing it to the console
        //mousePosition= Input.mousePosition;
        //Debug.Log(mousePosition);


        //THIS IS FOR DEBUGGING ONLY
        //--------------------------
        #endregion

        Direction = Vector2.zero;

        if (Input.GetKey(KeyCode.W))//moves up
        {
            Direction += Vector2.up;
            sightDirectionIndex = 0;
        }
        if (Input.GetKey(KeyCode.A))//moves left
        {
            Direction += Vector2.left;
            sightDirectionIndex = 1;
        }
        if (Input.GetKey(KeyCode.S))//moves down
        {
            Direction += Vector2.down;
            sightDirectionIndex = 2;
        }
        if (Input.GetKey(KeyCode.D))//moves right
        {
            Direction += Vector2.right;
            sightDirectionIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))//toggles run on
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//toggles run off
        {
            isRunning = false;
        }

        /* these inputs are dependent on keybind manager and keybind manager is dependent on UI manager and so on..
         * in order to prevent these there is going to be scriptable object called KeyBind and it will store keybind values
         * and keybind manager will only alter these keybinds and not actually store the values
         * with that way need of keybind manager on every scene will be deprecated and whole sstem is going to be more modular tests are going to be easier
         * untill the implementation of that system i'm goona use hardcoded values in the player script
        */
        //if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["Up"]))//moves up
        //{
        //    Direction += Vector2.up;
        //    sightDirectionIndex = 0;
        //}
        //if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["Left"]))//moves left
        //{
        //    Direction += Vector2.left;
        //    sightDirectionIndex = 1;
        //}
        //if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["Down"]))//moves down
        //{
        //    Direction += Vector2.down;
        //    sightDirectionIndex = 2;
        //}
        //if (Input.GetKey(KeyBindManager.MyInstance.KeyBinds["Right"]))//moves right
        //{
        //    Direction += Vector2.right;
        //    sightDirectionIndex = 3;
        //}
        //if (Input.GetKeyDown(KeyBindManager.MyInstance.KeyBinds["Run"]))//toggles run on
        //{
        //    isRunning = true;
        //}
        //if (Input.GetKeyUp(KeyBindManager.MyInstance.KeyBinds["Run"]))//toggles run off
        //{
        //    isRunning = false;
        //}

        if(UIManager.MyInstance != null)
        {
            UIManager.MyInstance.GetUIInput();
        }
    }

    #endregion

    #region Attack & Cast

    /// <summary>
    /// Cast a spell.
    /// </summary>
    /// <param name="spellIndex"></param>
    public void CastSpell(string spellIndex)
    {
        Block();

        Spell newSpell = spellBook.CastSpell(spellIndex); // refering the spell

        // check cast logic
        if (CastLogic(newSpell.MyMojoCost))
        {
            castRoutine = StartCoroutine(Cast(newSpell));
        }

        /* Depricated Code
        //if(IsAlive)
        //{
        //    if (Target != null && InCastRange && !isCasting && !IsMoving)
        //    {
        //        if (Target.GetComponentInParent<Character>().IsAlive)
        //        {
        //            if (newSpell.MyMojoCost <= Mojo.CurrentValue)
        //            {
        //                if(InLineOfSight())
        //                {
        //                    castRoutine = StartCoroutine(Cast(newSpell));
        //                }
        //                else
        //                { }
        //            }
        //            else if (InAttackRange)
        //            { 
        //                Debug.Log("Not enough mojo.");
        //                defaultAttackRoutine = StartCoroutine(DefaultAttack());
        //            }
        //            else
        //            {
                        
        //            }
        //        }
        //        else
        //        {  }
        //    }
        //    else
        //    { }
        //}
        */
    }

    private bool CastLogic(int _mojoCost)
    {
        if (!IsAlive)
        {
            return false;
        }
        if (Target == null)
        {
            // this is gonna go
            Debug.Log("No target aimed.");
            return false;
        }
        if (!InCastRange)
        {
            Debug.Log("Target is out of range.");
            return false;
        }
        if (isCasting)
        {
            Debug.Log("Allready casting.");
            return false;
        }
        if (IsMoving)
        {
            Debug.Log("Can not cast when moving.");
            return false;
        }
        if (_mojoCost > mojo.CurrentValue)
        {
            Debug.Log("Not enough mojo.");
            return false;
        }
        if (!InLineOfSight())
        {
            // this is gonna go
            Debug.Log("Target Obscured");
            return false;
        }
        if (!Target.GetComponentInParent<Character>().IsAlive)
        {
            Debug.Log(Target.parent.name + " is allready dead.");
            return false;
        }

            return true;
    }

    /// <summary>
    /// this is cast routine;
    /// Gathering spell values, spending mana, setting animation levels to cast and throwing the spell takes place in here.
    /// </summary>
    /// <param name="spellIndex"></param>
    /// <returns></returns>
    private IEnumerator Cast(Spell _newSpell)
    {
        isCasting = true; //indicates we are attacking

        Transform currentTarget = Target;

        Debug.Log("Casting" + _newSpell.MyName);

        SpendMojo(_newSpell.MyMojoCost);

        MyAnimator.SetBool("cast", isCasting); //trigger casting animation

        yield return new WaitForSeconds(MyAnimator.GetCurrentAnimatorStateInfo(2).length); //this is a hardcoded cast time, for debugging


        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(_newSpell.MySpellPrefab, exitPoints[sightDirectionIndex].position, Quaternion.identity).GetComponent<SpellScript>(); //instantiate spell sprite and sents to the target
            s.Initialize(_newSpell, currentTarget, transform);
        }

        else if (currentTarget == null)
        {
            Debug.Log("Casting interrupted!");
        }

        StopCasting();
    }

    //Not used for the moment
    /// <summary>
    /// Default Attack
    /// </summary>
    /// <returns></returns>
    public IEnumerator DefaultAttack()
    {
        Foe f = Target.GetComponentInParent<Foe>();

        if (this.IsAlive && f.IsAlive)
        {
            this.IsAttacking = true;
            MyAnimator.SetTrigger("attack");

            yield return new WaitForSeconds(MyAnimator.GetCurrentAnimatorStateInfo(2).length / 2);
        
        f.TakeDamage(UnWieldedBaseDamage, this.transform);
        Debug.Log(this.name + " dealt " + UnWieldedBaseDamage + " damage to " + this.Target + ".");
        this.IsAttacking = false;
        }
        
        StopCoroutine(defaultAttackRoutine);
    }

    /// <summary>
    /// Stops casting when this method is called
    /// </summary>
    public void StopCasting()
    {
        isCasting = false;
        MyAnimator.SetBool("cast", isCasting); // this should be a trigger instead of bool

        if (castRoutine != null)
        {
            StopCoroutine(castRoutine);
        }

    }

    /// <summary>
    /// this method checks if the target in line of sight by raycasting a line to the target from player
    /// </summary>
    /// <returns>bool</returns>
    private bool InLineOfSight ()
    {
        if (Target != null)
        {
            Vector3 targetDirection = (Target.transform.position - transform.position); //calculates direction

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.transform.position), 256); //using raycast and to define line of sight

            if (debugMode) { Debug.DrawRay(transform.position, targetDirection, Color.red); }//this code is for debugging, it is drawing a ray to show where raycast is passing
            if (hit.collider == null) { return true; }
            else { return false; }
            
        }
        //if raycast did hit block, then character can't cast spell
        else
        {
            return false;
        }
    }

    /// <summary>
    /// this method rearranges block oriantation arround the player.
    /// </summary>
    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }
        blocks[sightDirectionIndex].Activate();
    }

    #endregion

    #region Stat Modifying Methods
    public void Resurrect()
    {
        this.IsAlive = true;
        Vitality.CurrentValue = Vitality.MaxValue;
        Mojo.CurrentValue = Mojo.MaxValue;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder= 0;
    }
    #endregion

    #region World Interactions

    /// <summary>
    /// this is checking if there is a collision with a trigger
    /// maybe i can transfer this to a child object with another script called collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        ////checking if contact is on capsule collider
        bool touching = gameObject.GetComponent<CapsuleCollider2D>().IsTouching(other);
        //if it is an item
        if (touching)
        {
            // IPresenceSensitive NullCheck
            var presenceSensitive = other.gameObject.GetComponent<IPresenceSensitive>();
            presenceSensitive?.SeizePresence(gameObject.transform);

            // ICollectable NullCheck
            var collectable = other.gameObject.GetComponent<ICollectable>();
            collectable?.Collect(Inventory);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        ////checking if contact is on capsule collider
        bool touching = gameObject.GetComponent<CapsuleCollider2D>().IsTouching(other);
        //if it is an item
        if (touching)
        {
            var interactable = other.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                //this input check needs to be inside OnTriggerStay, else this code doesn't fire up
                Debug.Log("Press E to interact.");
                if (Input.GetKeyDown(KeyCode.E))
                    interactable.Interact();
            }
        }
    }


    #endregion
}
