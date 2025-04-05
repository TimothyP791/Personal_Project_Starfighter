using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{

    // Public variables
    public AudioClip enemyShoot;
    public AudioClip enemyDeath;
    public ParticleSystem explosionParticle;
    public GameObject projectilePrefab;

    [SerializeField] private int pointValue = 10;

    // Private variables
    private Vector3 offset = new Vector3(-0.030f, -0.32f, -1.29f);
    private float frontBound = 60.0f;
    private float backBound = -10.0f;
    private GameManager gameManager;
    private bool wasHit = false;
    private AudioSource enemyAudio;
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyAudio = GetComponent<AudioSource>();
    }

     void Update()
    {
        if ((transform.position.z > frontBound || transform.position.z < backBound))
        {
            //changed from destroy for object pooling
            gameObject.SetActive(false);
            //Cancels invoke to prevent firing after enemy is destroyed
            CancelInvoke("fireEnemyProjectile");
        }
    }
    // Controls enemy fire functionality
    void fireEnemyProjectile()
    {
        GameObject pooledEnemyProjectile = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab);
        if (pooledEnemyProjectile != null)
        {
            pooledEnemyProjectile.SetActive(true);
            pooledEnemyProjectile.transform.position = transform.position + offset;
            pooledEnemyProjectile.transform.rotation = projectilePrefab.transform.rotation;
            enemyAudio.PlayOneShot(enemyShoot, 0.3f);
        }
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
        if (collision.gameObject.CompareTag("Projectile") && !wasHit)
        {
            wasHit = true;
            PlaySoundAndDestroy();
            collision.gameObject.SetActive(false);
            gameManager.UpdateScore(pointValue);
            StartCoroutine("CancelFireOnDestruction");
            StartCoroutine(ResetHitFlag());
        }
        //TODO: Figure out why projectile glitch still occurs when colliding with player
        else if (collision.gameObject.CompareTag("Player") && !wasHit)
        {
            wasHit = true;
            PlaySoundAndDestroy();
            StartCoroutine("CancelFireOnDestruction"); 
            StartCoroutine(ResetHitFlag());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shield") && !wasHit)
        {
            wasHit = true;
            PlaySoundAndDestroy();
            StartCoroutine("CancelFireOnDestruction");
            StartCoroutine(ResetHitFlag());
        }
    }

    IEnumerator CancelFireOnDestruction()
    {
        yield return new WaitForFixedUpdate();
        CancelInvoke("fireEnemyProjectile");
        gameObject.SetActive(false);
        
    }

    public void StartRepeating()
    {
        InvokeRepeating("fireEnemyProjectile", 0.0f, 1.5f); 
    }

    IEnumerator ResetHitFlag()
    {
        yield return new WaitForEndOfFrame();
        wasHit = false;
    }
}
