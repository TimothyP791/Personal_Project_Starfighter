using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Add variables to control players movement speed
    public float speed = 10.0f;
    //public float turnSpeed = 50.0f;
    private float horizontalInput;
    private float verticalInput;
    private float horizontalBound = 8.5f;
    private float topBound = 4.3f;
    private float bottomBound = -1.8f;
    private Vector3 offset = new Vector3(0.07f, -0.54f, 1.37f);
    private float fireCooldown = 0.5f;
    private float lastFireTime = 0.0f;
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Call getaxis to get input from the players keyboard
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        //Add a rotation to the movement to make it look natural
        transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalInput);
        transform.Translate(Vector3.up * speed * Time.deltaTime * verticalInput);
        //transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        //Ensure the player can't leave the camera view - Potentially put this in another method for clean code
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
            fireProjectile();
        
    }

    //Create a method to fire a projectile
    void fireProjectile()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastFireTime + fireCooldown)
        {
            Instantiate(projectilePrefab, transform.position + offset, projectilePrefab.transform.rotation);
            lastFireTime = Time.time;
        }
    }
}
