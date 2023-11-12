using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Mini-Game")]
    public float catchTime = 0f;
    private float minigameLength = 10f; // In seconds.
    public bool inMinigame = false;
    private GameObject minigameUI;
    private GameObject catchZone;

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

        minigameUI = transform.GetChild(3).GetChild(0).transform.gameObject;
        catchZone = minigameUI.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) { inMinigame = !inMinigame; }
        if (inMinigame) // While player is in the fishing mini-game.
        {
            if (catchTime >= minigameLength) // If the player has had the needle in the catch-zone for long enough, they win the mini-game.
            {
                EndMinigame();
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
        // var uiHeight = minigameUI.GetComponent<RectTransform>().sizeDelta.y;
        // RectTransform rect = catchZone.GetComponent<RectTransform>();
        Animator anim = catchZone.GetComponent<Animator>();
        minigameUI.transform.parent.gameObject.SetActive(true);

        // float extra = 0;
        // if (inv.strongerLine) { extra = 0.1f; } // If the player has the Stronger Line shop upgrade, make the viable catch zone larger.
        rarity = fish.GetComponent<FishBehavior>().rarity;
        switch (rarity)
        {
            case "common":
                anim.speed = 0.15f;
                //rect.sizeDelta = new Vector2(rect.sizeDelta.x, uiHeight * (0.6f + extra));
                break;
            case "uncommon":
                anim.speed = 0.3f;
                //rect.sizeDelta = new Vector2(rect.sizeDelta.x, uiHeight * (0.3f + extra));
                break;
            case "rare":
                anim.speed = 0.6f;
                //rect.sizeDelta = new Vector2(rect.sizeDelta.x, uiHeight * (0.1f + extra));
                break;
            default:
                anim.speed = 0.15f;
                //rect.sizeDelta = new Vector2(rect.sizeDelta.x, uiHeight * (0.6f + extra));
                break;
        }
        catchTime = 0f; // Resets the catch progress.
    }

    private void EndMinigame()
    {
        minigameUI.transform.parent.gameObject.SetActive(false);
        CatchFish();
        inMinigame = false;
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
}