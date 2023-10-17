using UnityEngine;
using UnityEngine.UI;

public class BrightnessControl : MonoBehaviour
{
    public Image targetImage;
    public Slider brightnessSlider;

    private Color originalColor;

    void Start()
    {
        // Store the original color of the image
        originalColor = targetImage.color;

        // Set the slider values to range from 0 to 2
        brightnessSlider.minValue = 0.3f;
        brightnessSlider.maxValue = 1;

        // Set the initial slider value to 1 (original image brightness)
        brightnessSlider.value = 1;

        // Add a listener to the slider
        brightnessSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        // Apply brightness adjustment
        Color adjustedColor = new Color(originalColor.r * value, originalColor.g * value, originalColor.b * value, originalColor.a);
        targetImage.color = adjustedColor;
    }
}