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
    public int rarityCommon = 60; // 60% chance of spawn.
    public int rarityUncommon = 90; // 30% chance of spawn.
    public int rarityRare = 100; // 10% chance of spawn.

    [Header("Fish Value")]
    public int valueCommon = 1;
    public int valueUncommon = 3;
    public int valueRare = 5;

    private Inventory inv;

    private float minigameLength = 15f; // In seconds.
    private float catchTime = 0f;
    private bool inMinigame = false;
    private RectTransform catchZone;
    private RectTransform catchNeedle;

    private GameObject fish;
    private string rarity;

    // Start is called before the first frame update
    void Start()
    {
        if (currentFish < maxFish) { SpawnFish(); }
        if (inv.fishWhisperer) // If the player has the Fish Whisperer shop upgrade...
        {
            rarityCommon = 40; // 40% chance of spawn.
            rarityUncommon = 80; // 40% chance of spawn.
            rarityRare = 100; // 20% chance of spawn.
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (inMinigame) // While player is in the fishing mini-game.
        {
            if (rectOverlaps(catchNeedle, catchZone)) { catchTime += Time.deltaTime; } // If the needle is inside the catch-zone...
            else { catchTime -= Time.deltaTime; } // If the needle is outside the catch-zone...
            if (catchTime >= minigameLength) // If the player has had the needle in the catch-zone for long enough, they win the mini-game.
            {
                CatchFish();
            }
        }
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

    public void StartMinigame(GameObject fish_obj) // Call from FishBehavior script on fish.
    {
        fish = fish_obj;
        GameObject ui = GameObject.Find("MinigamePanel");
        var uiHeight = ui.GetComponent<RectTransform>().sizeDelta.y;
        GameObject catchZone = ui.transform.GetChild(0).gameObject; // Set to whatever child num. it is in the mini-game UI.
        var zoneRect = catchZone.GetComponent<RectTransform>();
        ui.SetActive(true);

        float extra = 0;
        if (inv.strongerLine) { extra = 0.1f; } // If the player has the Stronger Line shop upgrade, make the viable catch zone larger.
        rarity = fish.GetComponent<FishBehavior>().rarity;
        switch (rarity)
        {
            case "common":
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * (0.6f + extra));
                break;
            case "uncommon":
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * (0.3f + extra));
                break;
            case "rare":
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * (0.1f + extra));
                break;
            default:
                zoneRect.sizeDelta = new Vector2(zoneRect.sizeDelta.x, uiHeight * (0.6f + extra));
                break;
        }
        catchTime = 0f;
    }

    private void CatchFish()
    {
        int value = 0;

        if (rarity == "common") { value += valueCommon; }
        else if (rarity == "uncommon") { value += valueUncommon; }
        else if (rarity == "rare") { value += valueRare; }

        if (inv.bountifulHarvest) // If the player has the Bountiful Harvest shop upgrade...
        {
            int chance = Random.Range(1, 101);
            if (chance <= 33) { value += Random.Range(1, 3); } // A 33% chance of the fish value being 1-2 fish higher in value.
        }
        inv.UpdateCurrency(value);
        Destroy(fish);

    }

    bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2) // https://stackoverflow.com/a/42044325
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}