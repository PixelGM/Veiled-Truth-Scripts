using UnityEngine;
using System.Collections;

public class CameraTransition : MonoBehaviour
{
    private Hotkeys hotkeys;
    
    public Animator animator;
    
    public Camera cameraA; // Third Person
    public Camera cameraB; // Phone
    public Camera cameraC; // First Person
    public GameObject character;
    public float transitionDuration = 2f;
    public Vector3 cameraBOffset;
    public Vector3 cameraCOffset;
    public Quaternion cameraBRotationOffset;
    public Quaternion cameraCRotationOffset;
    public float transitionCooldown = 0.5f;  // Cooldown period in seconds
    private float lastTransitionTime = 0f;  // Time when the last transition started
    private bool transitioning = false;

    private Transform characterTransform;
    private Transform cameraATransform;
    private Transform cameraBTransform;
    private Transform cameraCTransform;
    private Rigidbody characterRigidbody;

    void Start()
    {
        hotkeys = GameObject.FindObjectOfType<Hotkeys>();
        
        cameraA.enabled = true;
        cameraB.enabled = false;
        cameraC.enabled = false;
        characterTransform = character.transform;
        cameraATransform = cameraA.transform;
        cameraBTransform = cameraB.transform;
        cameraCTransform = cameraC.transform;
        characterRigidbody = character.GetComponent<Rigidbody>();
    
        // Set the last transition time to be a negative cooldown period ago
        lastTransitionTime = -transitionCooldown;
    }

    void Update()
    {
        bool isGrounded = animator.GetBool("IsGrounded");
           
        // Tracks Camera In Front of the Character (Remove If Statement for Debug)
        if (cameraA.enabled)
        {
            cameraBTransform.position = characterTransform.position + characterTransform.TransformDirection(cameraBOffset);
            cameraBTransform.rotation = characterTransform.rotation * cameraBRotationOffset;
        
            cameraCTransform.position = characterTransform.position + characterTransform.TransformDirection(cameraCOffset);
            cameraCTransform.rotation = characterTransform.rotation * cameraCRotationOffset;
        }
        
        // Ignore key inputs if a transition has just started
        if (Time.time - lastTransitionTime < transitionCooldown)
            return;
        
        // Phone Pause (Escape Key)
        // Escape!
        if (isGrounded && Input.GetKeyDown(hotkeys.phoneKey) && !transitioning)
        {
            if (cameraA.enabled) // Third Person
                StartCoroutine(Transition(cameraA, cameraB));
            else if (cameraB.enabled) // Phone
                StartCoroutine(Transition(cameraB, cameraA));
        }

        // First Person (F Key)
        if (isGrounded && Input.GetKeyDown(hotkeys.firstPerson) && !transitioning)
        {
            if (cameraA.enabled) // Third Person
                StartCoroutine(Transition(cameraA, cameraC));
            else if (cameraC.enabled) // First Person
                StartCoroutine(Transition(cameraC, cameraA));
        }
    }


    private IEnumerator Transition(Camera startCamera, Camera endCamera)
    {
        transitioning = true;
        float startTime = Time.time;
        Vector3 startPosition = startCamera.transform.position;
        Quaternion startRotation = startCamera.transform.rotation;
        Vector3 endPosition = endCamera.transform.position;
        Quaternion endRotation = endCamera.transform.rotation;
    
        // startCamera goes to endCamera (still using startCamera)
        while (Time.time < startTime + transitionDuration)
        {
            float t = (Time.time - startTime) / transitionDuration;
            t = t * t * (3f - 2f * t);  // This is a smoothstep interpolation
    
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
            Quaternion newRotation = Quaternion.Lerp(startRotation, endRotation, t);
    
            startCamera.transform.position = newPosition;
            startCamera.transform.rotation = newRotation;
            yield return null;
        }
    
        // Force the camera to be exactly at the end position and rotation
        startCamera.transform.position = endPosition;
        startCamera.transform.rotation = endRotation;
    
        // Switch Camera
        startCamera.enabled = false;
        endCamera.enabled = true;
        transitioning = false;
    }
}
