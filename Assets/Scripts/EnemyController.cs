using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Vector3 offset = new Vector3(-0.030f, -0.21f, -1.29f);
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("fireProjectile", 0.0f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void fireProjectile()
    {   
            Instantiate(projectilePrefab, transform.position + offset, projectilePrefab.transform.rotation);  
    }
}
