using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Vector3 offset = new Vector3(-0.030f, -0.32f, -1.29f);
    public GameObject projectilePrefab;
    private GameManager gameManager;
    [SerializeField] private int pointValue = 10;
    private AudioSource enemyAudio;
    public AudioClip enemyShoot;
    public AudioClip enemyDeath;
    public ParticleSystem explosionParticle;
    private bool isDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("fireProjectile", 0.0f, 1.5f);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyAudio = GetComponent<AudioSource>();
    }

    void fireProjectile()
    {
        //Add audio clip to the projectile
        Instantiate(projectilePrefab, transform.position + offset, projectilePrefab.transform.rotation);
        enemyAudio.PlayOneShot(enemyShoot, 0.3f);
    }

    void PlaySoundAndDestroy()
    {
        GameObject tempAudio = new GameObject("TempAudioSource"); // Create a temporary GameObject
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>(); // Add an AudioSource
        audioSource.clip = enemyDeath;
        audioSource.volume = 30000.0f; // Set volume higher than 1.0 (adjust as needed)
        audioSource.spatialBlend = 0f; // Ensure it's a 2D sound if needed
        audioSource.Play();

        Destroy(tempAudio, enemyDeath.length); // Destroy after the sound finishes

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
                StartCoroutine(DeactivateAfterPhysicsFrame());
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDestroyed)
            {
                isDestroyed = true;
                PlaySoundAndDestroy();
                StartCoroutine(DeactivateAfterPhysicsFrame());
            }
        }
    }

    IEnumerator DeactivateAfterPhysicsFrame()
    {
        yield return new WaitForFixedUpdate();
        gameObject.SetActive(false);
        //TODO: Find a way to cancel and restart invoke repeating;
        //CancelInvoke();
    }
}
