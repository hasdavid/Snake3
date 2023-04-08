using UnityEngine;
using UnityEngine.UI;

namespace Snake3
{
    public class PauseMenuPanel : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Text _scoreText;

        private string _unformattedText;

        private void Awake()
        {
            // We want to save the source text at the start, so that we can format it later multiple times.
            _unformattedText = string.Copy(_scoreText.text);
        }

        public void OnSimulationPaused()
        {
            // We need to set the object to active first, so that Awake() is called.
            gameObject.SetActive(true);
            _scoreText.text = string.Format(_unformattedText, _gameManager.Score);
        }

        public void OnSimulationResumed()
        {
            gameObject.SetActive(false);
        }
    }
}
