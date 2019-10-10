using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseMerchantManager : BaseMenu
{


    public Item.ItemRank shopRank;

    private Item currentItem;
    public ItemInfoDisplay display;

    public Text goldNumber;
    public Button buyButton;

    List<Item> market = new List<Item>();
    int marketSize = 3;
    public List<Button> marketButtons;

    List<Item> blacksmith = new List<Item>();
    int blacksmithSize = 3;
    public List<Button> blacksmithButtons;

    List<Item> atelier = new List<Item>();
    int atelierSize = 3;
    public List<Button> atelierButtons;


    //Singleton
    public static BaseMerchantManager instance;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadShops();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void Escape()
    {
        if (currentItem != null)
        {
            ResetCurrentItem();
        }
        else
        {
            ResetCurrentItem();
            BaseManager.instance.CancelButton();
        }
    }


    void LoadShops()
    {
        LoadMarket();
        LoadBlacksmith();
        LoadAtelier();

        UpdateShops();
    }
    void LoadMarket()
    {
        for (int i = 0; i < marketSize; i++)
        {
            market.Add(ItemManager.instance.GetConsumable(shopRank));
        }
    }
    void LoadBlacksmith()
    {
        for (int i = 0; i < blacksmithSize; i++)
        {
            blacksmith.Add(ItemManager.instance.GetWeapon(shopRank));
        }
    }
    void LoadAtelier()
    {
        for (int i = 0; i < atelierSize; i++)
        {
            atelier.Add(ItemManager.instance.GetArmor(shopRank));
        }
    }



    private void UpdateShops ()
    {
        //Update gold UI
        goldNumber.text = ResourceManager.instance.GetGold().ToString();

        UpdateShopItems(market, marketButtons);
        UpdateShopItems(blacksmith, blacksmithButtons);
        UpdateShopItems(atelier, atelierButtons);
    }
    private void UpdateShopItems (List<Item> shop, List<Button> buttons)
    {

        int counter = 0;
        foreach (Button button in buttons)
        {
            if (counter >= shop.Count)
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
                button.GetComponentInChildren<Text>().text = shop[counter].GetName();
            }

            counter++;
        }

    }
    private void UpdateGoldUI ()
    {
        //Update gold UI
        goldNumber.text = ResourceManager.instance.GetGold().ToString();


        if (currentItem != null)
        {
            buyButton.gameObject.SetActive(true);
        }
        else
        {
            buyButton.gameObject.SetActive(false);
        }

        if (currentItem != null && ResourceManager.instance.GetGold() >= currentItem.goldCost)
        {
            //If the player can afford the item, then they can press the buy button
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }

    }


    private void ResetCurrentItem()
    {
        currentItem = null;

        display.Deactivate();
        UpdateGoldUI();
    }


    public void BuyCurrentItem ()
    {
        ResourceManager.instance.SpendGold(currentItem.goldCost);
        ConvoyManager.instance.AddItem(currentItem);
        RemoveItem(currentItem);
    }
    public void OnMarketButton (int index)
    {
        currentItem = market[index];
        display.LoadItemInfo(currentItem);
        UpdateGoldUI();
    }
    public void OnBlacksmithButton(int index)
    {
        currentItem = blacksmith[index];
        display.LoadItemInfo(currentItem);
        UpdateGoldUI();
    }
    public void OnAtelierButton(int index)
    {
        currentItem = atelier[index];
        display.LoadItemInfo(currentItem);
        UpdateGoldUI();
    }


    //Remove an item from the merchant
    private void RemoveItem (Item item)
    {

        if (item is Weapon) { blacksmith.Remove(item); }
        else if (item is Armor) { atelier.Remove(item); }
        else if (item is Consumable) { market.Remove(item); }

        UpdateShops();
        ResetCurrentItem();

    }

}
