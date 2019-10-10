using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EventTextManager : MonoBehaviour
{


    public Canvas eventTextCanvas;
    public Text eventText;

    MouseControl.MouseState prevState;

    UnityEvent func;


    //Singleton
    public static EventTextManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisplayText (string txt, UnityEvent f)
    {
        eventText.text = txt;
        eventTextCanvas.gameObject.SetActive(true);

        return;
    }
    
    public void EndText ()
    {
        eventTextCanvas.gameObject.SetActive(false);
        func.Invoke();
    }


}
