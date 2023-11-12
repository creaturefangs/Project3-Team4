using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool gameIsPaused = false;
    private GameObject pauseMenuUI;
    [SerializeField] private GameObject[] pauseMenuButtons;
    private GameObject playerUI;
    private GameObject helpMenu;
    public AudioSource pauseSound;
    public AudioClip pauseSFX;

    private FirstPersonLook look;
    private FirstPersonMovement movement;
    private Interactions interact;

    void Start()
    {
        pauseMenuUI = transform.GetChild(3).gameObject;
        playerUI = transform.GetChild(0).gameObject;
        helpMenu = pauseMenuUI.transform.GetChild(2).gameObject;

        movement = GameObject.Find("First Person Controller Minimal").GetComponent<FirstPersonMovement>();
        look = GameObject.Find("First Person Controller Minimal").GetComponentInChildren<FirstPersonLook>();
        interact = GetComponentInChildren<Interactions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        ExitMenu();
        Time.timeScale = 1f;

        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        gameIsPaused = false;
        AudioListener.pause = false;
        pauseSound.PlayOneShot(pauseSFX);
    }

    public void Pause()
    {
        EnterMenu();
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        gameIsPaused = true;
        pauseSound.PlayOneShot(pauseSFX);
        AudioListener.pause = false;
    }

    public void EnterMenu() // Enables and disables various things that shouldn't be accessible in menus/UI (like player/camera movement, interactions, the cursor...)
    {
        interact.canInteract = false;
        look.canMove = false;
        movement.TogglePlayerFreeze(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ExitMenu() // Reverses the above.
    {
        interact.canInteract = true;
        look.canMove = true;
        movement.TogglePlayerFreeze(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetHelpMenuActiveState()
    {
        if (helpMenu.activeSelf)
        {
            EnablePauseButtons(true);
            helpMenu.SetActive(false);
        }
        else
        {
            helpMenu.SetActive(true);
            EnablePauseButtons(false);
        }
    }

    private void EnablePauseButtons(bool enabled)
    {
        foreach (var button in pauseMenuButtons)
        {
            button.SetActive(enabled);
        }
    }

    public void LoadMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Loading Menu...");
        SceneManager.LoadScene("MAINMENU");
        AudioListener.pause = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}