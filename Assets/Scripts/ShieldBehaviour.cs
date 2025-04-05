using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    // Public variables
    public GameObject shield;
    public AudioClip shieldOff;

    // Private variables


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy Projectile") || other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("Enemy"))
        {
            PlaySoundAndDestroy();
            other.gameObject.SetActive(false);
            shield.SetActive(false);
        }
    }

    void PlaySoundAndDestroy()
    {
        GameObject tempAudio = new GameObject("TempAudioSource"); // Create a temporary GameObject
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>(); // Add an AudioSource
        audioSource.clip = shieldOff;
        audioSource.volume = 30000.0f; // Set volume higher than 1.0 (adjust as needed)
        audioSource.spatialBlend = 0f; // Ensure it's a 2D sound if needed
        audioSource.Play();

        Destroy(tempAudio, shieldOff.length); // Destroy after the sound finishes
    }
}
