using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    //Public variables
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public bool isGameActive;
    public bool bossExist = false;
    public bool bossDestroyed = true;
    public PlayerController playerControllerScript;
    public ObjectPooler objectPoolerScript;
    public EnemyController enemyControllerScript;
    public Button restartButton;
    public Button EasyButton;
    public Button MediumButton;
    public Button HardButton;
    public GameObject titleScreen;
    public GameObject playerMetrics;
    public GameObject bossPrefab;
    public GameObject enemyProjectilePrefab;
    public GameObject[] spawnPowerups;

    //Private variables
    private int score = 0;
    private float horizontalBound = 11.0f;
    private float topBound = 5.0f;
    private float bottomBound = -2.87f;
    private float enemyStartDelay = 1.0f;
    private float powerupStartDelay = 10.0f;
    private float hazardStartDelay = 1.5f;
    private float enemySpawnRate = 3.0f;
    private float powerupSpawnRate = 20.0f;
    private float hazardSpawnRate = 3.0f;
    private int nextBossThreshold = 100;


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        //Easy takes in 1, Medium takes in 2, Hard takes in 3 inside the button onClick event in the inspector
        enemySpawnRate /= difficulty;
        hazardSpawnRate /= difficulty;
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        UpdateScore(score);
        UpdateLives(playerControllerScript.lives);
        playerControllerScript.canMove = true;
        playerMetrics.gameObject.SetActive(true);
        StartSpawning();
        InvokeRepeating("SpawnRandomPowerup", powerupStartDelay, powerupSpawnRate);
    }

    public void Update()
    {
        
        if (score >= nextBossThreshold && bossDestroyed && !bossExist)
        {
            //bossDestroyed is set back to true upon boss destruction
            SpawnBossWave();
            bossExist = true;
            bossDestroyed = false;
            nextBossThreshold += nextBossThreshold;
        }
        
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        CancelInvoke();
    }

    public void UpdateScore( int pointValue)
    {
        score += pointValue;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + playerControllerScript.lives;
    }

    void SpawnRandomHazard()
    {
        //Checks if pooled object should be of type Asteroid
        GameObject pooledHazard = ObjectPooler.SharedInstance.GetPooledObject(objectPoolerScript.asteroidObjects[0]);
        if (pooledHazard != null)
        {
            pooledHazard.SetActive(true); // activate it
            pooledHazard.transform.position = new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55);
        }
    }

    void SpawnRandomPowerup()
    {
        GameObject powerup = spawnPowerups[Random.Range(0, spawnPowerups.Length)];
        Instantiate(powerup, new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55), powerup.transform.rotation);

    }

    void SpawnRandomEnemy()
    {
        //Checks if pooled object should be of type enemy
        GameObject pooledEnemy = ObjectPooler.SharedInstance.GetPooledObject(objectPoolerScript.enemyObjects[0]);
        if (pooledEnemy != null)
        {
            enemyControllerScript = pooledEnemy.GetComponent<EnemyController>();
            pooledEnemy.SetActive(true); // activate it
            //Call Start repeating when object is activated so the cancel on destruction in enemyController is overwritten when object is pooled again
            enemyControllerScript.StartRepeating();
            pooledEnemy.transform.position = new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55);
        }
    }

    void SpawnBossWave()
    {
        CancelInvoke("SpawnRandomEnemy");
        CancelInvoke("SpawnRandomHazard");
        Instantiate(bossPrefab, new Vector3(0, 0, 25), bossPrefab.transform.rotation);
        bossExist = true;
    }

    public void StartSpawning()
    {
        //Didn't add powerup to this since its never cancelled
        InvokeRepeating("SpawnRandomEnemy", enemyStartDelay, enemySpawnRate);
        InvokeRepeating("SpawnRandomHazard", hazardStartDelay, hazardSpawnRate);
    }

}
