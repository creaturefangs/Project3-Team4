using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NettingController : MonoBehaviour
{
    public LayerMask landLayer;
    public LayerMask waterLayer;
    public float catchChance = 0.5f; // Adjust this value to set the catch probability

    public GameObject NettingUI;
    public TMP_Text failureTXT;
    public TMP_Text caughtTXT;
    public List<GameObject> WaterCritters;
    public List<GameObject> LandCritters;

    public Animator FishingNet;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FishingNet.SetBool("netting",true);
            TryCatchAnimal();
        }
    }

    public void TryCatchAnimal()
    {
        if (IsPlayerInWater())
        {
            TryCatchAnimalInWater();
        }
        else
        {
            TryCatchAnimalOnLand();
        }
    }

    private bool IsPlayerInWater()
    {
        // Check if the player is colliding with the water layer
        return Physics.Raycast(transform.position, Vector3.down, 1f, waterLayer);
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
            StartCoroutine(HideFailureMessage());
        }
        else
        {
            Debug.LogWarning("Assign a TextMeshProUGUI component to the 'failureMessage' field in the Unity Editor.");
        }
    }

    private System.Collections.IEnumerator HideFailureMessage()
    {
        // Wait for a few seconds before hiding the failure message
        yield return new WaitForSeconds(2f);

        // Hide the failure message
        failureTXT.text = "";
    }

}

