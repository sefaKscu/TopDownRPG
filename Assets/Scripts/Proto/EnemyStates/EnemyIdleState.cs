using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IState
{
    private Foe parent;

    public void Enter(Foe parent)
    {
        this.parent = parent;
        //Debug.Log(parent.name + " is idling.");
    }

    public void Exit()
    {
        
    }

    public void Update()
    {   
        //change into chase state if the player is close
        if (parent.Target != null)
        {
            parent.ChangeState(new EnemyChaseState());
        }

        parent.RegenerateStats();
    }


}
