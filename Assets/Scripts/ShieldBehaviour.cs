using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBehaviour : MonoBehaviour
{
    public GameObject shield;
    private GameObject player;
    private Vector3 offset = new Vector3(0.0f, 0.23f, 0.1f); // Offset for shield position
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
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
