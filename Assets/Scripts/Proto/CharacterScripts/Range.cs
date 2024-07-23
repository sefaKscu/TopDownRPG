using UnityEngine;

public class Range : MonoBehaviour
{
    private Foe parent;

    private void Start()
    {
        parent = GetComponentInParent<Foe>();
        GetComponent<CircleCollider2D>().radius = parent.SightRange;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (parent.IsAlive && collision.CompareTag("Player"))
        {
            //Debug.Log("the player in range");
            parent.Target = collision.transform;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parent.IsAlive && collision.CompareTag("Player"))
        {
            //Debug.Log("the player in range");
            parent.Target = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (parent.IsAlive && collision.CompareTag("Player"))
        {
            //Debug.Log("player out of range");
            parent.Target = null;
        }
    }
}
