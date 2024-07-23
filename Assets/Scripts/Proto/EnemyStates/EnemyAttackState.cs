using System.Collections;
using UnityEngine;

public class EnemyAttackState : IState
{
    private Foe parent;

    public void Enter(Foe parent)
    {
        this.parent = parent;
        //Debug.Log(parent.name + " is attacking to " + parent.Target.gameObject.name + ".");
    }

    public void Exit()
    {

    }

    public void Update()
    {

        if(parent.Target != null && parent.Target.GetComponent<Player>().IsAlive)
        {
            if (parent.MyAttackTime >= parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length)
            {
                parent.MyAttackTime = 0;
                parent.StartCoroutine(Attack(parent.UnWieldedBaseDamage, parent.transform));
            }
            //check range and attack
            if (!parent.InAttackRange)
            {
                parent.ChangeState(new EnemyChaseState());                
            }
        }
        else
        {
            parent.ChangeState(new EnemyIdleState());
        }
    }


    /// <summary>
    /// Attack routine
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public IEnumerator Attack(float damage, Transform source)
    {
        //this method is buggy develop it further**

        //a reference to the characters target.
        Character c = parent.Target.GetComponentInParent<Character>();   
        
        //check if parent and target is alive
        if(parent.IsAlive && c.IsAlive)
        {
            //starts attack
            parent.IsAttacking = true;

            //sends a signal to the trigger in the animator
            parent.MyAnimator.SetTrigger("attack");

            //yield for the animation length in seconds
            yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length / 2);

            if (parent.IsAlive && c.IsAlive)
            {
                //calls TakeDamage function in the targets character script
                c.TakeDamage(damage, parent.transform);

                ////debugs the action as "parent dealt this many damage to target"
                //Debug.Log(parent.name + " dealt " + damage + " damage to " + parent.Target.name + ".");
            }

            //ends attack
            parent.IsAttacking = false;
        }


    }

}
