using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Vector3 offset = new Vector3(-0.030f, -0.32f, -1.29f);
    public GameObject projectilePrefab;
    private GameManager gameManager;
    [SerializeField] private int pointValue = 10;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("fireProjectile", 0.0f, 1.5f);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void fireProjectile()
    {   
            Instantiate(projectilePrefab, transform.position + offset, projectilePrefab.transform.rotation);  
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
            gameManager.UpdateScore(pointValue);
        }
    }

    //TODO: Create a method for the enemy to follow the player as it approches them in the scene
}
