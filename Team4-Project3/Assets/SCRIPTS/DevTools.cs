using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    public bool devTools = false;
    public KeyCode toggleKey = KeyCode.Keypad0;

    private Fishing fishing;
    private Inventory inv;

    private int originalCurrency;

    // Start is called before the first frame update
    void Start()
    {
        fishing = GetComponent<Fishing>();
        inv = GetComponent<Inventory>();
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
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            fishing.SpawnFish();
        }
    }

    private void ToggleUpgrades()
    {
        inv.fishWhisperer = true;
        inv.bountifulHarvest = true;
        inv.strongerLine = true;
    }

    private void InfiniteMoney()
    {
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
