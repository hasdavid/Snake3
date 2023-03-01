using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Snake3
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent GameResumed;
        public UnityEvent GamePaused;

        public int Score;

        private GameState _gameState;

        private void Start()
        {
            Time.fixedDeltaTime = 1.0f / 3;
            Score = 0;
            _gameState = GameState.Running;
        }

        private void Update()
        {
            switch (_gameState)
            {
                case GameState.Paused:
                    if (Input.GetKeyDown("escape")) ResumeGame();
                    else if (Input.GetKeyDown("r")) RestartGame();
                    else if (Input.GetKeyDown("q")) QuitGame();
                    break;
                case GameState.Running:
                    if (Input.GetKeyDown("escape")) PauseGame();
                    break;
                case GameState.GameOver:
                    if (Input.GetKeyDown("r")) RestartGame();
                    else if (Input.GetKeyDown("q")) QuitGame();
                    break;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException(
                        nameof(_gameState), (int)_gameState, _gameState.GetType());
            }
        }

        public void OnFoodEaten()
        {
            Score++;
        }

        public void OnGameOver()
        {
            _gameState = GameState.GameOver;
            Debug.Log("Game over.");
        }

        private void PauseGame()
        {
            Debug.Log("Pausing the game.");
            _gameState = GameState.Paused;
            GamePaused.Invoke();
        }

        private void ResumeGame()
        {
            Debug.Log("Resuming the game.");
            _gameState = GameState.Running;
            GameResumed.Invoke();
        }

        private void QuitGame()
        {
            Debug.Log("Quitting the game.");
            Application.Quit();
        }

        private void RestartGame()
        {
            Debug.Log("Restarting the game.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
