using System.Collections;
using System.Collections.Generic;
using GameStarter;
using TMPro;
using UnityEngine;

namespace GameStarter
{
    public class TestScore : MonoBehaviour
    {

        public TMP_Text ScoreText;
        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log("current game level: " + ScoreManager.Instance.CurrentLevel);
            //Debug.Log("unlocked game level: " + ScoreManager.Instance.UnlockedLevel);
            ScoreText.text = "Score: " + ScoreManager.Instance.Score;
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public void OnClickTestScore()
        {
            ScoreManager.Instance.AddScore(10);
            ScoreText.text = "Score: " + ScoreManager.Instance.Score;
        }
    }

}
