using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : EnemyController
{
    // Public variables
    public AudioClip bossShoot;
    public AudioClip bossHit;
    public GameObject projectilePrefab2; // Need 2 and 3 to differentiate from inherited projectilePrefab to shoot 3 projectiles
    public GameObject projectilePrefab3;
    public float speed = 5.0f; // Speed of the boss movement

    [SerializeField] private int pointValueBoss = 35;

    // Private variables
    private Vector3 offset1 = new Vector3(0f, 0.14f, -3.22f);
    private Vector3 offset2 = new Vector3(1.16f, -0.33f, -2.81f);
    private Vector3 offset3 = new Vector3(-1.13f, -0.33f, -2.81f);
    private Vector3 direction;
    private float horizontalBound = 50.1f;
    private float topBound = 16.0f;
    private float bottomBound = -21.0f;
    [SerializeField] private int lives = 10;
    private GameManager gameManager;
    //private bool wasHit = false;
    private AudioSource bossAudio;
    private GameObject player;
    private Rigidbody bossRb;


    // Start is called before the first frame update
    public override void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        bossAudio = GetComponent<AudioSource>();
        bossRb = GetComponent<Rigidbody>();
        StartRepeating();
    }

    public override void Update()
    {
        FollowPlayer();
        RestrictBoss();
    }
    // Controls enemy fire functionality
    public override void fireEnemyProjectile() //make virtual function
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
        }
        
        GameObject pooledEnemyProjectile2 = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab2);

        if (pooledEnemyProjectile2 != null)
        {
            // Set the pooled projectile to active
            pooledEnemyProjectile2.SetActive(true);
            // Set the position and rotation of the projectile
            pooledEnemyProjectile2.transform.position = transform.position + offset2;
            pooledEnemyProjectile2.transform.rotation = projectilePrefab.transform.rotation;
            bossAudio.PlayOneShot(bossShoot, 0.3f);
        }
        else
        {
            Debug.Log("No pooled object available");
        }
        GameObject pooledEnemyProjectile3 = ObjectPooler.SharedInstance.GetPooledObject(projectilePrefab3);

        if (pooledEnemyProjectile3 != null)
        {
            // Set the pooled projectile to active
            pooledEnemyProjectile3.SetActive(true);
            // Set the position and rotation of the projectile
            pooledEnemyProjectile3.transform.position = transform.position + offset3;
            pooledEnemyProjectile3.transform.rotation = projectilePrefab.transform.rotation;
            bossAudio.PlayOneShot(bossShoot, 0.3f);
        }
        else
        {
            Debug.Log("No pooled object available");
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

    public override void OnCollisionEnter(Collision collision)
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
                gameManager.UpdateScore(pointValueBoss);
                gameManager.StartSpawning();
                gameManager.bossExist = false;
                gameManager.bossDestroyed = true;
                StartCoroutine("CancelFireOnDestruction");
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
}
