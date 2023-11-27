using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public int currency = 0;
    public int requirement;

    public string currentItem;

    public bool bountifulHarvest = false;
    public bool fishWhisperer = false;
    public bool looseDirt = false;
    public bool strongerLine = false;

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

    public void UpdateCurrency(int change)
    {
        Animator animator = GameObject.Find("FishCurrency").GetComponent<Animator>();
        TMP_Text text = GameObject.Find("FishCurrency").GetComponent<TMP_Text>();
        currency += change;
        text.text = currency.ToString();
        if (change < 0) { animator.Play("LoseCurrency", -1, 0f); } // If you lose fish...
        else if (change > 0) { animator.Play("GainCurrency", -1, 0f); } // If you gain fish...
    }

    public void SetCurrency(int money)
    {
        TMP_Text text = GameObject.Find("FishCurrency").GetComponent<TMP_Text>();
        currency = money;
        text.text = currency.ToString();
        CheckRequirement();
    }

    public void BuyItem(int cost, string item)
    {
        if ((currency - cost) < 0) { } // Put player feedback here!
        else
        {
            UpdateCurrency(-cost);
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
                case "StrongerLine": // The fishing mini-game is shorter.
                    strongerLine = true;
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
