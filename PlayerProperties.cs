using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerProperties : MonoBehaviour
{
    private Hotkeys hotkeys;
    
    // #Headers
    [Header("Phone")]
    public AudioSource audioSource;
    public AudioClip phoneActiveClip;
    public AudioClip phoneInactiveClip;
    public GameObject phoneObject;
    public GameObject menu;
    public Animator boxAnimator;
    public GameObject UI;
    public vThirdPersonInput VThirdPersonInput;
    public Animator animator;
    public PostProcessVolume postProcessVolume;
    
    public float colorTransitionSpeed = 1f;  // the speed of color transition, you can adjust this
    private Vignette vignette;
    private Color originalVignetteColor;
    private Color targetVignetteColor;
    

    [Header("Footstep Sounds")]
    public AudioSource footstepAudioSource;
    [SerializeField] private float raycastDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;

    [Header("Location")]
    public GameObject locationFloor;
    public TextMeshPro roomText;
    
    [Header("Sidebar Close Right Animation Check")]
    public SidebarAdjust sidebarAdjust; // Make sure to set this reference in the inspector to your SidebarAdjust object
    public Animator sideAnimator; // Assign your Animator in the inspector
    private static readonly int IsSidebarLeft = Animator.StringToHash("IsSidebarLeft");
    
    [Header("Soundtrack")]
    public AudioSource soundtrackAudioSource;
    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    private bool isSoundtrackPlaying = false;
    private float soundtrackVolume;
    
    [Header("Transparency Settings")]
    public Material[] transparencyMaterials;
    public float minTransparencyDistance = 3f; // Minimum distance for alpha transition to start
    public float maxTransparencyDistance = 5f; // Maximum distance for alpha transition to end
    public float smoothFactor = 5f;
    
    private bool isPhoneActive = false;
    private bool isMenuActive = false;
    private Vector3 raycastDirection;
    private float escapeCooldown = 0.3f;
    private float menuCooldown = 0.3f;
    private float lastEscapePressTime;
    private float lastMenuPressTime;
    private bool isFKeyPressed = false;
    private Dictionary<GameObject, string> floorNames = new Dictionary<GameObject, string>();

    private static readonly int IsPhone = Animator.StringToHash("IsPhone");
    private static readonly int IsMenu = Animator.StringToHash("IsMenu");

    

    private void Awake()
        {
            // Cache component references
            hotkeys = FindObjectOfType<Hotkeys>();
            sidebarAdjust = GetComponent<SidebarAdjust>();
        }
    
        void Start()
        {
            InitializeSettings();
            PopulateFloorNames();
            soundtrackVolume = soundtrackAudioSource.volume;
            soundtrackAudioSource.volume = 0;
    
            if (postProcessVolume.profile.TryGetSettings<Vignette>(out var vign))
            {
                vignette = vign;
                originalVignetteColor = vign.color.value;
            }
            targetVignetteColor = Color.white;  // start with white color
        }
    
       void Update()
        {
            bool isGrounded = animator.GetBool("IsGrounded");
    
            if (isGrounded)
            {
                CheckForPhoneActivation();
                CheckForMenuActivation();
    
                if (Input.GetKeyDown(hotkeys.firstPerson))
                {
                    ToggleSoundtrack();
                }
            }
    
            AdjustMaterialTransparency();
            sideAnimator.SetBool(IsSidebarLeft, sidebarAdjust.isSideLeft);
            if (vignette != null)
            {
                vignette.color.value = Color.Lerp(vignette.color.value, targetVignetteColor, Time.deltaTime * colorTransitionSpeed);
            }
        }
    
        private void ToggleSoundtrack()
        {
            isFKeyPressed = !isFKeyPressed;
    
            if (isSoundtrackPlaying)
            {
                StopCoroutine(FadeIn());
                StartCoroutine(FadeOut());
            }
            else
            {
                StopCoroutine(FadeOut());
                StartCoroutine(FadeIn());
            }
    
            isSoundtrackPlaying = !isSoundtrackPlaying;
        }
    
    private IEnumerator FadeIn()
    {
        soundtrackAudioSource.Stop();  
        soundtrackAudioSource.Play();
        float t = 0;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            soundtrackAudioSource.volume = Mathf.Lerp(0, soundtrackVolume, t / fadeInTime);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            soundtrackAudioSource.volume = Mathf.Lerp(soundtrackVolume, 0, t / fadeOutTime);
            yield return null;
        }

        soundtrackAudioSource.Stop();  
    }

    void InitializeSettings()
    {
        lastMenuPressTime = -menuCooldown;
        phoneObject.SetActive(false);
        menu.SetActive(false);
        UI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        raycastDirection = Vector3.down * raycastDistance;
    }

    void PopulateFloorNames()
    {
        int floorCount = locationFloor.transform.childCount;
        for (int i = 0; i < floorCount; i++)
        {
            Transform floorTransform = locationFloor.transform.GetChild(i);
            floorNames.Add(floorTransform.gameObject, floorTransform.name);
        }
    }

    void CheckForPhoneActivation()
    {
        // Escape!
        if (Input.GetKeyDown(hotkeys.phoneKey) && Time.time >= lastEscapePressTime + escapeCooldown && !isFKeyPressed)  // Modify this line
        {
            lastEscapePressTime = Time.time;
            isPhoneActive = !isPhoneActive;
            TogglePhoneActivation(isPhoneActive);
            
            // Motion Blur Turn On Whe Transitioning (if isPhoneActive == true)
            if (postProcessVolume.profile.TryGetSettings<MotionBlur>(out var motionBlur))
            {
                motionBlur.enabled.value = isPhoneActive;
            }
            
            if (isPhoneActive)
            {
                targetVignetteColor = originalVignetteColor;
            }
            else
            {
                targetVignetteColor = Color.white;
            }
        }
    }

    void CheckForMenuActivation()
    {
        if (Input.GetKeyDown(hotkeys.sideKey) && Time.time >= lastMenuPressTime + menuCooldown)
        {
            lastMenuPressTime = Time.time;
            isMenuActive = !isMenuActive;
            ToggleMenuActivation(isMenuActive);
        }
    }

    void TogglePhoneActivation(bool isActive)
    {
        animator.SetBool(IsPhone, isActive);
        VThirdPersonInput.isPhoneActive = isActive;

        if (isActive)
        {
            VThirdPersonInput.cc.ResetVelocity();
            phoneObject.SetActive(true);
            UI.SetActive(true);
            audioSource.clip = phoneActiveClip;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            phoneObject.SetActive(false);
            UI.SetActive(false);
            audioSource.clip = phoneInactiveClip;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        audioSource.Play();
    }

    void ToggleMenuActivation(bool isActive)
    {
        boxAnimator.SetBool(IsMenu, isActive);
        if (isActive)
        {
            menu.SetActive(true);
        }
        else
        {
            StartCoroutine(DeactivateMenuAfterDelay(0.5f));
        }
    }
    
    void AdjustMaterialTransparency()
    {
        foreach (var material in transparencyMaterials)
        {
            float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

            // Get current color
            Color color = material.GetColor("_Color");

            if (distanceToCamera <= minTransparencyDistance)
            {
                // If we are within the minTransparencyDistance, ensure object is fully transparent
                color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * smoothFactor);
            }
            else if (distanceToCamera > minTransparencyDistance && distanceToCamera < maxTransparencyDistance)
            {
                // Calculate alpha based on the distance
                float alpha = ((distanceToCamera - minTransparencyDistance) / (maxTransparencyDistance - minTransparencyDistance));

                // Set the alpha value
                color.a = Mathf.Lerp(color.a, alpha, Time.deltaTime * smoothFactor);
            }
            else
            {
                // If we are beyond maxTransparencyDistance, ensure object is fully opaque
                color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * smoothFactor);
            }

            material.SetColor("_Color", color);
        }
    }



    private IEnumerator DeactivateMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(!isMenuActive) menu.SetActive(false);
    }

    private void PlayFootstepSound()
    {
        if (Physics.Raycast(transform.position, raycastDirection, raycastDistance, groundLayer))
        {
            footstepAudioSource.pitch = Random.Range(minPitch, maxPitch);
            footstepAudioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the collided GameObject is in the dictionary
        if (floorNames.TryGetValue(collision.gameObject, out string floorName))
        {
            // Set the 3D text to the name of the GameObject player is on
            roomText.text = floorName;
        }
    }
}
