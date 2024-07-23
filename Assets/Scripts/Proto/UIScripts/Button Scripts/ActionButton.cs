using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    public IUsable MyUsable { get; set; }

    public Button MyButton { get; private set; }

    [SerializeField]
    private Image icon;

    public Image Icon
    {
        get { return icon; }
        set { icon = value; }
    }  

    // Start is called before the first frame update
    void Start()
    {
        MyButton= GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if(MyUsable != null)
        {
            MyUsable.Use();
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
