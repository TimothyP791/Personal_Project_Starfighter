using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    private int score = 0;
    public bool isGameActive;
    public PlayerController playerControllerScript;
    public ObjectPooler objectPoolerScript;
    public Button restartButton;
    public Button StartButton;
    public GameObject titleScreen;
    public GameObject playerMetrics;
    public GameObject enemyProjectilePrefab;
    public GameObject[] spawnPowerups;
    //Revert back to enemyController if necessary
    public AudioClip enemyShoot;

    //Revert back to enemyController if necessary
    private Vector3 offset = new Vector3(-0.030f, -0.32f, -1.29f);
    private float horizontalBound = 11.0f;
    private float topBound = 5.0f;
    private float bottomBound = -2.87f;
    private float enemyStartDelay = 1.0f;
    private float powerupStartDelay = 10.0f;
    private float hazardStartDelay = 1.5f;
    private float enemySpawnRate = 2.5f;
    private float powerupSpawnRate = 20.0f;
    private float hazardSpawnRate = 2.5f;
    private AudioSource enemyAudio;
    //Remove if necessary
    private GameObject[] enemy;

    void Update()
    {
        enemyAudio = GetComponent<AudioSource>();
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        UpdateScore(score);
        UpdateLives(playerControllerScript.lives);
        playerControllerScript.canMove = true;
        playerMetrics.gameObject.SetActive(true);
        InvokeRepeating("SpawnRandomEnemy", enemyStartDelay, enemySpawnRate);
        InvokeRepeating("SpawnRandomHazard", hazardStartDelay, hazardSpawnRate);
        InvokeRepeating("SpawnRandomPowerup", powerupStartDelay, powerupSpawnRate);
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
            pooledEnemy.SetActive(true); // activate it
            pooledEnemy.transform.position = new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55);
            StartRepeating();
        }
    }
    //Figure out way to have all enemies fire at once
    void fireEnemyProjectile()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].activeInHierarchy)
            {
                Instantiate(enemyProjectilePrefab, enemy[i].transform.position + offset, enemyProjectilePrefab.transform.rotation);
                enemyAudio.PlayOneShot(enemyShoot, 0.3f);
            }
        }   
    }

    IEnumerator CancelFireOnDestruction()
    {
        CancelInvoke("fireProjectile");
        yield return new WaitForSeconds(10.0f);
        Debug.Log("Fire Cancelled");
        StartRepeating();
    }

    private void StartRepeating()
    {
        InvokeRepeating("fireEnemyProjectile", 0.0f, 1.5f);
    }
}
