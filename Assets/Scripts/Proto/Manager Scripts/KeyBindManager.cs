using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
    private static KeyBindManager instance;
    public static KeyBindManager MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<KeyBindManager>();
            }
            return instance;
        }
    }

    /// <summary>
    /// keybinds dictionary stores keybinds data, this hold a string and a KeyCode value
    /// </summary>
    public Dictionary<string, KeyCode> KeyBinds { get; private set; } = new Dictionary<string, KeyCode>();


    private string bindName;



    // Start is called before the first frame update
    void Start()
    {
        //assigning default values
        BindKey("Up", KeyCode.W);
        BindKey("Down", KeyCode.S);
        BindKey("Left", KeyCode.A);
        BindKey("Right", KeyCode.D);
        BindKey("Run", KeyCode.LeftShift);
        BindKey("Act1", KeyCode.Alpha1);
        BindKey("Act2", KeyCode.Alpha2);
        BindKey("Act3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        //temporary dictionary for checking if input value is allready assigned to a key
        Dictionary<string, KeyCode> currentDictionary = KeyBinds;

        //check if our input value has allready assigned to another key
        if (!currentDictionary.ContainsKey(key))
        {
            //if not assign the input value to a key
            currentDictionary.Add(key, keyBind);
        }


        //check if input value has allready assigned to some other key if so unassign
        else if (currentDictionary.ContainsValue(keyBind))
        {
            //Search the dictionary for our input value, when found, refer to it as allradyBoundedKey
            string allradyBoundedKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;
            //set its value to none
            currentDictionary[allradyBoundedKey] = KeyCode.None;
            //update text on the correspanded button as Unassigned
            UIManager.MyInstance.UpdateKeyText(allradyBoundedKey, "Unassigned");
        }

        currentDictionary[key] = keyBind;
        string keyName = keyBind.ToString();
        UIManager.MyInstance.UpdateKeyText(key, keyName);
        bindName = string.Empty;
    }

    public void KeyBindOnClick(string newBindName)
    {
        this.bindName = newBindName;
        UIManager.MyInstance.UpdateKeyText(newBindName, "Waiting");
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }

}
