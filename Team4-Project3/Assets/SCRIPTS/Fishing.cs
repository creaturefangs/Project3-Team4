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
    public float catchTime = 0f;
    private bool inMinigame = false;
    private GameObject minigameUI;
    private RectTransform catchZone;
    private RectTransform catchNeedle;

    private GameObject fish;
    private string rarity;

    // Start is called before the first frame update
    void Start()
    {
        // if (currentFish < maxFish) { SpawnFish(); }
        inv = gameObject.GetComponent<Inventory>();
        if (inv.fishWhisperer) // If the player has the Fish Whisperer shop upgrade...
        {
            rarityCommon = 40; // 40% chance of spawn.
            rarityUncommon = 80; // 40% chance of spawn.
            rarityRare = 100; // 20% chance of spawn.
        }

        minigameUI = GameObject.Find("MinigamePanel").transform.GetChild(0).gameObject;
        catchZone = minigameUI.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        catchNeedle = minigameUI.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
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
                inMinigame = false;
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
        inMinigame = true;
        fish = fish_obj;
        var uiHeight = minigameUI.GetComponent<RectTransform>().sizeDelta.y;
        minigameUI.SetActive(true);

        float extra = 0;
        if (inv.strongerLine) { extra = 0.1f; } // If the player has the Stronger Line shop upgrade, make the viable catch zone larger.
        rarity = fish.GetComponent<FishBehavior>().rarity;
        switch (rarity)
        {
            case "common":
                catchZone.sizeDelta = new Vector2(catchZone.sizeDelta.x, uiHeight * (0.6f + extra));
                break;
            case "uncommon":
                catchZone.sizeDelta = new Vector2(catchZone.sizeDelta.x, uiHeight * (0.3f + extra));
                break;
            case "rare":
                catchZone.sizeDelta = new Vector2(catchZone.sizeDelta.x, uiHeight * (0.1f + extra));
                break;
            default:
                catchZone.sizeDelta = new Vector2(catchZone.sizeDelta.x, uiHeight * (0.6f + extra));
                break;
        }
        catchTime = 0f; // Resets the catch progress.
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