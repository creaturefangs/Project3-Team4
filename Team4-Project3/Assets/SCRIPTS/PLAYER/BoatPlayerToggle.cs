using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatPlayerToggle : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player1UI;

    private bool isPlayer1Active = true;

    // Define the key to toggle between players
    public KeyCode toggleKey = KeyCode.T;

    void Update()
    {
        // Check if the toggle key is pressed
        if (Input.GetKeyDown(toggleKey))
        {
            // Toggle between player1 and player2
            isPlayer1Active = !isPlayer1Active;

            // Activate/deactivate player objects accordingly
            player1.SetActive(isPlayer1Active);
            player2.SetActive(!isPlayer1Active);
        }

        // Check if player1 is inactive before toggling
        if (!player1.activeSelf)
        {
            Debug.LogWarning("Player 1 is inactive!");
            player1UI.SetActive(false);
            return;
        }
        else
        {
            player1UI.SetActive(true);
        }
    }
}
