using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Public variables
    public AudioClip bossShoot;
    public AudioClip bossDeath;
    public AudioClip bossHit;
    public ParticleSystem explosionParticle;
    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;
    public GameObject projectilePrefab3;
    public float speed = 5.0f; // Speed of the boss movement

    [SerializeField] private int pointValue = 35;

    // Private variables
    private Vector3 offset1 = new Vector3(0f, 0.14f, -3.22f);
    private Vector3 offset2 = new Vector3(1.16f, 0.33f, -2.81f);
    private Vector3 offset3 = new Vector3(-1.13f, -0.33f, -2.81f);
    private Vector3 direction;
    //TODO: Get bounds from game Manager functions instead.
    private float horizontalBound = 50.1f;
    private float topBound = 16.0f;
    private float bottomBound = -21.0f;
    [SerializeField] private int lives = 10;
    private GameManager gameManager;
    private bool wasHit = false;
    private AudioSource bossAudio;
    private GameObject player;
    private Rigidbody bossRb;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        bossAudio = GetComponent<AudioSource>();
        bossRb = GetComponent<Rigidbody>();
        StartRepeating();
    }

    void Update()
    {
        FollowPlayer();
        RestrictBoss();
    }
    // Controls enemy fire functionality
    void fireEnemyProjectile() //make virtual function
    {
        GameObject pooledEnemyProjectile = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab);

        if (pooledEnemyProjectile != null)
        {
            // Set the pooled projectile to active
            pooledEnemyProjectile.SetActive(true);
            // Set the position and rotation of the projectile
            pooledEnemyProjectile.transform.position = transform.position + offset1;
            pooledEnemyProjectile.transform.rotation = projectilePrefab.transform.rotation;
            bossAudio.PlayOneShot(bossShoot, 0.3f);
        }
        else
        {
            Debug.Log("No pooled object available");
            return;
        }
        
        GameObject pooledEnemyProjectile2 = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab2);

        if (pooledEnemyProjectile2 != null)
        {
            // Set the pooled projectile to active
            pooledEnemyProjectile2.SetActive(true);
            // Set the position and rotation of the projectile
            pooledEnemyProjectile2.transform.position = transform.position + offset1;
            pooledEnemyProjectile2.transform.rotation = projectilePrefab.transform.rotation;
            bossAudio.PlayOneShot(bossShoot, 0.3f);
        }
        else
        {
            Debug.Log("No pooled object available");
            return;
        }
        GameObject pooledEnemyProjectile3 = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab3);

        if (pooledEnemyProjectile3 != null)
        {
            // Set the pooled projectile to active
            pooledEnemyProjectile3.SetActive(true);
            // Set the position and rotation of the projectile
            pooledEnemyProjectile3.transform.position = transform.position + offset1;
            pooledEnemyProjectile3.transform.rotation = projectilePrefab.transform.rotation;
            bossAudio.PlayOneShot(bossShoot, 0.3f);
        }
        else
        {
            Debug.Log("No pooled object available");
            return;
        }
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            //Have the boss follow the player on X and Y axis
            direction = new Vector3((player.transform.position.x - transform.position.x), (player.transform.position.y - transform.position.y), 0f);
            direction.Normalize();
            bossRb.AddForce(direction * Time.deltaTime * speed, ForceMode.Impulse); // Adjust speed as needed
        }
    }
        void PlaySoundAndDestroy()
    {
        GameObject tempAudio = new GameObject("TempAudioSource"); // Create a temporary GameObject
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>(); // Add an AudioSource
        audioSource.clip = bossDeath;
        audioSource.volume = 30000.0f; // Set volume higher than 1.0 (adjust as needed)
        audioSource.spatialBlend = 0f; // Ensure it's a 2D sound if needed
        audioSource.Play();

        Destroy(tempAudio, bossDeath.length); // Destroy after the sound finishes

        if (explosionParticle != null)
        {
            ParticleSystem explosionEffect = Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); 
            explosionEffect.Play();
            Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !wasHit)
        {
            wasHit = true;
            collision.gameObject.SetActive(false);
            bossAudio.PlayOneShot(bossHit, 0.3f);
            lives--;
            StartCoroutine(ResetHitFlag());
            if (lives <= 0)
            {
                PlaySoundAndDestroy();
                gameManager.UpdateScore(pointValue);
                gameManager.StartRepeating();
                gameManager.bossExist = false;
                gameManager.bossDestroyed = true;
                StartCoroutine("CancelFireOnDestruction");
                //Restart enemies and asteroid spawning
            }
        }
    }

    void RestrictBoss()
    {
        //Ensure the player can't leave the camera view
        if (transform.position.x < -horizontalBound)
        {
            transform.position = new Vector3(-horizontalBound, transform.position.y, transform.position.z);
            bossRb.velocity = new Vector3(0, bossRb.velocity.y, bossRb.velocity.z);
        }
        if (transform.position.x > horizontalBound)
        {
            transform.position = new Vector3(horizontalBound, transform.position.y, transform.position.z);
            bossRb.velocity = new Vector3(0, bossRb.velocity.y, bossRb.velocity.z);
        }
        if (transform.position.y > topBound)
        {
            transform.position = new Vector3(transform.position.x, topBound, transform.position.z);
            bossRb.velocity = new Vector3(bossRb.velocity.x, 0, bossRb.velocity.z);
        }
        if (transform.position.y < bottomBound)
        {
            transform.position = new Vector3(transform.position.x, bottomBound, transform.position.z);
            bossRb.velocity = new Vector3(bossRb.velocity.x, 0, bossRb.velocity.z);
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
