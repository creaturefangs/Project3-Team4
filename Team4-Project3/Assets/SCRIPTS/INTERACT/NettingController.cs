using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NettingController : MonoBehaviour
{
    public LayerMask landNetting;
    public LayerMask waterNetting;
    public float catchChance = 0.5f; // Adjust this value to set the catch probability

    public GameObject NettingUI;
    public TMP_Text failureTXT;
    public TMP_Text caughtTXT;
    public AudioSource NetSource;
    public AudioClip swingSFX;
    public List<GameObject> WaterCritters;
    public List<GameObject> LandCritters;


    public Animator FishingNetAnim;
    private GameObject fishingNet;
    private GameObject player;
    public string fishingNetTag = "FishingNet"; // Adjust the tag for your fishing net object
    public string playertag = "PLAYER";

    private void Start()
    {
        // Get the Animator component attached to the GameObject
        FishingNetAnim = fishingNet.GetComponent<Animator>();

        fishingNet = GameObject.FindGameObjectWithTag(fishingNetTag);

        if (fishingNet == null)
        {
            Debug.LogError("Fishing net object not found. Make sure to tag your fishing net object with the specified tag.");
        }

        player = GameObject.FindGameObjectWithTag(playertag);

        if (player == null)
        {
            Debug.LogError("player object not found. Make sure to tag your player object with the specified tag.");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FishingNetAnim.SetTrigger("Netting");
            NetSource.PlayOneShot(swingSFX);
            TryCatchAnimal();
        }
    }

    public void TryCatchAnimal()
    {
        if(IsPlayerInWater())
        {
            TryCatchAnimalInWater();
        }
        else if (IsPlayerOnLand())
        {
            TryCatchAnimalOnLand();
        }
    }

    private bool IsPlayerInWater()
    {
        // Check if the player is colliding with the water layer
        return Physics.Raycast(transform.position, Vector3.down, 1f, waterNetting);
    }

    private bool IsPlayerOnLand()
    {
        // Check if the player is colliding with the water layer
        return Physics.Raycast(transform.position, Vector3.down, 1f, landNetting);
    }

    private void TryCatchAnimalInWater()
    {
        // Implement logic for catching animals in water
        if (Random.value < catchChance)
        {
            // Animal caught!
            Debug.Log("Caught an animal in water!");
            // Choose a random index
            int randomIndex = Random.Range(0, WaterCritters.Count);

            // Instantiate a random object from the list
            InstantiateWaterCritter();

            // Display catch message
            NettingUI.SetActive(true);
            DisplayCatchMessage(WaterCritters[randomIndex].name);
        }
        else
        {
            // Animal escaped
            Debug.Log("Missed the catch in water.");
            // Animal escaped
            DisplayFailureMessage("You haven't caught anything!");
        }
    }

    private void TryCatchAnimalOnLand()
    {
        // Implement logic for catching animals on land
        if (Random.value < catchChance)
        {
            // Animal caught!
            Debug.Log("Caught an animal in land!");
            // Choose a random index
            int randomIndex = Random.Range(0, LandCritters.Count);

            // Instantiate a random object from the list
            InstantiateLandCritter();

            // Display catch message
            NettingUI.SetActive(true);
            DisplayCatchMessage(LandCritters[randomIndex].name);
        }
        else
        {
            Debug.Log("Missed the catch on the land.");

            // Animal escaped
            DisplayFailureMessage("You haven't caught anything!");
        }
    }
    private void InstantiateWaterCritter()
    {
        // Check if the list is not empty
        if (WaterCritters.Count > 0)
        {
            // Choose a random index
            int randomIndex = Random.Range(0, WaterCritters.Count);

            // Instantiate the object at the player's position (you may adjust the position as needed)
            Instantiate(WaterCritters[randomIndex], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("The list of caught objects is empty. Add objects to the list in the Unity Editor.");
        }
    }

    private void InstantiateLandCritter()
    {
        // Check if the list is not empty
        if (LandCritters.Count > 0)
        {
            // Choose a random index
            int randomIndex = Random.Range(0, WaterCritters.Count);

            // Instantiate the object at the player's position (you may adjust the position as needed)
            Instantiate(LandCritters[randomIndex], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("The list of caught objects is empty. Add objects to the list in the Unity Editor.");
        }
    }

    private void DisplayCatchMessage(string animalName)
    {
        // Display the catch message
        if (caughtTXT != null)
        {
            caughtTXT.text = "You have caught: " + animalName;
            StartCoroutine(HideMessage());
        }
        else
        {
            Debug.LogWarning("Assign a TextMeshProUGUI component to the 'catchMessage' field in the Unity Editor.");
        }
    }

    private void DisplayFailureMessage(string message)
    {
        // Display the failure message
        if (failureTXT != null)
        {
            failureTXT.text = message;

            // Start a coroutine to hide the failure message after a few seconds
            StartCoroutine(HideMessage());
        }
        else
        {
            Debug.LogWarning("Assign a TextMeshProUGUI component to the 'failureMessage' field in the Unity Editor.");
        }
    }

    private System.Collections.IEnumerator HideMessage()
    {
        // Wait for a few seconds before hiding the failure message
        yield return new WaitForSeconds(2f);

        // Hide the failure message
        NettingUI.SetActive(false);
        failureTXT.text = "";
        caughtTXT.text = "";
    }

}

