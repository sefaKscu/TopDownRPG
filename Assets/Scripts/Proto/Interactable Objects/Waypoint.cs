// WayPoint System v0.1 designed by Sefa Kuscu

/***Future Plans
 * Level loader
 * UI integration 
 */

using UnityEngine;

public class Waypoint : MonoBehaviour, IPresenceSensitive, IInteractable
{
    #region Properties
    public string ID;
    public string zoneName;
    #endregion

    #region References
    [SerializeField]
    private GameObject runes;
    private float runesX;
    private float runesY;
    #endregion

    #region Conditions
    private bool activated= false;
    public bool Activated { get { return activated; } }
    #endregion

    #region MonoBehaviour Methods
    // Start is called before the first frame update
    void Start()
    {
        //runes.SetActive(false);
        //x value of runes transform
        runesX = runes.transform.position.x;
        //y value of runes transform
        runesY = runes.transform.position.y;
        //start -20 on z axis relative to the original position
        runes.transform.position= new Vector3(runesX, runesY , -20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    
    #region Interface Methods

    public void SeizePresence(Transform _transform)
    {
        activated= true;
        // lights up runes
        Debug.Log(_transform.gameObject.name + " activated waypoint");
        //return to original position
        runes.transform.position = new Vector3(runesX, runesY, 0f);
    }

    public void Interact()
    {
        // Open teleport menu
        Debug.Log("Teleport Menu Opened");
        //UIManager.MyInstance.OpenCloseMenu(_menuIndex);
    }

    #endregion

    #region Depricated Code
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        // lights up runes
    //        Debug.Log("Light Signs");
    //        //return to original position
    //        runes.transform.position = new Vector3(runesX, runesY, 0f);
    //    }
    //}
    #endregion
}
