using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // Public variables
    public float speed;

    // Private variables
    private int startPoz = 100;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (transform.position.z < 60)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPoz);
        }
    }
}
