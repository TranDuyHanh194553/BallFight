using UnityEngine;

namespace GameStarter
{
    public class ScoreManager: MonoBehaviour, IScoreManager
    {
        private const string KeyPlayerScore = "player_score";
        private const string KeyCurrentLevel = "current_level";
        private const string KeyCurrentStage = "current_stage";
        private const string KeyUnlockedLevel = "unlock_level";
        private int _playerScore;
        private int _currentLevel;
        private int _currentStage;

        public static ScoreManager Instance;

        public int Score
        {
            get => _playerScore;
            private set
            {
                var before = _playerScore;
                _playerScore = value;
                OnScoreChange(before);
            }
        }

        public int CurrentLevel
        {
            get => _currentLevel;
            private set
            {
                var before = _currentLevel;
                _currentLevel = value; 
                OnLevelChange(before);
            } 
        }


        public int CurrentStage
        {
            get => _currentStage;
            private set
            {
                var before = _currentStage;
                _currentStage = value;
                OnStageChange(before);
            }
        }

        public int UnlockedLevel { get; private set; }

        void Awake()
        {
            Debug.Log("CurrentStage:");
            Score = PlayerPrefs.GetInt(KeyPlayerScore);
            CurrentLevel = PlayerPrefs.GetInt(KeyCurrentLevel, 1);
            CurrentStage = PlayerPrefs.GetInt(KeyCurrentStage, 1);
            Debug.Log("CurrentStage:" + CurrentStage);
            UnlockedLevel = PlayerPrefs.GetInt(KeyUnlockedLevel, 1);
            Instance = this;
        }
        void OnScoreChange(int before)
        {
            Debug.Log($"Player score change from {before} to {Score}");
            PlayerPrefs.SetInt(KeyPlayerScore, Score);
        }

        void OnLevelChange(int before)
        {
            Debug.Log($"player level from {before} up to {CurrentLevel}");

            PlayerPrefs.SetInt(KeyCurrentLevel, CurrentLevel);
            if (UnlockedLevel < CurrentLevel)
            {
                UnlockedLevel = CurrentLevel;
                PlayerPrefs.SetInt(KeyUnlockedLevel, UnlockedLevel);
            }
        }


        void OnStageChange(int before)
        {
            Debug.Log($"player stage from {before} up to {CurrentStage}");
            PlayerPrefs.SetInt(KeyCurrentStage, CurrentStage);
   
        }


        public void AddScore(int score)
        {
            Score += score;
        }

        public void NextLevel()
        {
            CurrentLevel += 1;
            CurrentStage = 1;
            
        }

        public void NextStage()
        {
            if(CurrentLevel == Common.LIMIT_LEVEL && CurrentStage == Common.LIMIT_STAGE)
            {
                // reset game
                ResetLevelAndStage();
                return;
            }


            if(CurrentStage == Common.LIMIT_STAGE)
            {
                NextLevel();
                return;
            }


            CurrentStage += 1;
        }


        public void ResetLevelAndStage()
        {
            CurrentLevel = 1;
            CurrentStage = 1;
        }

        public void ResetStage()
        {
            CurrentStage = 1;
        }

    }
}