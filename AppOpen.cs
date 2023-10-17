using UnityEngine;

public class AppOpen : MonoBehaviour
{
    public GameObject Application; // Reference to the Application GameObject

    void OnMouseDown()
    {
        // this.gameObject.SetActive(false); // Disable the current GameObject
        Application.SetActive(true); // Enable the Application GameObject
    }
}