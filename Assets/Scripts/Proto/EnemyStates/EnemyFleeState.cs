using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeState : IState
{
    private Foe parent;

    public void Enter(Foe parent)
    {
        this.parent= parent;
        //Debug.Log(parent.name + " returning.");
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        //check if target is null, if not chase it
        if (parent.Target == null)
        {
            //check if character is close to start point, if isn't move towards the start point. if is start to idle
            if (Vector2.Distance(parent.transform.position, parent.MyStartPosition) > 0.1f)
            {
                parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized;
                //regens while fleeing
                parent.RegenerateStats();
            }
            else
            {
                parent.ChangeState(new EnemyIdleState());
            }
        }
        else
        {
            parent.ChangeState(new EnemyChaseState());
        }
    }
}
