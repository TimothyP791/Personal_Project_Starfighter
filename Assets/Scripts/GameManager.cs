using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawnEnemy;
    public GameObject[] spawnHazards;
    public GameObject[] spawnPowerups;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    private int score = 0;
    public bool isGameActive;
    public PlayerController playerControllerScript;
    public Button restartButton;
    public Button StartButton;
    public GameObject titleScreen;
    public GameObject playerMetrics;

    //TODO: Apply Serialized Field to make the horizontalBound, topBound, and bottomBound variables visible in the Unity Editor when testing camera offset
    private float horizontalBound = 11.0f;
    private float topBound = 5.0f;
    private float bottomBound = -2.87f;
    private float enemyStartDelay = 1.0f;
    private float powerupStartDelay = 10.0f;
    private float hazardStartDelay = 1.5f;
    private float enemySpawnRate = 2.5f;
    private float powerupSpawnRate = 20.0f;
    private float hazardSpawnRate = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
        GameObject hazard = spawnHazards[Random.Range(0, spawnHazards.Length)];
        Instantiate(hazard, new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55), hazard.transform.rotation);
    }

    void SpawnRandomPowerup()
    {
        GameObject powerup = spawnPowerups[Random.Range(0, spawnPowerups.Length)];
        Instantiate(powerup, new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55), powerup.transform.rotation);
    }

    void SpawnRandomEnemy()
    {
        GameObject enemy = spawnEnemy[Random.Range(0, spawnEnemy.Length)];
        Instantiate(enemy, new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(bottomBound, topBound), 55), enemy.transform.rotation);
    }
}
