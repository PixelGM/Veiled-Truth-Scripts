using UnityEngine;

public class ClickPhoneDisable : MonoBehaviour
{
    public GameObject Application; // Reference to the Application GameObject

    void OnMouseDown()
    {
        // this.gameObject.SetActive(false); // Disable the current GameObject
        Application.SetActive(false); // Disable the Application GameObject
    }
}