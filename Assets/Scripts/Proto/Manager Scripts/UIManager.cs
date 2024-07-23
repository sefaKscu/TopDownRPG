using System;
using UnityEngine;
using UnityEngine.UI;
using StatSystem;

/*UIManager v0,2 designed by Sefa Kuscu
 * 
 * ****** I MADE SOME CHANGES IN THE STAT RELATED METHODS REGION, TEST THEM OUT ******
 * 
 ***UI Manager holds references of the menus
 *open and close them
 *update stat frames
 *holds the references and input values for action buttons
 *
 *
 ***Future plans
 *moving stat frames to their own script
 *moving the inputs to an inputManager
 *
 *
 */

public class UIManager : MonoBehaviour
{
    #region UIManager Singleton
    /// <summary>
    /// this section is for accesing method from unrelated script.
    /// it's important to be static.
    /// Because if samething is static it's shared among all of the instances
    /// </summary>
    private static UIManager instance;
    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    #endregion

    #region References
    //array of references to menuPanel
    [SerializeField] private GameObject[] menuPanel;
    
    /* MenuPanel Index
     * 0. Options
     * 1. DeathPanel
     * 2. InventoryMenu
     * 3. StashMenu
     * 4. DropItemArea
     * 5.
     * 6.
     * 7.
     * 8.
     * 9.
     */

    [SerializeField] private Player player;


                     private NPC targetNPC;


    //fillers and texts
    [SerializeField]
    private Image playerVitalityFiller;
    [SerializeField]
    private Text playerVitalityText;

    [SerializeField]
    private Image playerMojoFiller;
    [SerializeField]
    private Text playerMojoText;

    [SerializeField]
    private Image targetVitalityFiller;
    [SerializeField] 
    private Text targetVitalityText;




    [SerializeField]
    private float lerpSpeed = 10.0f;


    //a reference to npc name text
    [SerializeField] private Text npcName;

    // reference to target frame
    [SerializeField] private GameObject targetFrame;

    [SerializeField] private bool thereIsActionBar = false; // this bool is to temporary fix to avoid NullReference.

    //array of references to actionButtons
    [SerializeField] private ActionButton[] actionButtons;    



    #endregion

    //custom keycodes for action buttons
    private KeyCode action1, action2, action3;

    private GameObject[] keyBindButtons;

    #region MonoBehaviour Methods

    private void OnValidate()
    {
        if (playerVitalityText == null && playerVitalityFiller != null)
            playerVitalityText = playerVitalityFiller.gameObject.GetComponentInChildren<Text>();

        if (playerMojoText == null && playerMojoFiller != null)
            playerMojoText = playerMojoFiller.gameObject.GetComponentInChildren<Text>();

        if (targetVitalityText == null && targetVitalityFiller != null)
            targetVitalityText = targetVitalityFiller.gameObject.GetComponentInChildren<Text>();
    }

    private void Awake()
    {
        //this line gets a referance for each of the keybind buttons tagged with "KeyBind"
        keyBindButtons = GameObject.FindGameObjectsWithTag("KeyBind");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (thereIsActionBar)
        {
            BindUsablesToActionButtons();
            AssignCustomKeyCodes();
        }

        CloseAllMenus();
    }

    private void BindUsablesToActionButtons()
    {
        SetUsable(actionButtons[0], SpellBook.MyInstance.GetSpell("Fireball"));
        SetUsable(actionButtons[1], SpellBook.MyInstance.GetSpell("Beam Of Purity"));
        SetUsable(actionButtons[2], SpellBook.MyInstance.GetSpell("Fireball"));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIStats();
    }

    #endregion

    #region UI inputs
    /// <summary>
    /// get inputs related to ui
    /// </summary>
    public void GetUIInput()
    {
        //check if custom keycodes pressed
        if (Input.GetKeyDown(action1))
        {
            ClickActionButton("Act1");
            Debug.Log("action1");
        }
        if (Input.GetKeyDown(action2))
        {
            ClickActionButton("Act2");
            Debug.Log("action2");
        }
        if (Input.GetKeyDown(action3))
        {
            ClickActionButton("Act3");
            Debug.Log("action3");
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //toggles options menu
            OpenCloseMenu(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllMenus();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenCloseMenu(2);
        }
        // OpenCloseMenu(3) input is on the StashChest
    }

    /// <summary>
    /// assigns custom keycodes
    /// </summary>
    private void AssignCustomKeyCodes()
    {
        //assigning custom keycodes to numeric keys
        action1 = KeyBindManager.MyInstance.KeyBinds["Act1"];
        action2 = KeyBindManager.MyInstance.KeyBinds["Act2"];
        action3 = KeyBindManager.MyInstance.KeyBinds["Act3"];
    }
    #endregion

    #region target frame

    /// <summary>
    /// activates the target frame on UI
    /// </summary>
    /// <param name="target"></param>
    public void ShowTargetFrame(NPC target)
    {
        targetNPC = target;
        targetFrame.SetActive(true);
        npcName.text = target.name;

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);     
    }

    /// <summary>
    /// deactivates the target frame on UI
    /// </summary>
    public void HideTargetFrame () 
    { 
        targetFrame.SetActive(false); 
    }

    #endregion

    #region open and close menus

    /// <summary>
    /// Toggles the indexed menu.
    /// </summary>
    /// <param name="menuIndex"></param>
    public void OpenCloseMenu(int _menuIndex)
    {
        //setting a reference to the canvas group of the menu
        CanvasGroup canvasGroup = menuPanel[_menuIndex].GetComponent<CanvasGroup>();

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = !canvasGroup.blocksRaycasts;
    }

    /// <summary>
    /// Opens the indexed menu.
    /// </summary>
    /// <param name="_menuIndex"></param>
    public void OpenMenu(int _menuIndex)
    {
        //setting a reference to the canvas group of the menu
        CanvasGroup canvasGroup = menuPanel[_menuIndex].GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// Closes the indexed menu.
    /// </summary>
    /// <param name="_menuIndex"></param>
    public void CloseMenu(int _menuIndex)
    {
        //setting a reference to the canvas group of the menu
        CanvasGroup canvasGroup = menuPanel[_menuIndex].GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Closes all menus.
    /// </summary>
    public void CloseAllMenus()
    {
        foreach (GameObject menu in menuPanel)
        {
            if (menu != null)
            {
                CanvasGroup cg = menu.GetComponent<CanvasGroup>();
                cg.alpha = 0;
                cg.blocksRaycasts = false;
            }
        }
    }

    #endregion

    #region Keybinds and Usables **

    /// <summary>
    /// updates keybinds in the controls menu
    /// </summary>
    /// <param name="key"></param>
    /// <param name="code"></param>
    public void UpdateKeyText(string key, string keyName)
    {
        Text tmp = Array.Find(keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = keyName;
    }

    /// <summary>
    /// clicks action button
    /// </summary>
    /// <param name="buttonName"></param>
    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    /// <summary>
    /// setting usables to buttons
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="usable"></param>
    public void SetUsable(ActionButton btn, IUsable usable)
    {
        btn.Icon.sprite = usable.MyIcon;
        btn.Icon.color = Color.white;
        btn.MyUsable = usable;
    }

    #endregion

    #region Stat Methods

    //maybe this section of methods can go into a whole new stat frame script

    private void UpdateUIStats()
    {
        if(player != null)
        {
            UpdateUIStat(player.Vitality, playerVitalityFiller, playerVitalityText);
            UpdateUIStat(player.Mojo, playerMojoFiller, playerMojoText);
            if (targetFrame.activeSelf)
            {
                UpdateUIStat(targetNPC.Vitality, targetVitalityFiller, targetVitalityText);
            }
        }
    }

    private void UpdateUIStat(Stat _stat, Image _filler,Text _statText = null)
    {
        _filler.gameObject.GetComponentInChildren<Text>().text = _stat.TextValue;
        if (_filler.fillAmount != _stat.FillAmount)
        {
            _filler.fillAmount = Mathf.Lerp(_filler.fillAmount, _stat.FillAmount, Time.deltaTime * lerpSpeed);
            if (_statText != null)
                _statText.text = _stat.TextValue;
        }
    }

    #endregion

}

