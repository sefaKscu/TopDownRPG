using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyChaseState : IState
{
    private Foe parent;

    public void Enter(Foe parent)
    {
        this.parent = parent;
        //Debug.Log(parent.name + " is chasing the " + parent.Target.gameObject.name + ".");
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
            if (parent.Target != null && parent.Target.GetComponentInParent<Character>().IsAlive && (parent.Distance < 3f))
            {
                parent.Direction = parent.Target.position - parent.transform.position;

                if (parent.InAttackRange)
                {
                    parent.ChangeState(new EnemyAttackState());
                }
            }
            else
            {
                parent.Reset();
                parent.ChangeState(new EnemyFleeState());
            }
    }
}
