using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public variables
    public float moveForce = 300.0f;
    public float turnSpeed = 50.0f;
    public float fireRate = 0.5f;
    public int lives = 3;
    public bool canMove = false;
    public GameObject projectilePrefab;
    public GameManager gameManager;
    public AudioClip hitSound;
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip deathOther;
    public ParticleSystem explosionParticle;

    [SerializeField] private Vector3 offset = new Vector3(0.07f, -0.32f, 1.37f);

    //Private variables
    private float horizontalInput;
    private float verticalInput;
    private float horizontalBound = 11.14f;
    private float topBound = 5.59f;
    private float bottomBound = -2.87f;
    private bool canFire = true;
    //private Vector3 currentPosition;
    private Rigidbody playerRb;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            MovePlayer();
        }
        ManageLives();
        RestrictPlayer();
        FireProjectile();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Asteroid"))
        {
            lives--;
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(deathOther, 0.8f);
            gameManager.UpdateLives(lives);
            if (lives == 0)
            {
                PlaySoundAndDestroy();
                Destroy(gameObject);
                gameManager.GameOver();
            }
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            lives--;
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(hitSound, 0.8f);
            gameManager.UpdateLives(lives);
            if (lives == 0)
            {
                PlaySoundAndDestroy();
                Destroy(gameObject);
                gameManager.GameOver();
            }
        }
    }

    void PlaySoundAndDestroy()
    {
        GameObject tempAudio = new GameObject("TempAudioSource"); // Create a temporary GameObject
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>(); // Add an AudioSource
        audioSource.clip = deathSound;
        audioSource.volume = 10000.0f; // Set volume higher than 1.0 (adjust as needed)
        audioSource.spatialBlend = 0f; // Ensure it's a 2D sound if needed
        audioSource.Play();

        Destroy(tempAudio, deathSound.length); // Destroy after the sound finishes

        if (explosionParticle != null)
        {
            ParticleSystem explosionEffect = Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation); //Could replace transform.rotation with Quaternion.identity
            explosionEffect.Play();
            Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Life up"))
        {
            lives++;
            Destroy(other.gameObject);
            gameManager.UpdateLives(lives);
        }
        if (other.gameObject.CompareTag("Rapid fire"))
        {
            fireRate = 0.25f;
            Destroy(other.gameObject);
            StartCoroutine(RapidFireCountdownRoutine());
        }
    }

    //TODO: Utilize restrict player to stop adding force when player reaches the bounds of the screen
    void MovePlayer()
    {
        //Get the current rotation of the player
        Vector3 currentRotation = transform.eulerAngles;
        //Call getaxis to get input from the players keyboard
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //Add a rotation to the movement to make it look natural
        playerRb.AddForce(Vector3.right * moveForce * Time.deltaTime * horizontalInput);
        playerRb.AddForce(Vector3.up * moveForce * Time.deltaTime * verticalInput);
        transform.Rotate(Vector3.back * turnSpeed * horizontalInput * Time.deltaTime);

        //Ensure the player loses force applied in boundary direction so that they can move back in bounds quickly
        if (transform.position.x <= -horizontalBound)
        {
            playerRb.velocity = Vector3.zero;
        }
        else if (transform.position.x >= horizontalBound)
        {
            playerRb.velocity = Vector3.zero;
        }
        else if (transform.position.y >= topBound)
        {
            playerRb.velocity = Vector3.zero;
        }
        else if (transform.position.y <= bottomBound)
        {
            playerRb.velocity = Vector3.zero;
        }


        //Unity stores rotations form 0 - 360, so -30 becomes 330
        //To fix this we convert values greater than 180 to negative values using statement below
        float zRotation = (currentRotation.z > 180) ? currentRotation.z - 360 : currentRotation.z;

        //Now zRotation will be between -180 and 180
        if (zRotation > 25)
        {
            transform.rotation = Quaternion.Euler(0, 0, 25);
        }
        if (zRotation < -25)
        {
            transform.rotation = Quaternion.Euler(0, 0, -25);
        }
    }

    void RestrictPlayer()
    {
        //Ensure the player can't leave the camera view
        if (transform.position.x < -horizontalBound)
        {
            transform.position = new Vector3(-horizontalBound, transform.position.y, transform.position.z);
        }
        if (transform.position.x > horizontalBound)
        {
            transform.position = new Vector3(horizontalBound, transform.position.y, transform.position.z);
        }
        if (transform.position.y > topBound)
        {
            transform.position = new Vector3(transform.position.x, topBound, transform.position.z);
        }
        if (transform.position.y < bottomBound)
        {
            transform.position = new Vector3(transform.position.x, bottomBound, transform.position.z);
        }
    }
    //Create a method to fire a projectile
    void FireProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canFire)
        {
            Instantiate(projectilePrefab, transform.position + offset, projectilePrefab.transform.rotation);
            playerAudio.PlayOneShot(shootSound, 0.3f);
            StartCoroutine(FireProjectileCountdownRoutine());
        }
    }

    IEnumerator FireProjectileCountdownRoutine()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    IEnumerator RapidFireCountdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        fireRate = 0.5f;
    }

    void ManageLives()
    {
        if (lives > 3)
        {
            lives = 3;
        }
    }
}
