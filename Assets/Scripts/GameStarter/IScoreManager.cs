namespace GameStarter
{
    public interface IScoreManager
    {
        int Score
        {
            get;
        } 
        void AddScore(int score);
        void NextLevel();

    }
}