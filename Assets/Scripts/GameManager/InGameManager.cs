using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using GameStarter;
using TMPro;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    public LevelManager levelManager;

    public GameObject HomeScreen;
    public GameObject GameOverScreen;
    public GameObject RewardScreen;
    public GameObject GameClearEffect;

    public Transform inforObject;


    public List<GameObject> pooledTargetInforObjects;
    public List<GameObject> pooledBarrierObjects;
    public List<GameObject> pooledTargetObjects;
    public List<GameObject> pooledBonusCoinObjects;
    public List<GameObject> pooledEnemyObjects;


    // su dung de ve vị trí của enemy 
    public List<Vector3> listTargetPos;

    public GameObject targetInforPrefab;
    public GameObject targetPrefab;
    public GameObject coinPrefab;
    public GameObject ballInstance;
    public GameObject enemyPrefab;

    [SerializeField]
    private GameObject ballPrefab;

    public int totalTargetInStage;

    public List<GameObject> listBarrierObjects;

    private Vector3 MinSpawnTargetPos = new Vector3(0f, 2.0f, -1.19f);
    private Vector3 MaxSpawnTargetPos = new Vector3(3f, 3.13f, 2.31f);

    private float zMinSpawnEnemyPos = -2.60f;
    private float zMaxSpawnEnemyPos = -2.19f;

    private Vector3 MinSpawnBarrierPos = new Vector3(-0.86f, 0.02f, -4.52f);
    private Vector3 MaxSpawnBarrierPos = new Vector3(1.42f, 0.02f, -3.60f);


    private LevelData levelData;
    private StageData stageData;


    public int avaiableBall;
    public bool isUnLimitBall = false;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI numberBallText;
    public TextMeshProUGUI levelText;


    //audio clip
    public AudioClip shootSound;
    public AudioClip hitTargetSound;
    public AudioClip hitBarrierSound;
    public AudioClip getCoinSound;
    public AudioClip gameOverSound;
    public AudioClip nextStageSound;
    public AudioClip btnClick;


    public AudioSource playerAudio;

    public bool isHitTarget = false;


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
        playerAudio = GetComponent<AudioSource>();
        //CreateBall();
        LoadDataLevelGame();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void RemoveAllOldObject()
    {

        Debug.Log("RemoveAllOldObject pooledBarrierObjects:" + pooledBarrierObjects.Count);
        if (pooledBarrierObjects.Count > 0)
        {
            Debug.Log("barrier:" + pooledBarrierObjects[0]);
            foreach (GameObject barrier in pooledBarrierObjects)
            {
                Debug.Log("barrier:" + barrier);
                Destroy(barrier.gameObject);
            }

            pooledBarrierObjects.Clear();

        }


        if (pooledTargetObjects.Count > 0)
        {
            foreach (GameObject target in pooledTargetObjects)
            {
                Destroy(target.gameObject);
            }

            pooledTargetObjects.Clear();

        }


        if (pooledTargetInforObjects.Count > 0)
        {
            foreach (GameObject targetInfor in pooledTargetInforObjects)
            {
                Destroy(targetInfor.gameObject);
            }

            pooledTargetInforObjects.Clear();
        }

        if(pooledBonusCoinObjects.Count > 0)
        {
            foreach (GameObject coin in pooledBonusCoinObjects)
            {
                Destroy(coin.gameObject);
            }

            pooledBonusCoinObjects.Clear();
        }


        if (pooledEnemyObjects.Count > 0)
        {
            foreach (GameObject enemy in pooledEnemyObjects)
            {
                Destroy(enemy.gameObject);
            }

            pooledEnemyObjects.Clear();
        }

    }

    
    public void LoadDataLevelGame()
    {

        // show home screen 
        showObject(HomeScreen);
        StateManager.Instance.State = GameState.Loading;


        // remove all old object
        RemoveAllOldObject();

        int levelID = ScoreManager.Instance.CurrentLevel;
        int stageID = ScoreManager.Instance.CurrentStage;

        Debug.Log("LoadDataLevelGame - levelID:" + levelID);
        Debug.Log("LoadDataLevelGame - stageID:" + stageID);

        levelData = levelManager.GetLevelData(levelID);
        stageData = levelManager.GetStageData(levelData, stageID);

        Debug.Log("LoadDataLevelGame - targets:" + stageData.targets);
        Debug.Log("LoadDataLevelGame - objects:" + stageData.objects);


        if (levelData != null && stageData != null)
        {
            // render game infor: timer, ball avaiable, remainTarget
            LoadInforScene(levelID + "." + stageID);
            
            // render list target with level, stage
            totalTargetInStage = stageData.targets;
            SpawnListTargetRandomPosition(stageData.targets, stageData.bonusCoins);
            SpawnListBarrierRandomPosition(stageData.objects);
            SpawnListEnemyRandomPosition(stageData.enemyRobots);

            // reset timer
            TimerScript.instance.LoadStageTimer(stageData.stageTime);

        }
    }



    private void showObject(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void hideObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }


    void loadOldScore()
    {
        int lastHighScore = ScoreManager.Instance.Score;
        scoreText.text = "" + lastHighScore;
    }

    void UpdateScore(int score)
    {
        Debug.Log("Score:" + ScoreManager.Instance.Score);
        int lastHighScore = ScoreManager.Instance.Score + score;

        // update highScore
        ScoreManager.Instance.AddScore(score);

        scoreText.text = "" + lastHighScore;
    }


    public void LoadAvaiableBall()
    {
        if (stageData != null)
        {
            string numberBall = stageData.balls;
            numberBallText.text = "x " + numberBall;

            if (numberBall == Common.UNLIMIT_AVAIABLE)
            {
                isUnLimitBall = true;
                return;
            }

            isUnLimitBall = false;
            avaiableBall = int.Parse(stageData.balls);
        }

    }

    public void DecreaseNumberBall()
    {
        if (isHitTarget)
        {
            isHitTarget = false;
            return;
        }

        if(isUnLimitBall)
        {
            numberBallText.text = "x " + Common.UNLIMIT_AVAIABLE;
            return;
        }

        if (avaiableBall > 0)
        {
            avaiableBall--;
            numberBallText.text = "x " + avaiableBall;
        }
   
    }

    public void UpdateLevelStage(string level)
    {
        levelText.text = "Level " + level;
    }


    public void HandleEndGame()
    {
        Debug.Log("HandleEndGame:" + avaiableBall + "-" + TimerScript.instance.TimeLeft);
        int remainTarget = pooledTargetInforObjects.Count;
        Debug.Log("remainTarget:" + remainTarget);

        if (StateManager.Instance.State != GameState.Playing)
        {
            return;
        }


        if ((avaiableBall == 0 && !isUnLimitBall) || TimerScript.instance.TimeLeft == 0 )
        {
            // Neu van con target chua chung thi show game over screen
            if (remainTarget != 0)
            {
                LoadGameOverScene();
                return;
            }

            // load clear scene
            LoadClearScene();

        }


        // nếu stage 1.4, 2.4 ko gioi hạn so luong bong ma sut trung het thi show thanh 
        if (isUnLimitBall && remainTarget == 0)
        {
            // load clear scene
            LoadClearScene();
            return;
        }
    }


    public void LoadInforScene(string level)
    {

        inforObject.gameObject.SetActive(true);


        // start timer neu game trang thai playing 
        if (StateManager.Instance.State == GameState.Playing)
        {
            StartTimer();
        }

        // draw list remain target infor
        SpawnListTargetInfor(stageData.targets);

        // load remain ball
        LoadAvaiableBall();

        // load old score 
        loadOldScore();

        // update current level text
        UpdateLevelStage(level);
    }

    void StartTimer()
    {
        // start timer 
        TimerScript.instance.TimerOn = true;
        TimerScript.instance.TimeLeft = stageData.stageTime;
    }

    public void LoadGameOverScene()
    {
        Debug.Log("show game over");
        StateManager.Instance.State = GameState.GameOver;
        TimerScript.instance.TimerOn = false;

        // play sound game over
        playSound(Common.SoundState.GameOver);
        showObject(GameOverScreen);
    }


    public void LoadRewardScene()
    {
        Debug.Log("show reward");
        showObject(RewardScreen);
        // next stage and level
        ScoreManager.Instance.NextStage();
    }


    public void LoadClearScene()
    {
        // play sound show effect
        playSound(Common.SoundState.EffectNext);

        StateManager.Instance.State = GameState.Clear;
        TimerScript.instance.TimerOn = false;

        UpdateScore(stageData.reward);
        showObject(GameClearEffect);
        StartCoroutine(ShowRewardAfter(2));
    }


    IEnumerator ShowRewardAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("Done " + Time.time);
        LoadRewardScene();
        hideObject(GameClearEffect);
    }

    public void HandleRetryStage()
    {
        playSound(Common.SoundState.ButtonClick);
        hideObject(GameOverScreen);

        // retry van giu nguyen man choi hien tai, reset so qua bong, reset timer

        // show home screen 
        showObject(HomeScreen);
        StateManager.Instance.State = GameState.Loading;

        // reset ball
        LoadAvaiableBall();

        // reset timer
        TimerScript.instance.LoadStageTimer(stageData.stageTime);


    }

    public void HandleNoThank()
    {
        playSound(Common.SoundState.ButtonClick);
        hideObject(GameOverScreen);
        // reset old data
        ScoreManager.Instance.ResetStage();

        LoadDataLevelGame();

    }


    public void HandleNextStage()
    {
        playSound(Common.SoundState.ButtonClick);
        hideObject(RewardScreen);
        LoadDataLevelGame();

        // reset ball position
        BallControl.BallScript.share.reset(false);
    }


    public void HandlePlayStage()
    {
        playSound(Common.SoundState.ButtonClick);
        hideObject(HomeScreen);
        StateManager.Instance.State = GameState.Playing;
        StartTimer();
    }


    public void AddBonusScore()
    {
        UpdateScore(1);
    }


    public void DecreaseTargetHit()
    {
        isHitTarget = true;
        Debug.Log("pooledTargetObjects:" + pooledTargetInforObjects.Count);
        if (pooledTargetInforObjects.Count > 0)
        {
            GameObject lastTarget = pooledTargetInforObjects[pooledTargetInforObjects.Count - 1];
            Debug.Log("lastTarget:" + lastTarget);
            Destroy(lastTarget.gameObject);
            pooledTargetInforObjects.RemoveAt(pooledTargetInforObjects.Count - 1);
        }

        // handle show reward screen
        if (pooledTargetInforObjects.Count == 0 && TimerScript.instance.TimeLeft > 0)
        {
            LoadClearScene();
        }
    }


    /*
     *  Play sound effect
     */
    public void playSound(Common.SoundState soundStage)
    {
        switch (soundStage)
        {
            case Common.SoundState.ShootBall:
                playerAudio.PlayOneShot(shootSound, 2.0f);
                break;
            case Common.SoundState.HitTarget:
                playerAudio.PlayOneShot(hitTargetSound, 1.0f);
                break;
            case Common.SoundState.GameOver:
                playerAudio.PlayOneShot(gameOverSound, 2.0f);
                break;
            case Common.SoundState.GetCoin:
                playerAudio.PlayOneShot(getCoinSound, 2.0f);
                break;
            case Common.SoundState.EffectNext:
                playerAudio.PlayOneShot(nextStageSound, 2.0f);
                break;
            case Common.SoundState.ButtonClick:
                playerAudio.PlayOneShot(btnClick, 2.0f);
                break;
            case Common.SoundState.HitBarrier:
                playerAudio.PlayOneShot(hitBarrierSound, 1.0f);
                break;

        }
    }


    /**
     *  Handle Spawn object in game : ball, target, barrier, coin, game information
     */

    // Create new ball
    void CreateBall()
    {
        Debug.Log("ballPrefab:" + ballPrefab.transform.position);
        ballInstance = Instantiate(ballPrefab, ballPrefab.transform.position, Quaternion.identity);
    }


    /* ===================================== START CREATE TARGET INFOR  ===============================*/

    public void SpawnListTargetInfor(int numberTargets)
    {
        for (int i = 0; i <= numberTargets - 1; i++)
        {
            Vector3 spawnPos = new Vector3(targetInforPrefab.gameObject.transform.position.x + i * 80, targetInforPrefab.gameObject.transform.position.y, targetInforPrefab.gameObject.transform.position.z);
            StartCoroutine(SpawnTargetInfor(spawnPos));
        }

    }


    IEnumerator SpawnTargetInfor(Vector3 spawnPos)
    {

        GameObject obj = Instantiate(targetInforPrefab, spawnPos, targetInforPrefab.transform.rotation, inforObject);
        obj.gameObject.SetActive(true);
        pooledTargetInforObjects.Add(obj);
        Debug.Log("List Target Infor:" + pooledTargetInforObjects.Count);
        yield return 0f;
    }


    /* ===================================== END CREATE TARGET INFOR  ===============================*/


    /* ===================================== START CREATE LIST ENEMY IN GAME  ===============================*/
    public void SpawnListEnemyRandomPosition(int numberEnemys)
    {

        // get min, max pos of all target
        Vector3 minTargetPos = Helper.GetMinPosition(listTargetPos);
        Vector3 maxTargetPos = Helper.GetMaxPosition(listTargetPos);


        pooledEnemyObjects = RandomPointsGenerator.GenerateObjectsByPrefab(enemyPrefab, numberEnemys, new Vector3(minTargetPos.x + 1.0f, minTargetPos.y, zMinSpawnEnemyPos),
            new Vector3(maxTargetPos.x - 1.0f, maxTargetPos.y, zMaxSpawnEnemyPos));


        for (int i = 0; i < pooledEnemyObjects.Count; i++)
        {
            EnemyScript enemyScript = pooledEnemyObjects[i].GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.enablePingPong = true;
            }
        }
    }


    /* ===================================== END CREATE LIST ENEMY IN GAME  ===============================*/


    /* ===================================== START CREATE LIST TARGET IN GAME  ===============================*/

    public void SpawnListTargetRandomPosition(int numberTargets, int numberCoins)
    {
        int numberCoinWithTarget = numberCoins / numberTargets;
        Debug.Log("numberCoinWithTarget:" + numberCoinWithTarget);

        Vector3 EasyMinSpawnTargetPos = new Vector3(MinSpawnTargetPos.x, MinSpawnTargetPos.y, MinSpawnTargetPos.z - 2f);
        Vector3 EasyMaxSpawnTargetPos = new Vector3(MaxSpawnTargetPos.x, MaxSpawnTargetPos.y - 2f, MaxSpawnTargetPos.z - 2f);

        bool isEasyLevel = false;

        if(levelData.levelID == 1 && (stageData.stageID == 1 || stageData.stageID == 2))
        {
            isEasyLevel = true;
        }

        pooledTargetObjects = RandomPointsGenerator.GenerateObjectsByPrefab(targetPrefab, numberTargets, isEasyLevel ? EasyMinSpawnTargetPos : MinSpawnTargetPos,
            isEasyLevel ? EasyMaxSpawnTargetPos : MaxSpawnTargetPos);

        for (int i = 0; i < pooledTargetObjects.Count; i++)
        {
            TargetScript targetScript = pooledTargetObjects[i].GetComponent<TargetScript>();
            if (targetScript != null && numberCoins == 0)
            {
                targetScript.enablePingPong = true;
            }

            // add list pos of target to draw enemy 
            listTargetPos.Add(pooledTargetObjects[i].transform.position);


            // draw list bonus coin 
            if (numberCoinWithTarget > 0)
            {
                StartCoroutine(SpawnCoin(numberCoinWithTarget, pooledTargetObjects[i].transform.position));
            }
        }
    }

    IEnumerator SpawnCoin(int numberCoins, Vector3 targetPos)
    {

        // check if stage have bonus coin
        for (int i = 0; i <= numberCoins - 1; i++)
        {
            StartCoroutine(SpawnBonusCoin(targetPos, i + 1));
        }
        yield return 0f;
    }

    /* ===================================== END  CREATE LIST TARGET IN GAME  ===============================*/


    /* ===================================== START CREATE LIST BARRIER IN GAME  ===============================*/
    public void SpawnListBarrierRandomPosition(int numberBarriers)
    {
        pooledBarrierObjects = RandomPointsGenerator.GenerateObjectsByListPrefabs(listBarrierObjects, numberBarriers, MinSpawnBarrierPos, MaxSpawnBarrierPos);
        Debug.Log("SpawnListBarrierRandomPosition pooledBarrierObjects:" + pooledBarrierObjects[0]);
    }

    /* ===================================== END CREATE LIST BARRIER IN GAME  ===============================*/




    /* ===================================== START CREATE LIST COIN IN GAME  ===============================*/
    IEnumerator SpawnBonusCoin(Vector3 targetPos, int index)
    {
        GameObject bonusCoin = Instantiate(coinPrefab, GetCoinPosWithTargetPos(targetPos, index), coinPrefab.transform.rotation);
        pooledBonusCoinObjects.Add(bonusCoin);
        yield return 0f;
    }

    Vector3 GetCoinPosWithTargetPos(Vector3 targetPos, int index)
    {
        // draw coin vị trí nằm trên đường thẳng nối từ target hướng tới vị trí quả bóng
        Vector3 objectPosition = Vector3.Lerp(targetPos, new Vector3(0.0f, 0.16f, -14.0f), (index * 0.1f)); // Adjust the 0.05f to control the position along the line
        return objectPosition;
    }

    /* ===================================== END CREATE LIST COIN IN GAME  ===============================*/



}