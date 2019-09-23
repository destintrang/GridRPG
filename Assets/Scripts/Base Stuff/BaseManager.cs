using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : BaseMenu
{

    //Turn this ON when the battle starts
    public MouseControl mc;
    
    //Turn this OFF when the battle starts
    public GameObject startingTiles;

    public Camera battleCamera;
    public Camera baseCamera;

    public Canvas mainCanvas;
    public Canvas unitsCanvas;
    public Canvas repositionCanvas;
    Canvas currentCanvas;
    Stack<BaseMenu> menuStack;

    private BaseMenu currentBaseMenu;


    //Singleton
    public static BaseManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        menuStack = new Stack<BaseMenu>();
        currentBaseMenu = this;
        currentCanvas = mainCanvas;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInput();
    }


    public override void Enter()
    {
        StartButton();
    }
    


    public void UnitsButton ()
    {
        BaseUnitManager.instance.LoadPartyUnits();
        menuStack.Push(currentBaseMenu);
        currentBaseMenu = BaseUnitManager.instance;
        StartCoroutine(SwitchCanvas(unitsCanvas));
    }

    public void MapButton ()
    {
        menuStack.Push(currentBaseMenu);
        currentBaseMenu = BaseRepositionManager.instance;
        StartCoroutine(ViewMap());
    }

    public void ItemsButton ()
    {
        BaseItemsManager.instance.LoadPartyUnits();
        menuStack.Push(currentBaseMenu);
        currentBaseMenu = BaseItemsManager.instance;
        StartCoroutine(SwitchCanvas(currentBaseMenu.canvas));
    }

    public void ConvoyButton (GameObject character)
    {
        BaseConvoyManager.instance.LoadConvoyMenu(character);
        menuStack.Push(currentBaseMenu);
        currentBaseMenu = BaseConvoyManager.instance;
        StartCoroutine(SwitchCanvas(currentBaseMenu.canvas));
    }


    IEnumerator SwitchCanvas(Canvas n)
    {
        Darkness.instance.FadeIn();
        while (Darkness.instance.fading) { yield return null; }

        currentCanvas.gameObject.SetActive(false);
        n.gameObject.SetActive(true);
        currentCanvas = n;

        Darkness.instance.FadeOut();
        while (Darkness.instance.fading) { yield return null; }
    }


    public void CancelButton ()
    {
        if (menuStack.Count != 0)
        {
            BaseMenu previous = menuStack.Pop();
            StartCoroutine(SwitchCanvas(previous.canvas));
            currentBaseMenu = previous;

            if (battleCamera.enabled == true)
            {
                baseCamera.enabled = true;
                battleCamera.enabled = false;
            }

        }
        else if (menuStack.Count == 0)
        {
            Debug.Log("Stack is empty");
        }
    }

    public void StartButton ()
    {
        mc.enabled = true;

        TurnOffBase();

        //Once the battle starts, record the tiles of the player units
        TileManager.instance.RecordPlayerUnits();

        //Once the battle starts, record all the units being used this battle
        TurnManager.instance.SetPlayerUnits(PartyManager.instance.GetDeployedUnits());

        StartCoroutine(ViewMap());
    }

    IEnumerator ViewMap ()
    {
        Darkness.instance.FadeIn();
        while (Darkness.instance.fading) { yield return null; }

        mainCanvas.gameObject.SetActive(false);

        currentCanvas.gameObject.SetActive(false);
        repositionCanvas.gameObject.SetActive(true);
        currentCanvas = repositionCanvas;

        baseCamera.enabled = false;
        battleCamera.enabled = true;

        Darkness.instance.FadeOut();
        while (Darkness.instance.fading) { yield return null; }
    }


    private void CheckForInput ()
    {
        if (Input.GetKeyDown(KeyCode.E)) { Debug.Log(currentBaseMenu); currentBaseMenu.E(); }
        if (Input.GetKeyDown(KeyCode.Space)) { currentBaseMenu.SpaceBar(); }
        if (Input.GetKeyDown(KeyCode.Escape)) { currentBaseMenu.Escape(); }
        if (Input.GetKeyDown(KeyCode.Return)) { currentBaseMenu.Enter(); }
        if (Input.GetKeyDown(KeyCode.A)) { currentBaseMenu.A(); }
        if (Input.GetKeyDown(KeyCode.D)) { currentBaseMenu.D(); }
    }


    private void TurnOffBase ()
    {
        startingTiles.SetActive(false);
        BaseRepositionManager.instance.enabled = false;
    }


}
