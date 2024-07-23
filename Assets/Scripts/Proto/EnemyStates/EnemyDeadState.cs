using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : IState
{
    private Foe parent;

    public void Enter(Foe parent)
    {
        this.parent = parent;
        parent.Target = null;
        //following line is unnecessary due to IsAlive is setted in the character script allready
        //parent.IsAlive = false;
        parent.Direction = Vector2.zero;
        //Debug.Log(parent.name + " is dead.");
        DropLoot();
    }

    public void Exit()
    {

    }

    void IState.Update()
    {

    }

    private void DropLoot()
    {
        //drop loot here
    }
}
