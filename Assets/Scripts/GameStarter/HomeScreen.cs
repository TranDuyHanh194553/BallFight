using System.Collections;
using System.Collections.Generic;
using GameStarter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStarter
{
    public class HomeScreen : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickPlay()
        {
            StateManager.Instance.State = GameState.Playing;
            SceneManager.LoadScene(GameScene.MainScene);
        }
    }
}