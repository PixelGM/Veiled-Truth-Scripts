using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSystem : MonoBehaviour
{
    public Transform cover; // Reference to the cover (cube)
    public float coverDistanceThreshold = 1f; // Distance threshold to decide whether player is in cover
    public float movementThreshold = 0.1f; // Threshold to consider the player as not moving

    private Animator animator; // Reference to the player's animator
    private float playerHeight; // Player's height
    private Rigidbody playerRigidbody; // Player's Rigidbody

    void Start()
    {
        // Get the Animator component from the player
        animator = GetComponent<Animator>();
        
        // Get the Rigidbody component from the player
        playerRigidbody = GetComponent<Rigidbody>();

        // Calculate player's height
        playerHeight = GetComponent<Collider>().bounds.size.y;
    }

    void Update()
    {
        // Calculate the direction from player to cover
        Vector3 toCover = cover.position - transform.position;
        toCover.y = 0; // Ignore height difference

        // Calculate the distance to cover
        float distanceToCover = toCover.magnitude;

        // Normalize the toCover vector (make its length 1) for accurate dot product calculation
        toCover.Normalize();

        // Calculate the dot product of the player's forward direction and the direction to the cover
        float dotProduct = Vector3.Dot(transform.forward, toCover);

        // Get the cube's height
        float coverHeight = cover.GetComponent<Collider>().bounds.size.y;

        // Check if the player's velocity is close to zero (not moving)
        bool isPlayerNotMoving = playerRigidbody.velocity.magnitude < movementThreshold;

        // If the distance is less than the threshold, player's height is less than or equal to the cube's height,
        // player is not moving, and player is facing the cover, set IsCover to true, else set IsCover to false
        if(distanceToCover <= coverDistanceThreshold && playerHeight <= coverHeight && isPlayerNotMoving && dotProduct > 0)
        {
            animator.SetBool("IsCover", true);
            Debug.Log("Player is in front of the cover.");
        }
        else
        {
            animator.SetBool("IsCover", false);
            Debug.Log("Player is not in front of the cover.");
        }
    }
}
