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

    void Start()
    {
        pauseMenuUI = transform.GetChild(3).gameObject;
        playerUI = transform.GetChild(0).gameObject;
        helpMenu = pauseMenuUI.transform.GetChild(2).gameObject;

        look = GameObject.Find("First Person Controller Minimal").transform.GetChild(0).GetComponent<FirstPersonLook>();
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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        look.canMove = true;

        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        gameIsPaused = false;
        AudioListener.pause = false;
        pauseSound.PlayOneShot(pauseSFX);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        look.canMove = false;
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseSound.PlayOneShot(pauseSFX);
        AudioListener.pause = false;
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