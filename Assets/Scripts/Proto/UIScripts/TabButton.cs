using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup; //a reference to the TabGroup Script
    public Image background;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = this.gameObject.name;
        background= GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    public void Select()
    {
        onTabSelected.Invoke();
    }

    public void DeSelect()
    {
        onTabDeSelected.Invoke();
    }
}
