using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiggingController : MonoBehaviour
{
    
    public AudioSource playersource;
    public AudioClip diggingSFX;
    public Animator shovelController;

    //slider 
    public Slider slider;
    public float fillSpeed = 1f; // Fill speed in units per second
    public float maxValue = 100f; // Maximum value for the slider
    public GameObject[] objectsToInstantiate; // List of GameObjects to instantiate when the slider reaches 100

    private bool isEKeyDown = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the "E" key is being held down
        isEKeyDown = Input.GetKey(KeyCode.E);

        // Fill the slider only when the "E" key is being held down
        if (isEKeyDown)
        {
            FillSlider();
        }
        
    }

    public void Digging()
    {
        shovelController.SetBool("Digging", true);
        playersource.loop = true;
        playersource.PlayOneShot(diggingSFX);
        FillSlider();
    }

    void FillSlider()
    {
        // Ensure the slider is not already at the maximum value
        if (slider.value < maxValue)
        {
            float fillAmount = fillSpeed * Time.deltaTime;
            slider.value = Mathf.Clamp(slider.value + fillAmount, 0f, maxValue);
        }

        // Check if the slider is at the maximum value
        if (slider.value == maxValue)
        {
            // Instantiate a random GameObject from the list
            Instantiate(objectsToInstantiate[Random.Range(0, objectsToInstantiate.Length)], transform.position, Quaternion.identity);
        }
    }

    public void StopDigging()
    {
        shovelController.SetBool("Digging", false);
        playersource.Stop();

    }
}

