using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float speed;
    private int startPoz = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
