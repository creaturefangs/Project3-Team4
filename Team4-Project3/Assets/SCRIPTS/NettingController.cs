using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NettingController : MonoBehaviour
{
    public LayerMask landLayer;
    public LayerMask waterLayer;
    public float catchChance = 0.5f; // Adjust this value to set the catch probability

    public List<GameObject> WaterCritters;
    public List<GameObject> LandCritters;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
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

            // Instantiate a random object from the list
            InstantiateWaterCritter();
        }
        else
        {
            // Animal escaped
            Debug.Log("Missed the catch in water.");
        }
    }

    private void TryCatchAnimalOnLand()
    {
        // Implement logic for catching animals on land
        if (Random.value < catchChance)
        {
            // Animal caught!
            Debug.Log("Caught an animal on land!");
        }
        else
        {
            // Animal escaped
            Debug.Log("Missed the catch on land.");
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

}

