using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    #region GameManager instance
    private static GameManager instance;
    public static GameManager MyInstance
    {
        get 
        { 
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>(); 
            }
            return instance; 
        }
    }
    #endregion

    [SerializeField] private Player player;

    private NPC currentTarget;


    // Start is called before the first frame update
    void Start()
    {
        //Getting information of layermask index
        //Debug.Log(LayerMask.GetMask("Clickable"));
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        UpdateTargetVitality();
    }

    /// <summary>
    /// this function checks if there is a foe on the cursor
    /// </summary>
    private void ClickTarget() 
    {
        //if we did not use if(Input.GetMouseButtonDown(0)) condition we would have our target no matter what.
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //deselect old target
            if(currentTarget != null)
            {
                currentTarget.DeSelect();
            }


            //targets
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if(hit.collider != null)
            {
                currentTarget = hit.collider.GetComponent<NPC>();
                if (currentTarget != null)
                {
                    player.Target = currentTarget.Select();
                    UIManager.MyInstance.ShowTargetFrame(currentTarget);
                }
            }

            else
            {
                if(currentTarget != null)
                    UIManager.MyInstance.HideTargetFrame();

                currentTarget = null;
                player.Target = null;
            }
        }
    }
    
    /// <summary>
    /// calls the updateTargetFrame function in the UIManager script
    /// </summary>
    private void UpdateTargetVitality()
    {
        if (currentTarget != null)
        {
            //UIManager.MyInstance.UpdateTargetFrame(currentTarget.Vitality.CurrentValue);
        }
    }
}
