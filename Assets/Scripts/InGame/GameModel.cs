using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    public int Score { get; private set; }
    public int LastHighScore { get; private set; }

    #region Phase variables
    TurnPhaseEnum currentPhase = TurnPhaseEnum.LOADING;

    float timeUnderCurrentPhase = 0;

    public TurnPhaseEnum CurrentPhase
    {
        get { return currentPhase; }
        set { currentPhase = value; }
    }

    public System.Action<TurnPhaseEnum, TurnPhaseEnum> OnNewPhaseActivatedEvent;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScore()
    {
        Score++;
    }

    IEnumerator IEChangePhase(TurnPhaseEnum newPhase)
    {
        timeUnderCurrentPhase = 0;
        TurnPhaseEnum previousPhase = CurrentPhase;
        CurrentPhase = newPhase;
        switch (newPhase)
        {
            case TurnPhaseEnum.LOADING:
                Debug.Log("GameService Loading...");
                break;
            case TurnPhaseEnum.ENDGAME:
                Debug.Log("GameService endgame");
                EndGame();
                break;
            case TurnPhaseEnum.ENGAME_SCORE_PROCESSED:
                Debug.Log("Load current rank");
                break;

        }

        if(OnNewPhaseActivatedEvent != null)
        {
            OnNewPhaseActivatedEvent(newPhase, previousPhase);
        }
        yield break;
    }


    void EndGame()
    {
    }
}

public enum TurnPhaseEnum
{
    NONE = -1,
    LOADING = 0,
    DEPLOY = 1,
    WAIT = 2,
    PLAYING = 4,
    ENDGAME = 6,
    STARTGAME = 8,
    REPORTING_HIGHSCORE,
    ENGAME_SCORE_PROCESSED,
}
