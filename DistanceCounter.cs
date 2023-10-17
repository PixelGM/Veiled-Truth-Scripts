using System.Collections;
using UnityEngine;
using TMPro;

public class DistanceCounter : MonoBehaviour
{
    public Transform playerTransform;
    public Transform targetTransform;
    public TextMeshProUGUI distanceText;

    void Update()
    {
        float distance = Vector3.Distance(playerTransform.position, targetTransform.position);
        distanceText.text = "Distance: " + distance.ToString("F2");
    }
}