using UnityEngine;

public class ToggleColliderAndVisibility : MonoBehaviour
{
    public GameObject[] gameObjects;
    
    private Collider myCollider;
    private bool isColliderEnabled = false;  // Initial state is off
    private bool areObjectsVisible = true;   // Initial state is on

    void Start()
    {
        // Get the Collider component on this game object
        myCollider = GetComponent<Collider>();
        
        if (myCollider != null)
        {
            myCollider.enabled = isColliderEnabled;
        }
        else
        {
            Debug.LogError("No Collider component found on this game object.");
        }

        // Set the initial visibility of the GameObjects
        SetObjectsVisibility(areObjectsVisible);
    }

    void Update()
    {
        // Toggle the enabled state of the Collider and the visibility of the GameObjects when the 'F' key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            isColliderEnabled = !isColliderEnabled;
            
            if (myCollider != null)
            {
                myCollider.enabled = isColliderEnabled;
            }

            areObjectsVisible = !areObjectsVisible;
            SetObjectsVisibility(areObjectsVisible);
        }
    }

    private void SetObjectsVisibility(bool isVisible)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(isVisible);
            }
        }
    }
}