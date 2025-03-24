using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private int pointValue = 5;
    public AudioClip explosion;
    public ParticleSystem explosionParticle;
    private bool isDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaySoundAndDestroy()
    {
        GameObject tempAudio = new GameObject("TempAudioSource"); // Create a temporary GameObject
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>(); // Add an AudioSource
        audioSource.clip = explosion;
        audioSource.volume = 30000.0f; // Set volume higher than 1.0 (adjust as needed)
        audioSource.spatialBlend = 0f; // Ensure it's a 2D sound if needed
        audioSource.Play();
        Destroy(tempAudio, explosion.length); // Destroy after the sound finishes

        //Separate instantiation of explosion particle so it plays after asteroid is destroyed
        if (explosionParticle != null)
        {
            ParticleSystem explosionEffect = Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); //Could replace transform.rotation with Quaternion.identity
            explosionEffect.Play();
            Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            
            if (!isDestroyed)
            {
                isDestroyed = true;
                PlaySoundAndDestroy();
                collision.gameObject.SetActive(false);
                gameManager.UpdateScore(pointValue);
                StartCoroutine(DestroyAfterPhysicsFrame());
            }
        }
    }
    // Use wait for fixed update to destroy the asteroid after the physics frame so that Unity can reconcile with the physics engine
    IEnumerator DestroyAfterPhysicsFrame()
    {
        yield return new WaitForFixedUpdate();
        gameObject.SetActive(false);
    }
}
