using System.Collections;
using System.Collections.Generic;
using GameStarter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStarter
{
    public class TestGameOver : MonoBehaviour
    {
        public GameObject gameOverScreen;

        public GameObject inGameScreen;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnClickTestGameOver()
        {
            StateManager.Instance.State = GameState.GameOver;
            gameOverScreen.SetActive(true);
            inGameScreen.SetActive(false);
        
        }
        
        public void OnClickReplay()
        {
            StateManager.Instance.State = GameState.Playing;
            gameOverScreen.SetActive(false);
            inGameScreen.SetActive(true);
        }


    }
}

