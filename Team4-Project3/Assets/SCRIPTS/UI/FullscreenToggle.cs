using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    
    private bool isFullscreen = false;

    private void Start()
    {
        // Initialize with the current fullscreen state
        isFullscreen = Screen.fullScreen;
    }

    private void Update()
    {
        // Check for a key press or some other event to toggle fullscreen
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    ToggleFullscreen();
        //}
    }

   public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

}
