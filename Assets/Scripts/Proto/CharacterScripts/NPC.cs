using UnityEngine;

//this is how inScope updated vitality in his tutorials
//public delegate void VitalityChanged(float vitality);

public delegate void CharacterRemoved();

public class NPC : Character
{

    public event CharacterRemoved characterRemoved;

    public virtual Transform Select()
    {
        return hitBox;
    }

    public virtual void DeSelect()
    {

        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);

        //this funtion is deselecting targets, this is place holder.
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }

}
