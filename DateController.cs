using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateController : MonoBehaviour
{
    private string[] daysOfWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    private int currentDayIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DayCycleCoroutine());
    }

    // A coroutine that cycles through the days of the week every second
    IEnumerator DayCycleCoroutine()
    {
        while (true)
        {
            // Print in Console
            // Debug.Log("Current day: " + daysOfWeek[currentDayIndex]);

            // Increase currentDayIndex by 1 or reset to 0 if it's the end of the week
            currentDayIndex = (currentDayIndex + 1) % daysOfWeek.Length;

            yield return new WaitForSeconds(1f);
        }
    }
}