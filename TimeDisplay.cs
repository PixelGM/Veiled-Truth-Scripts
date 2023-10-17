using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    private TextMesh textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMesh>();
    }

    private void Update()
    {
        // Get the current system time
        string time = System.DateTime.Now.ToString("h:mm:ss tt");

        // Update the text of the TextMesh component
        textMesh.text = time;
    }
}
