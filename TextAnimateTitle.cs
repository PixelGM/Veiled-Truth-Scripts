using UnityEngine;
using TMPro;

public class TextAnimateTitle : MonoBehaviour
{
    public float animationSpeed = 0.1f;
    public string[] randomCharacters = { "!", "@", "#", "$", "%", "&", "*", "-", "+", "=" };

    private TextMeshPro textMeshPro;
    private string originalText;
    private string animatedText;
    private float timer;
    private int revealIndex;
    private bool isAnimating;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        originalText = textMeshPro.text;
        animatedText = GetRandomString(originalText.Length);
        revealIndex = 0;
        timer = 0f;
        isAnimating = true;
    }

    void OnEnable()
    {
        if (!isAnimating)
        {
            revealIndex = 0;
            animatedText = GetRandomString(originalText.Length);
            isAnimating = true;
        }
    }

    void OnDisable()
    {
        isAnimating = false;
    }

    void Update()
    {
        if (!isAnimating)
            return;

        timer += Time.deltaTime;

        if (timer >= animationSpeed)
        {
            timer = 0f;
            AnimateText();
        }
    }

    void AnimateText()
    {
        animatedText = animatedText.Remove(revealIndex, 1).Insert(revealIndex, originalText[revealIndex].ToString());
        textMeshPro.text = animatedText;

        revealIndex++;

        if (revealIndex > originalText.Length)
        {
            isAnimating = false;
        }
    }

    string GetRandomString(int length)
    {
        string randomString = "";

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, randomCharacters.Length);
            randomString += randomCharacters[randomIndex];
        }

        return randomString;
    }
}
