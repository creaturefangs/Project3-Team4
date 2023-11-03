using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTime : MonoBehaviour
{
    private TMP_Text clock;
    private float timeSince = 0.0f;
    public float hourLength = 30.0f; // The length of in-game hours in seconds. An hour length of 30s should make levels 360 seconds long (6 minutes).

    public string timeOfDay = "morning"; // Be called by fish script to alter fish spawning? Or maybe have shop only open at certain time or day?
    private int hour = 6;
    private int minute = 0;

    private Inventory inv;

    // Start is called before the first frame update
    void Start()
    {
        clock = GameObject.Find("Clock").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSince += Time.deltaTime;
        UpdateClock();
        CheckTimeOfDay();
    }

    private void UpdateClock()
    {
        // Handle time as 24-hour format, then at the very end convert to 12-hour format.

        hour = 6 + (int)Mathf.Floor(timeSince / hourLength);
        minute = (int)Mathf.Floor(((timeSince / hourLength) - (hour - 6)) * 60);
        int formattedHour = hour; // Using 12-hour time.
        string twelveHour = "am";
        if (hour > 11) { twelveHour = "pm"; } // If time is past 11am...
        string hourFill = "0";
        string minFill = "0";

        if (hour >= 10 && hour < 13) { hourFill = ""; } // If it's 10am, 11am, or 12pm, don't put a placeholder zero in the clock.
        if (minute > 9) { minFill = ""; } // If the minutes go into double digits, don't put a placeholder zero.
        if (hour > 12) { formattedHour -= 11; } // Converting to 12-hour time.
        clock.text = $"{hourFill}{formattedHour}:{minFill}{minute} {twelveHour}";
    }

    private void CheckTimeOfDay()
    {
        if (hour < 12) { timeOfDay = "morning"; }
        else if (hour >= 12 && hour < 16) { timeOfDay = "afternoon"; }
        else if (hour >= 16 && hour < 18) { timeOfDay = "evening"; }
        else if (hour >= 18)
        {
            // Call function from other script here to check for win condition.
            if (inv.currency >= inv.requirement) { SceneManager.LoadScene(""); } // NEXT LEVEL
            else { SceneManager.LoadScene("GameOver"); } // Game ends if player doesn't have enough fish.
        }
    }
}
