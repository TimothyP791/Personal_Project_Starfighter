using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnHazards;
    public GameObject[] spawnPowerups;
    // Use player restriction values to set spawn bounds
    private float horizontalBound = 8.5f;
    private float topBound = 4.3f;
    private float bottomBound = -1.8f;
    private float hazardStartDelay = 1.0f;
    private float powerupStartDelay = 10.0f;
    private float hazardSpawnRate = 2.5f;
    private float powerupSpawnRate = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Call the SpawnRandomItem function every 1 second using invoke repeating
        InvokeRepeating("SpawnRandomHazard", hazardStartDelay, hazardSpawnRate);
        InvokeRepeating("SpawnRandomPowerup", powerupStartDelay, powerupSpawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomHazard()
    {
        GameObject hazard = spawnHazards[Random.Range(0, spawnHazards.Length)];
        /*if (hazard.CompareTag("Enemy"))
        {
            Instantiate(hazard, new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55), hazard.transform.rotation);
        }
        else*/
            Instantiate(hazard, new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound,topBound), 55), hazard.transform.rotation);
    }
    void SpawnRandomPowerup()
    {
        GameObject powerup = spawnPowerups[Random.Range(0, spawnPowerups.Length)];
        Instantiate(powerup, new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55), powerup.transform.rotation);
    }
}
