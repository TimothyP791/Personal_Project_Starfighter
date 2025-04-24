using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Public Variables
    public static ObjectPooler SharedInstance;
    public List<GameObject> pooledProjectile;
    public List<GameObject> pooledEnemyProjectile;
    public List<GameObject> enemyObjects;
    public List<GameObject> asteroidObjects;
    public GameObject objectToPool;
    public GameObject enemyObjectToPool;
    public GameObject[] enemyPool;
    public GameObject[] asteroidPool;
    // All data below is defined in the GameManager script, but is public here so that it can be set in the inspector
    public int amountToPoolPlayer;
    public int amountToPoolEnemy;
    public int enemyAmountToPool;
    public int asteroidAmountToPool;

    // Private Variables

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        pooledProjectile = new List<GameObject>();
        for (int i = 0; i < amountToPoolPlayer; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledProjectile.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Game Manager
        }
        enemyObjects = new List<GameObject>();
        for (int i = 0; i < enemyAmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(enemyPool[Random.Range(0,enemyPool.Length)]);
            obj.SetActive(false);
            enemyObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Game Manager
        }
        asteroidObjects = new List<GameObject>();
        for (int i = 0; i < asteroidAmountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(asteroidPool[Random.Range(0, asteroidPool.Length)]);
            obj.SetActive(false);
            asteroidObjects.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Game Manager
        }
        pooledEnemyProjectile = new List<GameObject>();
        for (int i = 0; i < amountToPoolEnemy; i++)
        {
            GameObject obj = (GameObject)Instantiate(enemyObjectToPool);
            obj.SetActive(false);
            pooledEnemyProjectile.Add(obj);
            obj.transform.SetParent(this.transform); // set as children of Game Manager
        }
    }

    public GameObject GetPooledObject(GameObject poolType)
    {
        if (poolType.CompareTag("Projectile"))
        {
            for (int i = 0; i < pooledProjectile.Count; i++)
            {
                // For as many objects as are in the pooledObjects list
                // if the pooled objects is NOT active, return that object 
                if (!pooledProjectile[i].activeInHierarchy)
                {
                    return pooledProjectile[i];
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

        if (poolType.CompareTag("Enemy Projectile"))
        {
            for (int i = 0; i < pooledEnemyProjectile.Count; i++)
            {
                if (!pooledEnemyProjectile[i].activeInHierarchy)
                {
                    return pooledEnemyProjectile[i];
                }
            }
        }
        // otherwise, return null   
        return null;
    }

}
