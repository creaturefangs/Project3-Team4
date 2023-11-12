using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    public bool devTools = false;
    public KeyCode toggleKey = KeyCode.Keypad0;

    private int originalCurrency;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            devTools = !devTools;
            InfiniteMoney();
            ToggleUpgrades();
            // EasyMinigame();
        }
    }

    private void ToggleUpgrades()
    {
        Inventory inv = GetComponent<Inventory>();
        inv.fishWhisperer = true;
        inv.bountifulHarvest = true;
        inv.strongerLine = true;
    }

    private void InfiniteMoney()
    {
        Inventory inv = GetComponent<Inventory>();
        if (devTools)
        {
            originalCurrency = inv.currency;
            inv.SetCurrency(999);
        }
        else
        {
            inv.SetCurrency(originalCurrency);
        }
    }

    private void EasyMinigame()
    {
        Fishing fishing = GetComponent<Fishing>();
        if (devTools)
        {
            fishing.speedCommon = 0.1f;
            fishing.speedUncommon = 0.1f;
            fishing.speedRare = 0.1f;
        }
        else
        {
            fishing.speedCommon = 0.15f;
            fishing.speedUncommon = 0.3f;
            fishing.speedRare = 0.5f;
        }
    }
}
