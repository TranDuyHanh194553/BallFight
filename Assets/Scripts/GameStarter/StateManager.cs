using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStarter
{
    public class StateManager : MonoBehaviour
    {
        //singleton
        public static StateManager Instance;

        //Game state
        private GameState _state;
        
        private void Awake()
        {
            Instance = this;
        }

        public GameState State
        {
            get => _state;
            set
            {
                var before = _state;
                _state = value; 
                OnStateChange(before);
                
            }
        }
        
        
        private void OnStateChange(GameState before)
        {
            Debug.Log( $"Game Phase change from {before} to {State}");
        }
        
        
    
    }
}

