using System.Collections;
using UnityEngine;
using TMPro;

public class FrameTimeCounter : MonoBehaviour
{
    public TextMeshProUGUI frameTimeText;
    public float updateInterval = 0.5f;

    private float timeSinceLastUpdate = 0.0f;
    private int frameCount = 0;

    void Update()
    {
        frameCount++;
        timeSinceLastUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastUpdate > updateInterval)
        {
            float frameTime = Time.deltaTime * 1000; // frame time in milliseconds
            frameTimeText.text = "Frame Time: " + frameTime.ToString("F2") + " ms";
            frameCount = 0;
            timeSinceLastUpdate -= updateInterval;
        }
    }
}