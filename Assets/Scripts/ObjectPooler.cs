using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledObjects;
    public List<GameObject> enemyObjects;
    public List<GameObject> asteroidObjects;
    public List<GameObject> powerupObjects;
    public GameObject objectToPool;
    public GameObject[] enemyPool;
    public GameObject[] asteroidPool;
    public int amountToPool;
    public int enemyAmountToPool;
    public int asteroidAmountToPool;


    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
        enemyObjects = new List<GameObject>();
        for (int i = 0; i < enemyAmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(enemyPool[Random.Range(0,enemyPool.Length)]);
            obj.SetActive(false);
            enemyObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
        asteroidObjects = new List<GameObject>();
        for (int i = 0; i < asteroidAmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(asteroidPool[Random.Range(0, asteroidPool.Length)]);
            obj.SetActive(false);
            asteroidObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Spawn Manager
        }
    }

    public GameObject GetPooledObject(GameObject poolType)
    {
        if (poolType.CompareTag("Projectile"))
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                // For as many objects as are in the pooledObjects list
                // if the pooled objects is NOT active, return that object 
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
        }
        if (poolType.CompareTag("Enemy"))
        {
            for (int i = 0; i < enemyObjects.Count; i++)
            {
                // if the pooled objects is NOT active, return that object 
                if (!enemyObjects[i].activeInHierarchy)
                {
                    return enemyObjects[i];
                }
            }
        }

        if (poolType.CompareTag("Asteroid"))
        {
            for (int i = 0; i < asteroidObjects.Count; i++)
            {
                // if the pooled objects is NOT active, return that object 
                if (!asteroidObjects[i].activeInHierarchy)
                {
                    return asteroidObjects[i];
                }
            }
        }
        // otherwise, return null   
        return null;
    }

}
