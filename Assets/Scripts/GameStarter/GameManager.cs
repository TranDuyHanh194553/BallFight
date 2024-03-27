using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStarter
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
    
        private void Awake()
        {
            // clear old data
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene(GameScene.MainScene);

        }

        // Update is called once per frame
        void Update()
        {
            
        }

    }

 

}
