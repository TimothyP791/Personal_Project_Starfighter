using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Add variables to control players movement speed
    public float speed = 10.0f;
    //public float turnSpeed = 50.0f;
    public float fireRate = 0.5f;
    private bool canFire = true;
    private float horizontalInput;
    private float verticalInput;
    private float horizontalBound = 8.5f;
    private float topBound = 4.3f;
    private float bottomBound = -1.8f;
    // TODO: Apply Serialized Field to make the offset variable visible in the Unity Editor
    [SerializeField] private Vector3 offset = new Vector3(0.07f, -0.32f, 1.37f);
    public GameObject projectilePrefab;
    public int lives = 3;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
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
            gameManager.UpdateLives(lives);
            if (lives == 0)
            {
                Destroy(gameObject);
                gameManager.GameOver();
            }
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            lives--;
            Destroy(collision.gameObject);
            gameManager.UpdateLives(lives);
            if (lives == 0)
            {
                Destroy(gameObject);
                gameManager.GameOver();
            }
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
    void MovePlayer()
    {
        //Call getaxis to get input from the players keyboard
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //Add a rotation to the movement to make it look natural
        transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalInput);
        transform.Translate(Vector3.up * speed * Time.deltaTime * verticalInput);
        //transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
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
