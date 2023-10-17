using UnityEngine;
using TMPro;
using System.Collections;

public class SidebarAdjust : MonoBehaviour
{
    public float raycastLength = 10f;
    public Vector3 raycastStartOffset = Vector3.zero;

    public GameObject toMove;
    public GameObject target;

    public float smoothTime = 0.3f;
    [Header("Title Text Settings")]
    public TextMeshPro titleText; // The TextMeshPro component for the title
    public float translationPerCharacter = 0.1124f;  // Amount to translate per character
    public float translationSpeed = 2f;  // Speed of the translation
    private Vector3 initialTitleTextPosition;
        
    public bool isSideLeft = false;

    private LineRenderer lineRenderer;
    private Vector3 originalLocalPosition;
    private Vector3 velocity = Vector3.zero;
    
    void Start()
    {
        originalLocalPosition = toMove.transform.localPosition;
        initialTitleTextPosition = titleText.transform.localPosition; // Added this line
        initialTitleTextPosition = titleText.transform.localPosition;
        
#if UNITY_EDITOR
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
#endif
    }

    void Update()
    {
        Vector3 rightDirection = transform.right;
        Vector3 raycastStartPos = transform.position + raycastStartOffset;
        RaycastHit hit;
        AdjustTitleTextPosition();

        if (Physics.Raycast(raycastStartPos, rightDirection, out hit, raycastLength))
        {
            Debug.Log("Hit: " + hit.transform.name);
            toMove.transform.position = Vector3.SmoothDamp(toMove.transform.position, target.transform.position, ref velocity, smoothTime);
            isSideLeft = true;

            AdjustTitleTextPosition(); // Added this line

#if UNITY_EDITOR
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, raycastStartPos);
            lineRenderer.SetPosition(1, hit.point);
#endif
        }
        else
        {
            toMove.transform.localPosition = Vector3.SmoothDamp(toMove.transform.localPosition, originalLocalPosition, ref velocity, smoothTime);
            isSideLeft = false;

#if UNITY_EDITOR
            lineRenderer.enabled = false;
#endif
        }
    }

    // if 10 chars -> translate (move)
    // if 12 chars or more -> DON'T translate (move)!
    void AdjustTitleTextPosition()
    {
        int characterCount = titleText.text.Length;
        Vector3 targetPosition = initialTitleTextPosition;

        // Check if character count is more than 10 and the sidebar is on the left
        if (characterCount > 10 && isSideLeft)
        {
            float translationAmount = (characterCount - 10) * translationPerCharacter;
            targetPosition += new Vector3(-translationAmount, 0, 0);
            StartCoroutine(SmoothlyTranslateTitleText(targetPosition));
        }
        else if (!isSideLeft) // If sidebar is not on the left, reset the title position
        {
            StartCoroutine(SmoothlyTranslateTitleText(initialTitleTextPosition));
        }
    }

    private IEnumerator SmoothlyTranslateTitleText(Vector3 targetPosition)
    {
        float journeyLength = Vector3.Distance(titleText.transform.localPosition, targetPosition);
        float startTime = Time.time;

        while (Vector3.Distance(titleText.transform.localPosition, targetPosition) > 0.05f)
        {
            float distanceCovered = (Time.time - startTime) * translationSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            titleText.transform.localPosition = Vector3.Lerp(titleText.transform.localPosition, targetPosition, fractionOfJourney);

            yield return null;
        }
    }
}
