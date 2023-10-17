using UnityEngine;
using System.Collections;

public class AppClose : MonoBehaviour
{
    public GameObject Application; // Reference to the Application GameObject
    public Animator animator;  // The Animator component on the GameObject
    private int isClosedHash;  // The hash for the "IsClosed" parameter
    public float delay = 0.25f; // Delay in seconds before disabling the animator

    void Start()
    {
        // Calculate the hash for the "IsClosed" parameter
        isClosedHash = Animator.StringToHash("IsClosed");
    }

    void OnMouseDown()
    {
        // Play the animation and then deactivate the GameObject
        StartCoroutine(PlayAnimationThenDeactivateCoroutine());
    }

    private IEnumerator PlayAnimationThenDeactivateCoroutine()
    {
        // Set the "IsClosed" parameter to true to start the animation
        animator.SetBool(isClosedHash, true);
        Debug.Log("Playing animation: IsClosed");

        // // Wait until the animation is done
        // while (animator.GetBool(isClosedHash))
        // {
        //     yield return null;
        // }

        Debug.Log("Animation complete: IsClosed");

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Now deactivate the Application GameObject
        Application.SetActive(false);
        Debug.Log("Deactivated the Application GameObject");
    }
}