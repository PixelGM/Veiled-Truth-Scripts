using System.Collections;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public ParticleSystem rainParticleSystem;
    public float fadeDuration = 1f;

    public bool isRaining = true;

    void Start()
    {
        StartRain();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isRaining)
            {
                StopRain();
            }
            else
            {
                StartRain();
            }
        }
    }

    public void StartRain()
    {
        StartCoroutine(FadeRain(6000f));
        isRaining = true;
    }

    public void StopRain()
    {
        StartCoroutine(FadeRain(0f));
        isRaining = false;
    }

    IEnumerator FadeRain(float endRate)
    {
        var emission = rainParticleSystem.emission;
        var startRate = emission.rateOverTime.constant;
        var elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            var newRate = Mathf.Lerp(startRate, endRate, elapsedTime / fadeDuration);
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);
            yield return null;
        }

        emission.rateOverTime = new ParticleSystem.MinMaxCurve(endRate);
    }
}
