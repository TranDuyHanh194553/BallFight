using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public static TimerScript instance;

    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;


    void Awake()
    {
        CreateInstance();
    }

    void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(TimerOn)
        {
            if(TimeLeft < 1)
            {

                TimeLeft = 0;
                TimerOn = false;
                InGameManager.instance.HandleEndGame();
                return;
            }


            //Debug.Log("TimeLeft:" + TimeLeft);
            TimeLeft -= Time.deltaTime;
            UpdateTimer(TimeLeft);
        }
    }


    void UpdateTimer(float currentTime)
    {
        float seconds = Mathf.FloorToInt(currentTime);
        //Debug.Log("seconds:" + seconds);
        TimerTxt.text = string.Format("{0} s", seconds);
    }

    public void LoadStageTimer(float seconds)
    {
        TimerTxt.text = string.Format("{0} s", seconds);
    }
}
