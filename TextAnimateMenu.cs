using UnityEngine;
using TMPro;

public class TextAnimateMenu : MonoBehaviour
{
    public float revealSpeed = 0.1f;
    public string[] randomCharacters = { "!", "@", "#", "$", "%", "&", "*", "-", "+", "=" };

    private TextMeshPro textMeshPro;
    private string originalText;
    private string animatedText;
    private int currentIndex;
    private float timer;
    private bool isAnimating;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        originalText = textMeshPro.text;
        animatedText = "";
        currentIndex = 0;
        timer = 0f;
        isAnimating = true;
    }

    void OnEnable()
    {
        if (!isAnimating)
        {
            currentIndex = 0;
            animatedText = "";
            isAnimating = true;
        }
    }

    void OnDisable()
    {
        isAnimating = false;
    }

    void Update()
    {
        if (!isAnimating || currentIndex >= originalText.Length)
            return;

        timer += Time.deltaTime;

        if (timer >= revealSpeed)
        {
            timer = 0f;
            AnimateText();
        }
    }

    void AnimateText()
    {
        currentIndex++;
        animatedText = originalText.Substring(0, currentIndex);

        if (currentIndex < originalText.Length)
        {
            int randomIndex = Random.Range(0, randomCharacters.Length);
            animatedText += randomCharacters[randomIndex];
        }

        textMeshPro.text = animatedText;
    }
}
