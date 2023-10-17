using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiClippingOnPause : MonoBehaviour
{
    private Hotkeys hotkeys;
    
    public float detectDistance = 1.0f;
    public float rotationSpeed = 10.0f;
    public float raycastAngle = 45.0f;  // Angle of the raycast relative to the forward direction, in degrees

    private bool isScriptEnabled = false;  // Initial state is off

    void Start()
    {
        hotkeys = GameObject.FindObjectOfType<Hotkeys>();
    }
    
    void Update()
    {
        // Toggle the script state when the escape key is pressed
        // Escape!
        if (Input.GetKeyDown(hotkeys.phoneKey))
        {
            isScriptEnabled = !isScriptEnabled;
        }

        if (!isScriptEnabled)
        {
            return;  // Skip the rest of the Update method if the script is disabled
        }

        // Rotate the forward direction vector by 'raycastAngle' degrees around the y-axis
        Vector3 raycastDirection = Quaternion.Euler(0, raycastAngle, 0) * transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, raycastDirection, out hit, detectDistance))
        {
            Debug.Log("Something");

            // Define the target rotation as a rotation 90 degrees around the y-axis
            Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}