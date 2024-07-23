// SpellScript v0.35 by Sefa Kuscu

/* This is the spell script
 */

using System.Text;
using Unity.Mathematics;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class SpellScript : MonoBehaviour
{

    #region References
    //The spell's rigidbody
    private Rigidbody2D myRigidbody;

    //The spell's Source
    private Transform source;

    //The spell's current target
    private Transform target;

    #endregion

    #region Projectile Properties

    //The spell's movement speed
    private float projectileSpeed;
    private Vector2 projectileDirection;
    private float projectileAngle;

    #endregion

    #region Spell Effect Values

    //The spell's damage
    private int damage;

    #endregion

    #region MonoBehaviour Methods

    // Use this for initialization
    void Start()
    {
        //creates a reference to the spell's rigidbody
        myRigidbody= GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // this method needs to be constantly called
        MoveProjectile();
    }

    #endregion

    #region Spell Initiation & Pathfinding

    public void Initialize(Spell _spell, Transform _target, Transform _source)
    {
        this.target = _target;
        this.damage = _spell.MyDamage;
        this.source = _source;
        this.projectileSpeed = _spell.MySpeed;
        CalculatePath();        
    }

    private void CalculatePath()
    {
        //calculates the direction, path, velocity, path and rotation of the spell
        if (target != null)
        {
            //calculates the spells direction
            projectileDirection = target.position - transform.position;

            Debug.Log(projectileDirection);

            //calculates the rotation of path
            projectileAngle = Mathf.Atan2(projectileDirection.y, projectileDirection.x) * Mathf.Rad2Deg;
        }
    }
    #endregion

    #region Set Spell Movement

    private void MoveProjectile()
    {
        myRigidbody.velocity = projectileDirection.normalized * projectileSpeed;
        transform.rotation = Quaternion.AngleAxis(projectileAngle, Vector3.forward);
    }

    private void StopProjectile()
    {
        projectileSpeed = 0f;
        projectileAngle = 0f;
    }

    #endregion

    #region Spell Hit

    private void EnemyCheck(Collider2D _other)
    {
        var foe = _other.gameObject.GetComponent<Foe>();
        if (foe != null)
        {
            StopProjectile();

            foe.TakeDamage(damage, source);
            GetComponent<Animator>().SetTrigger("impact"); // trigger impact animation
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false; // this line ensures the hit only occures one time

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyCheck(collision);
    }

    #endregion

}
