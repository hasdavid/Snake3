using UnityEngine;
using UnityEngine.UI;

namespace Snake3
{
    public class PauseMenuItem : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Text _scoreText;

        public void OnGamePaused()
        {
            _scoreText.text = string.Format(_scoreText.text, _gameManager.Score);
            gameObject.SetActive(true);
        }

        public void OnGameResumed()
        {
            gameObject.SetActive(false);
        }
    }
}
