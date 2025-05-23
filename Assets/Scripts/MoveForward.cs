using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Public variables
    public float speed = 5f;
    public float rotateSpeed = 50.0f;

    // Private variables
    private float frontBound = 60.0f;
    private float backBound = -10.0f;

    // Update is called once per frame
    void Update()
    {
        ControlObjectMovement();
    }

    void ControlObjectMovement()
    {
        if (gameObject.CompareTag("Projectile") || gameObject.CompareTag("Enemy") || gameObject.CompareTag("Enemy Projectile"))
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }
        if (gameObject.CompareTag("Asteroid") || 
            gameObject.CompareTag("Rapid Fire") || 
            gameObject.CompareTag("Life Up") || 
            gameObject.CompareTag("Shield Up"))
        {
            //Keeps movement in the same forward direction
            transform.position += Vector3.back * Time.deltaTime * speed;

            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);

        }
        //References objects that are not powerups
        if ((transform.position.z > frontBound || transform.position.z < backBound) && 
            (gameObject.CompareTag("Projectile") || gameObject.CompareTag("Enemy Projectile") || gameObject.CompareTag("Asteroid")))
        {
            //changed from destroy for object pooling Enemies are handled in enemy controller for Invoke canceling reasons
            gameObject.SetActive(false);
        }

        if ((transform.position.z > frontBound || transform.position.z < backBound) && (gameObject.CompareTag("Rapid Fire") || gameObject.CompareTag("Life Up")))
        {
            Destroy(gameObject);
        }
    }
}
