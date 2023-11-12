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
    private LayerMask water;

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

    [Header("Mini-Game Speed")]
    public float speedCommon = 0.15f;
    public float speedUncommon = 0.3f;
    public float speedRare = 0.5f;

    private Inventory inv;
    private FirstPersonMovement movement;
    private FirstPersonLook look;

    [Header("Mini-Game")]
    public float catchTime = 0f;
    private float minigameLength = 10f; // In seconds.
    public bool inMinigame = false;
    private GameObject minigameUI;
    private GameObject catchZone;

    private GameObject activeFish;
    private GameObject fish;
    private string rarity;

    // Start is called before the first frame update
    void Start()
    {
        // if (currentFish < maxFish) { SpawnFish(); }
        inv = gameObject.GetComponent<Inventory>();
        movement = GameObject.Find("First Person Controller Minimal").GetComponent<FirstPersonMovement>();
        look = GameObject.Find("First Person Controller Minimal").transform.GetChild(0).GetComponent<FirstPersonLook>();
        if (inv.fishWhisperer) // If the player has the Fish Whisperer shop upgrade...
        {
            rarityCommon = 40; // 40% chance of spawn.
            rarityUncommon = 80; // 40% chance of spawn.
            rarityRare = 100; // 20% chance of spawn.
        }

        minigameUI = transform.GetChild(3).GetChild(0).transform.gameObject;
        catchZone = minigameUI.transform.GetChild(1).gameObject;

        water = LayerMask.NameToLayer("Water");
        InvokeRepeating("SpawnFish", 5f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        if (inMinigame) // While player is in the fishing mini-game.
        {
            if (movement.canMove) { movement.TogglePlayerFreeze(); look.canMove = false; }
            if (catchTime >= minigameLength) // If the player has had the needle in the catch-zone for long enough, they win the mini-game.
            {
                EndMinigame();
                movement.TogglePlayerFreeze();
                look.canMove = true;
            }
        }
    }

    private void SpawnFish()
    {
        // Dynamically create a container for the spawned fish if it hasn't already been made via the script.
        activeFish = GameObject.Find("ActiveFish");
        if (!activeFish) { activeFish = new GameObject(); activeFish.name = "ActiveFish";  }

        int chance = Random.Range(1, 101);
        float xRange = Random.Range(610, 631);
        float zRange = Random.Range(460, 476);
        float yRotation = Random.Range(1, 361);
        
        GameObject prefab;
        if (chance <= rarityCommon) { prefab = commonFish; }
        else if (rarityCommon < chance && chance <= rarityUncommon) { prefab = uncommonFish; }
        else { prefab = rareFish; }

        GameObject newFish = Instantiate(prefab, new Vector3(xRange, 44f, zRange), Quaternion.Euler(0f, yRotation, 0f), activeFish.transform);
        ValidPosition(newFish);
    }

    private void ValidPosition(GameObject fish_obj)
    {
        RaycastHit hit;
        while (Physics.Raycast(fish_obj.transform.position, fish_obj.transform.TransformDirection(Vector3.up), out hit, 50f, ~water)) // While there's anything above the fish that isn't water...
        {
            fish_obj.transform.position = new Vector3(Random.Range(610, 631), 44f, Random.Range(460, 476));
        }
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
                anim.speed = speedCommon;
                //rect.sizeDelta = new Vector2(rect.sizeDelta.x, uiHeight * (0.6f + extra));
                break;
            case "uncommon":
                anim.speed = speedUncommon;
                //rect.sizeDelta = new Vector2(rect.sizeDelta.x, uiHeight * (0.3f + extra));
                break;
            case "rare":
                anim.speed = speedRare;
                //rect.sizeDelta = new Vector2(rect.sizeDelta.x, uiHeight * (0.1f + extra));
                break;
            default:
                anim.speed = speedCommon;
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