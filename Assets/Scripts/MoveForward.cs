using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 50.0f;
    private float frontBound = 60.0f;
    private float backBound = -10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ControlObjectMovement();
    }

    void ControlObjectMovement()
    {
        if (gameObject.CompareTag("Projectile") || gameObject.CompareTag("Enemy"))
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }
        if (gameObject.CompareTag("Asteroid") || gameObject.CompareTag("Rapid Fire") || gameObject.CompareTag("Life Up"))
        {
            transform.position += Vector3.back * Time.deltaTime * speed;
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
            // potentially add shield: Use System04 sound for shield powerup

        }
        //References objects that are not powerups
        if ((transform.position.z > frontBound || transform.position.z < backBound) && (!gameObject.CompareTag("Rapid Fire") || !gameObject.CompareTag("Life Up")))
        {
            //changed from destroy for object pooling
            gameObject.SetActive(false);
        }
        if ((transform.position.z > frontBound || transform.position.z < backBound) && (gameObject.CompareTag("Rapid Fire") || gameObject.CompareTag("Life Up")))
        {
            Destroy(gameObject);
        }
    }
}
