using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public GameObject gameObject;
    public float speed = 5f;
    private float frontBound = 60.0f;
    private float backBound = -10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Projectile"))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        if (gameObject.CompareTag("Enemy"))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        if (gameObject.CompareTag("Asteroid"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (transform.position.z > frontBound)
        {
            Destroy(gameObject);
        }
        if (transform.position.z < backBound)
        {
            Destroy(gameObject);
        }

    }
}
