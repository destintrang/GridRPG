using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExpandInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public string info;
    public Text infoField;
    public GameObject infoObject;


    private void Start()
    {
        
    }

    //Detect if the Cursor starts to pass over the GameObject
    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        infoField.text = info;
        infoObject.SetActive(true);
        infoField.gameObject.SetActive(true);
    }

    //Detect when Cursor leaves the GameObject
    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        infoField.gameObject.SetActive(false);
        infoObject.SetActive(false);
        infoField.text = "";
    }

}
