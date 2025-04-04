using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{

    private Vector3 offset = new Vector3(-0.030f, -0.32f, -1.29f);
    private float frontBound = 60.0f;
    private float backBound = -10.0f;
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
        //TODO: Add object pooling for enemy projectiles
        GameObject pooledEnemyProjectile = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab);
        if (pooledEnemyProjectile != null)
        {
            pooledEnemyProjectile.SetActive(true);
            pooledEnemyProjectile.transform.position = transform.position + offset;
            pooledEnemyProjectile.transform.rotation = projectilePrefab.transform.rotation;

            /*Rigidbody rb = pooledEnemyProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                // use up vector since object is on its side
                rb.AddForce(transform.up * 20.0f, .Impulse);
            }*/
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
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (!isDestroyed)
            {
                isDestroyed = true;
                PlaySoundAndDestroy();
                collision.gameObject.SetActive(false);
                gameManager.UpdateScore(pointValue);
                StartCoroutine("CancelFireOnDestruction");
            }
        }
        //TODO: Figure out why projectile glitch still occurs when colliding with player
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDestroyed)
            {
                isDestroyed = true;
                PlaySoundAndDestroy();
                StartCoroutine("CancelFireOnDestruction");
            }
        }
    }
    //TODO: add condition for shield collision to play a sound

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
}
