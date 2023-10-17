using UnityEngine;
using System;
using TMPro;  // Remember to add this namespace for TextMeshPro

public class TimeManager : MonoBehaviour
{
    private DateTime currentDate;
    private DateTime endDate;
    private float realTimePerGameMinute; // How many real-time seconds for each in-game minute.

    public TextMeshProUGUI timeText;  // Reference to your TextMeshPro UI element

    void Start()
    {
        currentDate = new DateTime(2018, 1, 1, 7, 0, 0); // Start on Monday 1 Jan 2018 at 7:00 AM.
        endDate = new DateTime(2018, 1, 7, 19, 0, 0); // End on Sunday 7 Jan 2018 at 7:00 PM.

        realTimePerGameMinute = 1.25f; // 1 second in real life equals 1 game minute.

        StartCoroutine(TimeProgression());
    }

    System.Collections.IEnumerator TimeProgression()
    {
        while (currentDate <= endDate)
        {
            UpdateTimeDisplay();  // Update the UI

            yield return new WaitForSeconds(realTimePerGameMinute); // Wait for the real-time equivalent of 1 game minute.

            currentDate = currentDate.AddMinutes(1); // Advance the game time by 1 minute.

            // If the time is 8:00 PM (after a 7:59 PM log), reset to 7:00 AM of the next day.
            if (currentDate.Hour == 20)
            {
                currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 7, 0, 0).AddDays(1);
            }
        }
    }

    void UpdateTimeDisplay()
    {
        timeText.text = currentDate.ToString("dddd, dd MMMM yyyy hh:mm tt");
    }
}