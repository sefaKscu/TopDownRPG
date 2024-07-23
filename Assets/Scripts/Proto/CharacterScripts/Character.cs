// Character Script v0.21 designed by Sefa Kuscu

/***Version History
 * 
 * v0.21 
 * - Stat Altering Methods improved & re-arranged
 */

using UnityEngine;
using StatSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CapsuleCollider2D))]
public abstract class Character : MonoBehaviour, IDamageble
{
    #region movement related fields


    //direction modifier of the character
    private Vector2 direction;

    [Header("Movement")]    
    // characters movement speed    
    [SerializeField, Range(0f, 60f)] private float speed;
    [SerializeField, Range(1f, 2f)] private float runMultiplier;

    protected bool isRunning = false;
    protected bool IsMoving
    {
        get
        {
            //comparing the direction vector to vector zero
            return Direction != Vector2.zero;
        }
    }

    //animation speed
    private float animationSpeed = 1f;


    public float Speed
    {
        get
        {
            if (isRunning)
            {
                return speed * runMultiplier;
            }
            else
            {
                return speed;
            }

        }
        set
        {
            speed = value;
        }
    }
    public float AnimationSpeed
    {
        get
        {
            if (isRunning)
            {
                return animationSpeed * runMultiplier;
            }
            else
            {
                return animationSpeed;
            }
        }
    }

    #endregion

    #region References

    // reference to the character's animator
    public Animator MyAnimator { get; set; }

    //reference to the character's rigidbody
    protected Rigidbody2D myRigidbody;

    protected MovementHandler myMovementHandler;


    //a reference to cast routine
    protected Coroutine castRoutine;

    [Space]
    // A reference to the transform of character's hitBox
    [SerializeField] protected Transform hitBox;

    
    // a reference to the transform of the target.
    public Transform Target { get; set; }

    //a reference to the target's distance to the character
    public float Distance { get; set; }



    /// <summary>
    /// direction of the movement.
    /// </summary>
    public Vector2 Direction
    { get => direction; set => direction = value; }


    #endregion

    #region Conditions

    protected bool isCasting = false;
    private bool isAttacking = false;
    private bool ýsAlive = true;
  


    public bool IsAlive
    { get => ýsAlive; set => ýsAlive = value; }

    public bool IsAttacking
    { get => isAttacking; set => isAttacking = value; }

    public bool InCastRange
    {
        get { return castRange > Distance; }
    }

    public bool InAttackRange
    {
        get { return attackRange > Distance; }
    }

    #endregion

    #region Stats

    protected Stat vitality;
    [Header("Stats")][SerializeField] 
    protected float maxVitality;
    protected float initVitality;

    public Stat Vitality
    {
        get
        {
            if (vitality == null)
            {
                vitality = new Stat();
            }
            return vitality;
        }
    }

    //old regen code
    //                 private float vitalityRegeneration;
    //[SerializeField, Range(0, 100)] 
    //                 private float vitalityRegenRate;

    protected Stat mojo;
    [SerializeField] 
    protected float maxMojo;
    protected float initMojo;

    public Stat Mojo
    {
        get
        {
            if (mojo == null)
            {
                mojo = new Stat();
            }
            return mojo;
        }
    }


    public bool HasMojo
    { get => (Mojo.CurrentValue != 0); }

    //old regen code
    //                 private float mojoRegeneration;
    //[SerializeField, Range(0, 100)]
    //                 private float mojoRegenRate;

    #endregion

    #region Attributes

    /* plans
     * there is going to be 
     * an attributes class
     * str, int, dex attributes
     * these attributes are going to change modifiers like base vitality, mojo
    */

    [Header("Base Values")]
    [SerializeField] private float unWieldedBaseDamage;

    public float UnWieldedBaseDamage
    { get => unWieldedBaseDamage; set => unWieldedBaseDamage = value; }

    #endregion

    #region Range
    [SerializeField] private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        set
        {
            //in future i can add range multiplier to swords, startingItems or passive tree skills
            attackRange = value;
        }
    }
    [SerializeField] private float castRange;
    public float CastRange
    {
        get { return castRange; }
        set 
        {
            //in future i can add range multiplier to swords, startingItems or passive tree skills
            castRange = value; 
        }
    }
    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ApplyDefaultSettings();
        InitializeStats();        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckDistance();
        AnimateMovement(Direction);
    }

    private void CheckDistance()
    {
        if (Target != null)
        {
            Distance = Vector2.Distance(transform.position, Target.transform.position);
        }
    }

    private void FixedUpdate()
    {
        Move(Direction);
    }

    /// <summary>
    /// This function appyl some default settings to the gameObject when it's called.
    /// </summary>
    public void ApplyDefaultSettings()
    {
        GetAndSetRigidbody2D();
        GetMovementHandler();
        GetAnimator();
    }

    #region Get Methods
    private void GetAnimator()
    {
        MyAnimator = GetComponent<Animator>();
    }

    private void GetMovementHandler()
    {
        myMovementHandler = GetComponent<MovementHandler>();
    }

    private void GetAndSetRigidbody2D()
    {
        myRigidbody = GetComponent<Rigidbody2D>(); //gets rb2d on the game object
        myRigidbody.gravityScale = 0;  //to prevent character from falling off the screen
        myRigidbody.freezeRotation = true;  //to prevent character from falling to the sides
    }
    #endregion

    #endregion

    #region Movement Methods
    /// <summary>
    /// handles movement.
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction)
    {
        if(IsAlive)
        {
            myRigidbody.velocity = direction.normalized * Speed * Time.fixedDeltaTime;
            //myMovementHandler.MoveToDirection(myRigidbody, direction, Speed);
        }
    }
    #endregion

    #region Animation Methods

    /// <summary>
    /// handles animation.
    /// </summary>
    /// <param name="direction"></param>
    public void AnimateMovement(Vector2 direction)
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                MyAnimator.SetFloat("x", direction.x);
                MyAnimator.SetFloat("y", direction.y);
                MyAnimator.SetFloat("run", AnimationSpeed);
                ActivateLayer("Move");
            }
            else if (isCasting)
            {
                ActivateLayer("Cast");
            }
            else if (isAttacking)
            {
                ActivateLayer("Attack");
            }
            else
            {
                ActivateLayer("Idle");
            }
        }
        else
        {
            ActivateLayer("Die");
            MyAnimator.SetBool("Dead", true);
            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1; //this sorts the sprite layer to be on the ground
            //gameObject.GetComponent<CapsuleCollider2D>().enabled = false; //this disables to capsule collider
        }
    }

    /// <summary>
    /// activates animation layers of the characters
    /// </summary>
    /// <param name="layerName"></param>
    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i< MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    #endregion

    #region Stat Related Methods

    /// <summary>
    /// initializes stats like vitality and mojo
    /// </summary>
    public void InitializeStats()
    {
        //check if vitality is available then initializes it
        if (Vitality != null)
        {
            initVitality = maxVitality;
            vitality.InitializeStat(maxVitality, initVitality);
            //Debug.Log(gameObject.name + "Vitality initialized");
        }
        //check if mojo is available then initializes it
        if (Mojo != null)
        {
            initMojo = maxMojo;
            mojo.InitializeStat(maxMojo, initMojo);
            //Debug.Log(gameObject.name + "Mojo initialized.");
        }
    }

    /// <summary>
    /// this function regenerates mana
    /// this is first prototype.
    /// </summary>
    public void RegenerateStats()
    {
        if (IsAlive)
        {
            if (Vitality != null)
            {
                Vitality.Regenerate();
            }

            if (Mojo != null)
            {
                Mojo.Regenerate();
            }
        }
    }

    #region Vitality

    /// <summary>
    /// this function deals damage to it's parent.
    /// </summary>
    /// <param name="damage">Damage value</param>
    /// <param name="source">Source of the damage</param>
    public void TakeDamage(float _damage, Transform source)
    {
        //this is first prototype, there is going to be damage reductions here
        if (this is NPC)
        {
            if (Target == null)
            {
                Target = source;
            }
        }

        LoseStatValue(Vitality, _damage);

        Debug.Log(source.gameObject.name + " dealt " + _damage + " damage to " + this.name + ".");

        if(vitality.CurrentValue <= 0)
        {
            Die(source);
        }
    }


    /// <summary>
    /// This is for healing
    /// </summary>
    /// <param name="_healAmount"></param>
    public void Heal(float _healAmount)
    {
        // Heal Logic Here; maybe some conditions like "if recently healed..."
        GainStatValue(Vitality, _healAmount);
        //Debug.Log("healed " + _healAmount + " vitality, vitality is now" + (int)Vitality.CurrentValue);
    }

    #endregion

    #region Mojo

    /// <summary>
    /// this function spends mana
    /// </summary>
    /// <param name="_mojoCost"></param>
    public void SpendMojo(float _mojoCost)
    {
        // Heal Logic Here; maybe some conditions like "if recently healed..."
        LoseStatValue(Mojo, _mojoCost);
    }

    /// <summary>
    /// this function gains mana
    /// </summary>
    /// <param name="_gainAmount"></param>
    public void GainMojo(float _gainAmount)
    {
        // Gain Mojo Logic Here; maybe some conditions like "if recently gained mojo..."
        GainStatValue(Mojo, _gainAmount);
    }

    #endregion

    #region Generic Stat Value Change
    public void LoseStatValue(Stat _stat, float _value)
    {
        _stat.LoseValue(_value);
    }

    public void GainStatValue(Stat _stat, float _value)
    {
        _stat.GainValue(_value);
    }
    #endregion

    /// <summary>
    /// this method kills the character
    /// </summary>
    /// <param name="source"></param>
    public void Die(Transform source)
    {
        this.IsAlive = false;
        Debug.Log(source.name + " killed " + gameObject.name);
        if (this is Player)
        {
            UIManager.MyInstance.CloseAllMenus();
            UIManager.MyInstance.OpenCloseMenu(1);
        }
    }

    #endregion

}
