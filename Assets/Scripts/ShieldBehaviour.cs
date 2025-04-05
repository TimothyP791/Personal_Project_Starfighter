using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    public GameObject shield;
    
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy Projectile") || other.gameObject.CompareTag("Asteroid") || other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            shield.SetActive(false);
        }
    }
}
