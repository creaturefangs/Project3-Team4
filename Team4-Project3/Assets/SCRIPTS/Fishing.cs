using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    private int maxFish = 10;
    private int currentFish = 0;

    [Header("Fish Prefabs")]
    public GameObject commonFish;
    public GameObject uncommonFish;
    public GameObject rareFish;

    [Header("Fish Rarity")]
    public int rarityCommon = 60;
    public int rarityUncommon = 90;
    public int rarityRare = 100;

    [Header("Fish Value")]
    public int valueCommon = 1;
    public int valueUncommon = 3;
    public int valueRare = 5;

    private Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        if (currentFish < maxFish) { SpawnFish(); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnFish()
    {
        int chance = Random.Range(1, 101);
        GameObject prefab;
        if (chance <= rarityCommon) { prefab = commonFish; }
        else if (rarityCommon < chance && chance > rarityRare) { prefab = uncommonFish; }
        else { prefab = rareFish; }

        Instantiate(prefab);
    }

    private void StartMinigame(GameObject fish) // Call from FishBehavior script on fish.
    {
        GameObject ui = GameObject.Find("MinigamePanel");
        var uiHeight = ui.GetComponent<RectTransform>().sizeDelta.y;
        GameObject catchZone = ui.transform.GetChild(0).gameObject; // Set to whatever child num. it is in the mini-game UI.
        var zoneRect = catchZone.GetComponent<RectTransform>();
        ui.SetActive(true);

        string rarity = fish.GetComponent<FishBehavior>().rarity;
        switch (rarity)
        {
            case "common":
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * 0.6f);
                break;
            case "uncommon":
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * 0.3f);
                break;
            case "rare":
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * 0.1f);
                break;
            default:
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * 0.6f);
                break;
        }

    }

    private void CatchFish(GameObject fish)
    {
        string rarity = fish.GetComponent<FishBehavior>().rarity;

        if (rarity == "common") { inv.UpdateCurrency(valueCommon); }
        else if (rarity == "uncommon") { inv.UpdateCurrency(valueUncommon); }
        else if (rarity == "rare") { inv.UpdateCurrency(valueRare); }

        Destroy(fish);

    }
}