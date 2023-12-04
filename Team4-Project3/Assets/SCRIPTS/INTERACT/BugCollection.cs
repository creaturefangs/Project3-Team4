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
        
    }


    public void CollectBug()
    {
        inv = GameObject.Find("MainUI").GetComponentInChildren<Inventory>();
        inv.UpdateCurrency("bugs", 1);
        inv.UpdateCurrency("coins", 1);
    }
}
