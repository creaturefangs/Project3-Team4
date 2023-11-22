using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BugCollection : MonoBehaviour
{

    private Inventory inv;
    int currency;

    // Start is called before the first frame update
    void Start()
    {
        inv = gameObject.GetComponent<Inventory>();
    }


    public void CollectBug()
    {
        TMP_Text text = GameObject.Find("BugCurrency").GetComponent<TMP_Text>();
        UpdateCurrency(1);
       
    }

    public void UpdateCurrency(int change)
    {
        Animator animator = GameObject.Find("BugCurrency").GetComponent<Animator>();
        TMP_Text text = GameObject.Find("BugCurrency").GetComponent<TMP_Text>();
        currency += change;
        text.text = currency.ToString();
        if (change < 0) { animator.Play("LoseCurrency", -1, 0f); } // If you lose bugs
        else if (change > 0) { animator.Play("GainCurrency", -1, 0f); } // If you gain bugs
    }

    public void SetCurrency(int money)
    {
        TMP_Text text = GameObject.Find("BugCurrency").GetComponent<TMP_Text>();
        currency = money;
        text.text = currency.ToString();
        
    }

    public void BuyItem(int cost)
    {
        if ((currency - cost) < 0)
        {
            Debug.Log("Not enough currency to buy the item!"); // Add player feedback here
        }
        else
        {
            UpdateCurrency(-cost);
            // Additional logic for activating the newly purchased upgrade
        }
    }
}
