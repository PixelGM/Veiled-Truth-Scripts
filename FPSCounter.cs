using System.Collections;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public float updateInterval = 0.5f;

    private float fps;
    private float timeSinceLastUpdate = 0.0f;
    private int frameCount = 0;

    void Update()
    {
        frameCount++;
        timeSinceLastUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastUpdate > updateInterval)
        {
            fps = frameCount / timeSinceLastUpdate;
            fpsText.text = "FPS: " + fps.ToString("F2");

            frameCount = 0;
            timeSinceLastUpdate -= updateInterval;
        }
    }
}