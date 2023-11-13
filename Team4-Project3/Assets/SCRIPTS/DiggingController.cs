using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DiggingController : MonoBehaviour
{
    public GameObject diggingUI;
    public AudioSource diggingSource;
    public AudioClip digDone; 
    public Animator shovelController;

    //slider 
    public Slider slider;
    public float fillSpeed = 1f; // Fill speed in units per second
    public float maxValue = 20f; // Maximum value for the slider
    public GameObject[] objectsToInstantiate; // List of GameObjects to instantiate when the slider reaches 100
    //for spawning objects
    public float moveSpeed = 2f; // Movement speed in units per second
    public float moveDistance = 3f; // Distance to move upward and downward
    private bool isEKeyDown = false;
    private bool hasRun = false; // Flag to track if the code has run

    private Interactions interactions;

    void Start()
    {
        interactions = GameObject.Find("MainUI").GetComponentInChildren<Interactions>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the "E" key is being held down
        isEKeyDown = Input.GetKey(KeyCode.E);

        if (interactions.canDig)
        {
            // Fill the slider only when the "E" key is being held down
            if (isEKeyDown)
            {
                FillSlider();
                diggingUI.SetActive(true);
            }
            else
            {
                // Stop filling the slider when the "E" key is not pressed
                StopDigging();
                diggingUI.SetActive(false);
            }
        }
    }

    public void Digging()
    {
        shovelController.SetBool("Digging", true);
        diggingSource.Play();
        FillSlider();
    }

    void FillSlider()
    {
        // Ensure the slider is not already at the maximum value
        if (slider.value < maxValue && !hasRun)
        {
            float fillAmount = fillSpeed * Time.deltaTime;
            slider.value = Mathf.Clamp(slider.value + fillAmount, 0f, maxValue);
        }

        // Check if the slider is at the maximum value
        if (slider.value == maxValue && !hasRun)
        {
            // Calculate the spawn position in front of the parent object
            Vector3 spawnPosition = transform.position + transform.forward * 1f; // Adjust the distance as needed

            // Instantiate a random GameObject from the list at the calculated position
            int randomIndex = Random.Range(0, objectsToInstantiate.Length);
            GameObject spawnedObject = Instantiate(objectsToInstantiate[randomIndex], spawnPosition, Quaternion.identity);

            // Move the spawned object upward and stop after a certain distance
            StartCoroutine(MoveObjectUpAndDown(spawnedObject));
            Debug.Log("Instantiated Bug");

            StopDigging();
            // Bug  spawns and player stops digging.
            diggingSource.PlayOneShot(digDone);
            // Set the flag to true so the code doesn't run again
            hasRun = true;
        }
    }

    IEnumerator MoveObjectUpAndDown(GameObject obj)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = obj.transform.position;

        while (elapsedTime < moveDistance / moveSpeed)
        {
            obj.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the position to the initial position
        obj.transform.position = initialPosition;

        // Optionally, perform any additional actions or logic after moving the object back down
    }

    public void StopDigging()
    {
        shovelController.SetBool("Digging", false);
        diggingSource.Stop();
    }
}

