using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public int currency = 0;
    private int fish = 0;
    private int bugs = 0;
    public int requirement;

    public string currentItem;

    public bool bountifulHarvest = false;
    public bool fishWhisperer = false;
    public bool looseDirt = false;
    public bool secondWind = false;
    public bool strongerLine = false;
    public bool thickNet = false;

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            // Change values later with play-testing.
            case "LevelOne":
                requirement = 10;
                break;
            case "LevelTwo":
                requirement = 15;
                break;
            case "LevelThree":
                requirement = 20;
                break;
            default:
                requirement = 10;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentItem = CurrentItem();
    }

    public void UpdateCurrency(string type, int change)
    {
        GameObject obj;
        int amt;
        switch (type)
        {
            case "bugs":
                obj = GameObject.Find("BugCurrency");
                bugs += change;
                amt = bugs;
                break;
            case "coins":
                obj = GameObject.Find("CoinCurrency");
                currency += change;
                amt = currency;
                break;
            case "fish":
                obj = GameObject.Find("FishCurrency");
                fish += change;
                amt = fish;
                break;
            default:
                obj = GameObject.Find("CoinCurrency");
                currency += change;
                amt = currency;
                break;
        }
        Animator animator = obj.GetComponent<Animator>();
        TMP_Text text = obj.GetComponent<TMP_Text>();
        
        text.text = amt.ToString();
        if (change < 0) { animator.Play("LoseCurrency", -1, 0f); } // If you lose some of the currency...
        else if (change > 0) { animator.Play("GainCurrency", -1, 0f); } // If you gain some of the currency...
    }

    public void SetCurrency(int money)
    {
        TMP_Text text = GameObject.Find("CoinCurrency").GetComponent<TMP_Text>();
        currency = money;
        text.text = currency.ToString();
        CheckRequirement();
    }

    public void BuyItem(int cost, string item)
    {
        if ((currency - cost) < 0) { } // Put player feedback here!
        else
        {
            UpdateCurrency("coins", -cost);
            switch (item)
            {
                case "BountifulHarvest": // There is a chance that caught fish will be worth 1-2 more fish.
                    bountifulHarvest = true;
                    break;
                case "FishWhisperer": // Better chances of uncommon and rare fish spawning.
                    fishWhisperer = true;
                    break;
                case "LooseDirt": // Less time spent digging.
                    looseDirt = true;
                    break;
                case "SecondWind":
                    secondWind = true; // Player sprints faster.
                    break;
                case "StrongerLine": // The fishing mini-game is shorter.
                    strongerLine = true;
                    break;
                case "ThickNet": // Better chance of catching something in the net.
                    thickNet = true;
                    break;
                default:
                    break;
            }
        }
        CheckRequirement();
    }

    private void CheckRequirement()
    {
        if (currency >= requirement)
        {
            //
        }
    }

    private string CurrentItem()
    {
        GameObject hand = GameObject.Find("ObjectInHand");
        foreach (Transform child in hand.transform)
        {
            if (child.gameObject.activeSelf) { return child.name; }
        }
        return null;
    }
}
