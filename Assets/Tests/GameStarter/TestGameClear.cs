using System.Collections;
using System.Collections.Generic;
using GameStarter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStarter
{
    public class TestGameClear : MonoBehaviour
    {
        public GameObject rewardScreen;

        public GameObject inGameScreen;


        public GameObject gameClearEffect;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickTestGameClear()
        {
            StateManager.Instance.State = GameState.Clear;
            ScoreManager.Instance.NextLevel();
            Debug.Log("test game clear");
            inGameScreen.SetActive(false);
            gameClearEffect.SetActive(true);
            StartCoroutine(ShowRewardAfter(2));
        }
        
        
        IEnumerator ShowRewardAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Debug.Log("Done " + Time.time);
            rewardScreen.SetActive(true);
            gameClearEffect.SetActive(false);
        }
        
         


    }
}

