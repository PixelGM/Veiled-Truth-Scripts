using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public GameObject[] interactableObjects;
    public float interactionDistance = 1f;  // The distance within which the player can interact with an object

    private GameObject interactableObject;

    void Update()
    {
        // Get the nearest interactable object within the interaction distance
        interactableObject = GetNearestInteractableObject();

        if (interactableObject != null && Input.GetKeyDown(KeyCode.E))
        {
            interactableObject.SetActive(false);
            interactableObject = null;
        }
    }

    private GameObject GetNearestInteractableObject()
    {
        GameObject nearestObject = null;
        float minDistance = interactionDistance;

        foreach (GameObject obj in interactableObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance <= minDistance)
            {
                minDistance = distance;
                nearestObject = obj;
            }
        }

        return nearestObject;
    }
}
