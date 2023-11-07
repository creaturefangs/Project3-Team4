using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject[] pauseMenuButtons;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject helpMenu;
    public AudioSource pauseSFX;

    void Start()
    {

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
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        gameIsPaused = false;
        AudioListener.pause = false;
        pauseSFX.Play();

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseSFX.Play();
        AudioListener.pause = true;
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