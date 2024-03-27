using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
//    private float spawnRate = 1.0f;
//    public List<GameObject> targets;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private Button restartButton;

    private int score;
    public bool isGameActive;
//    public GameObject titleScreen;
//    public GameObject[] targets2;

    void Start()
    {

    }
    public void StartGame(/*int difficulty*/)
    {   
        isGameActive = true;
//        StartCoroutine(SpawnTarget());
        score = 0;
//        scoreText.text = "Score: " + score;
        UpdateScore(0);
//        spawnRate /= difficulty;
//        titleScreen.gameObject.SetActive(false);
    }
        public void UpdateScore(int scoreToAdd){
            score += scoreToAdd;
            scoreText.text = "Score: " + score; 
        }
        public void GameOver(){
            gameOverText.gameObject.SetActive(true);
            victoryText.gameObject.SetActive(false);
            isGameActive = false;
            restartButton.gameObject.SetActive(true);
            return;
        }

        public void Victory(){
            victoryText.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(false);
            isGameActive = false;
            restartButton.gameObject.SetActive(true);
            return;
        }

        public void RestartGame(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
}
