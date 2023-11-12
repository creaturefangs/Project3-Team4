using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public int currency = 0;
    public int requirement;

    public List<string> items;

    public bool bountifulHarvest = false;
    public bool fishWhisperer = false;
    public bool strongerLine = false;

    // Start is called before the first frame update
    void Start()
    {
        Upgrades();
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
        
    }

    public void UpdateCurrency(int change)
    {
        TMP_Text text = GameObject.Find("Currency").GetComponent<TMP_Text>();
        currency += change;
        text.text = currency.ToString();
        if (change < 0) { } // If you lose fish...
        else if (change > 0) { } // If you gain fish...
    }

    public void SetCurrency(int current)
    {
        TMP_Text text = GameObject.Find("Currency").GetComponent<TMP_Text>();
        currency = current;
        text.text = currency.ToString();
    }

    public void BuyItem(int cost, string item)
    {
        if ((currency - cost) < 0) { } // Put player feedback here!
        else
        {
            items.Add(item);
            UpdateCurrency(-cost);
            Upgrades(); // Makes sure the newly purchased upgrade is activated.
        }
    }

    private void Upgrades() // Iterate through player's upgrades and activate them if not already active.
    {
        foreach (string item in items)
        {
            switch (item)
            {
                case "Bountiful Harvest": // There is a chance that caught fish will be worth 1-2 more fish.
                    if (!bountifulHarvest)
                    {
                        bountifulHarvest = true;
                        Sprite icon = GameObject.Find("BountifulHarvest_Icon").GetComponent<Sprite>();
                        // icon = Resources.Load<Sprite>("/UI/BountifulHarvest_Icon");
                    }
                    break;
                case "Fish Whisperer": // Better chances of uncommon and rare fish spawning.
                    if (!fishWhisperer)
                    {
                        fishWhisperer = true;
                        Sprite icon = GameObject.Find("FishWhisperer_Icon").GetComponent<Sprite>();
                        // icon = Resources.Load<Sprite>("/UI/FishWhisperer_Icon");
                    }
                    break;
                case "Stronger Line": // The fishing mini-game is easier overall.
                    if (!strongerLine)
                    {
                        strongerLine = true;
                        Sprite icon = GameObject.Find("StrongerRod_Icon").GetComponent<Sprite>();
                        // icon = Resources.Load<Sprite>("/UI/StrongerRod_Icon");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
