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
                requirement = 10;
                break;
            case "LevelThree":
                requirement = 10;
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

    public void BuyItem(int cost, string item)
    {
        if ((currency - cost) < 0) { } // Put player feedback here!
        else
        {
            items.Add(item);
            UpdateCurrency(-cost);
        }
    }
}
