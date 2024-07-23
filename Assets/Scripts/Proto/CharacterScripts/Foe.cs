using UnityEngine;
using UnityEngine.UI;

public class Foe : NPC
{
    #region Fields & Properties

    [SerializeField] private float sightRange = 1.5f;

    public float SightRange
    {
        get { return sightRange; }
        private set { sightRange = value; }
    }

    //a reference to the enemy's attack time
    public float MyAttackTime { get; set; }

    //a reference to enemy character's start position
    public Vector3 MyStartPosition { get; set; }

    #endregion

    #region References

    [Header("Vitality Frame (Foe Only)")]
    [SerializeField] private CanvasGroup vitalityGroup;
    [SerializeField] private Image vitalityFillerOnNpc;
    [SerializeField] private Text vitalityTextOnNpc;

    //a reference to state interface
    private IState currentState;

    #endregion

    #region MonoBehaviour Methods

    protected void Awake()
    {
        //defining the idle state as current state at awake
        MyStartPosition= transform.position;
        ChangeState(new EnemyIdleState());
    }

    protected override void Update()
    {
        if(!IsAlive)
        {
            ChangeState(new EnemyDeadState());
            vitalityGroup.alpha = 0.0f;
        }
        else if (IsAlive)
        {
            UpdateVitalityCanvasOnEnemy();
            CountAttackTime();
        }

        currentState.Update(); //beware of calling state update before calculating distance may cause bugs!!! This seems to work for now..
        base.Update();
    }

    #endregion

    #region Enemy Frame

    public override Transform Select()
    {
        if (IsAlive)
        {
            vitalityGroup.alpha = 1.0f;
        }
        return base.Select();
    }

    public override void DeSelect()
    {
        vitalityGroup.alpha = 0.0f;
        base.DeSelect();
    }

    private void UpdateVitalityCanvasOnEnemy()
    {
        vitalityFillerOnNpc.fillAmount = Vitality.FillAmount;
        vitalityTextOnNpc.text = Vitality.TextValue;
    }

    #endregion

    #region Enemy States

    /// <summary>
    /// this method changes the state of the foe.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IState newState)
    {
        if(currentState is EnemyDeadState)
        {
            return;
        }
        else
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            //this is a shorter version of null check
            //currentState?.Exit();

            currentState = newState;
            currentState.Enter(this);
            if (currentState is EnemyDeadState)
            {
                hitBox.GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<CapsuleCollider2D>().enabled = false;
            }
        }

    }

    public void Reset()
    {
        this.Target = null;
        this.Direction= Vector3.zero;
        this.myRigidbody.velocity = Vector3.zero;
    }

    private void CountAttackTime()
    {
        if (!IsAttacking)
        {
            MyAttackTime += Time.deltaTime;
        }
    }

    #endregion

}
