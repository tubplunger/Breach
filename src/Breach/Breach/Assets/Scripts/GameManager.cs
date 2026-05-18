using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject winUI;
    public GameObject loseUI;

    [Header("Enemies")]
    public int enemiesRemaining;

    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void OnEnable()
    {
        GameEvents.OnHealthDied += HandleHealthDied;
    }

    void OnDisable()
    {
        GameEvents.OnHealthDied -= HandleHealthDied;
    }

    void Update()
    {
        if (!gameEnded) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    void HandleHealthDied(Health health)
    {
        if (health.CompareTag("Player"))
        {
            PlayerDied();
        }
        else if (health.CompareTag("Enemy"))
        {
            EnemyDied();
        }
    }

    public void PlayerDied()
    {
        if (gameEnded) return;

        gameEnded = true;

        if (loseUI != null)
        {
            loseUI.SetActive(true);
        }

        EndGameState();
    }

    public void EnemyDied()
    {
        if (gameEnded) return;

        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            gameEnded = true;

            if (winUI != null)
            {
                winUI.SetActive(true);
            }

            EndGameState();
        }
    }

    void EndGameState()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
