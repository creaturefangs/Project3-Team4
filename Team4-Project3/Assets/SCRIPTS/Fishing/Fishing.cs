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
    public float waterLevel;
    public float spawnRate = 10f; // In seconds.

    [Header("Prefabs")]
    public GameObject previewPrefab;
    public GameObject bobPrefab;
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
    public float speedRare = 0.45f;

    private Inventory inv;
    private PauseManager pause;

    [Header("Mini-Game")]
    [HideInInspector] public float catchTime = 0f;
    private float loseTime = 0f;
    private float minigameLength = 10f; // In seconds.
    [HideInInspector] public bool inMinigame = false;
    private GameObject meter;
    private GameObject catchZone;
    private TMP_Text timer;
    private GameObject tooltipPanel;

    private GameObject activeFish;
    private GameObject fish;
    private string rarity;
    private int fishValue;

    public bool lineCast = false;
    private Vector3 castPosition;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        inv = gameObject.GetComponent<Inventory>();
        pause = GetComponentInParent<PauseManager>();

        meter = transform.GetChild(3).GetChild(0).transform.gameObject;
        catchZone = meter.transform.GetChild(1).gameObject;
        timer = meter.transform.parent.GetComponentInChildren<TMP_Text>();
        tooltipPanel = transform.GetChild(2).gameObject;

        player = GameObject.Find("First Person Controller Minimal");

        InvokeRepeating("SpawnFish", 0f, spawnRate);

        switch (SceneManager.GetActiveScene().name)
        {
            case "LEVELONE":
                waterLevel = 50f;
                break;
            case "LEVELTWO":
                waterLevel = 44f;
                break;
            case "LEVELTHREE":
                waterLevel = -0.75f;
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
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E)) { EndMinigame(false); }

            catchTime = Mathf.Clamp(catchTime, 0f, minigameLength);
            timer.text = (minigameLength - catchTime).ToString("F1"); // "F1" shows only to first decimal place.
            if (catchTime >= minigameLength) // If the player has had the needle in the catch-zone for long enough, they win the mini-game.
            {
                EndMinigame(true);
            }
            else if (catchTime <= 0) { loseTime += Time.deltaTime; }
            else if (catchTime > 0) { loseTime -= Time.deltaTime; }
            if (loseTime >= 2.5f) { EndMinigame(false); }
        }
        else if (inv.currentItem == "FishingPole")
        {
            if (!lineCast) { castPosition = CastValid(); }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!lineCast) { CastLine(); } // Casts fishing line if it's not already out.
                else { RetrieveLine(); } // Retrieves the line if it's already out.
            }
        }
        if (inv.currentItem != "FishingPole")
        {
            GameObject preview = GameObject.Find("CastPreview");
            if (preview != null) { Destroy(preview); }
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
        float xRange = Random.Range(playerPos.x - 15f, playerPos.x + 16f);
        float zRange = Random.Range(playerPos.z - 15f, playerPos.z + 16f);
        float yRotation = Random.Range(1, 361);
        
        GameObject prefab;
        if (chance <= rarityCommon) { prefab = commonFish; }
        else if (rarityCommon < chance && chance <= rarityUncommon) { prefab = uncommonFish; }
        else { prefab = rareFish; }

        Instantiate(prefab, new Vector3(xRange, waterLevel, zRange), Quaternion.Euler(0f, yRotation, 0f), activeFish.transform);
    }

    private Vector3 CastValid()
    {
        GameObject preview = GameObject.Find("CastPreview"); // Check if the cast preview already exists.
        bool valid = false;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 40f, ~LayerMask.NameToLayer("Water"), QueryTriggerInteraction.Ignore)) // Check if looking at water.
        {
            float waterDistance = Vector3.Distance(hit.transform.position, player.transform.position);
            if (waterDistance > 0) // When raycast bugs out and hits the player collider, it registers as a distance of zero, so this evades that.
            {
                if (preview == null) // If preview does not already exist...
                {
                    preview = Instantiate(previewPrefab, hit.point, Quaternion.Euler(90, 0, 0));
                    preview.name = "CastPreview";
                }
                else { preview.transform.position = hit.point; }

                Collider terrain = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (terrain.Raycast(ray, out RaycastHit _, 50f)) // Check if terrain in front of player.
                {
                    float terrainDistance = Vector3.Distance(ray.GetPoint(50), player.transform.position);
                    if (terrainDistance > waterDistance) { valid = true; } // If the water is closer to the player (above the terrain), then the player can cast.
                    else { Debug.Log($"Terrain in front of water. Distance: {terrainDistance}, water distance: {waterDistance}."); }
                }
                else { valid = true; }
            }
            else if (preview != null) { Destroy(preview); }
        }
        if (valid)
        {
            preview.GetComponent<TMP_Text>().color = new Color32(30, 255, 25, 255);
            return hit.point;
        }
        else
        {
            // if (preview != null) { Destroy(preview); }
            if (preview != null) { preview.GetComponent<TMP_Text>().color = Color.red; }
            return Vector3.zero;
        }
    }

    private void CastLine()
    {
        if (castPosition != Vector3.zero)
        {
            GameObject bob = Instantiate(bobPrefab, castPosition, Quaternion.identity);
            bob.name = "FishingBob";
            lineCast = true;
            TMP_Text preview = GameObject.Find("CastPreview").GetComponent<TMP_Text>();
            preview.color = Color.white;
            GameObject.Find("First Person Controller Minimal").GetComponent<FirstPersonMovement>().TogglePlayerFreeze(false);
        }
    }

    private void RetrieveLine()
    {
        GameObject bob = GameObject.Find("FishingBob");
        Destroy(bob);
        lineCast = false;
        GameObject.Find("First Person Controller Minimal").GetComponent<FirstPersonMovement>().TogglePlayerFreeze(true);
    }

    public void StartMinigame(GameObject fish_obj) // Call from FishBehavior script on fish.
    {
        catchTime = 0f; // Resets the catch progress.
        loseTime = 0f;

        inMinigame = true;
        fish = fish_obj;
        // var uiHeight = meter.GetComponent<RectTransform>().sizeDelta.y;
        // RectTransform rect = catchZone.GetComponent<RectTransform>();
        Animator anim = catchZone.GetComponent<Animator>();
        meter.transform.parent.gameObject.SetActive(true);

        pause.EnterMenu(); // Freezes player and camera movement.
        tooltipPanel.GetComponentInChildren<TMP_Text>().text = "Hold SPACE to move the needle!";
        tooltipPanel.SetActive(true);

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
    }

    private void EndMinigame(bool caught_fish)
    {
        meter.transform.parent.gameObject.SetActive(false);
        tooltipPanel.SetActive(false);
        RetrieveLine();
        Destroy(fish);
        if (caught_fish) { CatchFish(); }
        else { pause.ExitMenu(); }
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
        fishValue = value;
        StartCoroutine(CatchResults());
    }

    private IEnumerator CatchResults()
    {
        int childCount = GameObject.Find("MainUI").transform.GetChild(0).childCount;
        GameObject panel = GameObject.Find("MainUI").transform.GetChild(0).GetChild(childCount-2).gameObject;
        pause.EnterMenu();
        panel.SetActive(true);
        panel.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = $"{fish.name.Split("(")[0].ToUpper()}!";
        panel.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = $"rarity: {rarity.ToUpper()}";
        panel.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = $"value: {fishValue} COINS";
        yield return new WaitForSeconds(5);
        panel.SetActive(false);
        pause.ExitMenu();
    }
}