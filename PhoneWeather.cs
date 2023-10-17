using TMPro;
using UnityEngine;

public class PhoneWeather : MonoBehaviour
{
    [Header("Weather App")]
    public GameObject currentWeather;
    //public string rainMessage;
    //public string sunnyMessage;

    public Animator cloudPhone;
    public ParticleSystem heavyRain;

    private float rainEmission;
    
    void Start()
    {
        // Cloud Logo Rain Besides Phone Animation
        // Set IsRain to false on start
        cloudPhone.SetBool("IsRain", false);
    }

    void Update()
    {
        // Cloud Logo Rain Besides Phone Animation
        // Check if the rainParticles are active
        if (heavyRain.emission.rateOverTime.constant > 0f)
        {
            // Set IsRain to true if rainParticles are active
            cloudPhone.SetBool("IsRain", true);

            // Set the TextMeshPro text to the value of the public string
            currentWeather.SetActive(true); // On Rain Logo
        }
        else
        {
            // Set IsRain to false if rainParticles are inactive
            cloudPhone.SetBool("IsRain", false);
            
            // Clear the TextMeshPro text
            currentWeather.SetActive(false); // Off Rain Logo
        }
    }
}