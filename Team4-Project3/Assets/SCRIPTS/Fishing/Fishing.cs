using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Fishing : MonoBehaviour
{
    private int maxFish = 10;
    private int currentFish = 0;
    private LayerMask water;
    private float waterLevel;

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
    private PauseManager pause;

    [Header("Mini-Game")]
    [HideInInspector] public float catchTime = 0f;
    private float minigameLength = 10f; // In seconds.
    [HideInInspector] public bool inMinigame = false;
    private GameObject meter;
    private GameObject catchZone;
    private TMP_Text timer;

    private GameObject activeFish;
    private GameObject fish;
    private string rarity;

    // Start is called before the first frame update
    void Start()
    {
        inv = gameObject.GetComponent<Inventory>();
        pause = GetComponentInParent<PauseManager>();

        meter = transform.GetChild(3).GetChild(0).transform.gameObject;
        catchZone = meter.transform.GetChild(1).gameObject;
        timer = meter.transform.parent.GetComponentInChildren<TMP_Text>();

        water = LayerMask.NameToLayer("Water");
        InvokeRepeating("SpawnFish", 0f, 15f);

        switch (SceneManager.GetActiveScene().name)
        {
            case "LEVELONE":
                waterLevel = 50f;
                break;
            case "LEVELTWO":
                waterLevel = 44f;
                break;
            default:
                waterLevel = 44f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inMinigame) // While player is in the fishing mini-game.
        {
            catchTime = Mathf.Clamp(catchTime, 0f, minigameLength);
            timer.text = (minigameLength - catchTime).ToString("F1"); // "F1" shows only to first decimal place.
            if (catchTime >= minigameLength) // If the player has had the needle in the catch-zone for long enough, they win the mini-game.
            {
                EndMinigame();
            }
        }
    }

    public void SpawnFish()
    {
        // Dynamically create a container for the spawned fish if it hasn't already been made via the script.
        activeFish = GameObject.Find("ActiveFish");
        if (!activeFish) { activeFish = new GameObject(); activeFish.name = "ActiveFish";  }

        if (inv.fishWhisperer) // If the player has the Fish Whisperer shop upgrade...
        {
            rarityCommon = 40; // 40% chance of spawn.
            rarityUncommon = 80; // 40% chance of spawn.
            rarityRare = 100; // 20% chance of spawn.
        }

        int chance = Random.Range(1, 101);
        Vector3 playerPos = GameObject.Find("First Person Controller Minimal").transform.position;
        float xRange = Random.Range(playerPos.x - 20f, playerPos.x + 21f);
        float zRange = Random.Range(playerPos.z - 20f, playerPos.z + 21f);
        float yRotation = Random.Range(1, 361);
        
        GameObject prefab;
        if (chance <= rarityCommon) { prefab = commonFish; }
        else if (rarityCommon < chance && chance <= rarityUncommon) { prefab = uncommonFish; }
        else { prefab = rareFish; }

        GameObject newFish = Instantiate(prefab, new Vector3(xRange, waterLevel, zRange), Quaternion.Euler(0f, yRotation, 0f), activeFish.transform);
        ValidPosition(newFish);
    }

    private void ValidPosition(GameObject fish_obj)
    {
        float distance = Vector3.Distance(fish_obj.transform.position, GameObject.Find("First Person Controller Minimal").transform.position);
        // Vector3 terrainHeight;
        // terrainHeight.y = Terrain.activeTerrain.SampleHeight(fish_obj.transform.position);
        RaycastHit hit;
        while (Physics.Raycast(fish_obj.transform.position, fish_obj.transform.TransformDirection(Vector3.up), out hit, 50f, ~water)) // While there's anything above the fish that isn't water...
        {
            // distance = Vector3.Distance(fish_obj.transform.position, GameObject.Find("First Person Controller Minimal").transform.position);
            fish_obj.transform.position = new Vector3(Random.Range(610, 631), 44f, Random.Range(460, 476));
        }
    }

    public void StartMinigame(GameObject fish_obj) // Call from FishBehavior script on fish.
    {
        inMinigame = true;
        fish = fish_obj;
        // var uiHeight = meter.GetComponent<RectTransform>().sizeDelta.y;
        // RectTransform rect = catchZone.GetComponent<RectTransform>();
        Animator anim = catchZone.GetComponent<Animator>();
        meter.transform.parent.gameObject.SetActive(true);

        pause.EnterMenu(); // Freezes player and camera movement.
        TMP_Text tooltipText = GameObject.Find("TooltipPanel").GetComponentInChildren<TMP_Text>();
        tooltipText.text = "Hold SPACE to move the needle!";

        if (inv.strongerLine) { minigameLength = 7.5f; } // If the player has the Stronger Line shop upgrade, the time needed to stay in the catch-zone is shorter by 25%.

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
        meter.transform.parent.gameObject.SetActive(false);
        CatchFish();
        inMinigame = false;
        pause.ExitMenu();
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