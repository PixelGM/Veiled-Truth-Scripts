using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayLoopingAudioClip();
    }

    private void PlayLoopingAudioClip()
    {
        audioSource.loop = true;
        audioSource.Play();
    }
}
