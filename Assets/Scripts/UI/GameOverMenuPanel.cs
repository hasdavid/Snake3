using UnityEngine;
using UnityEngine.UI;

namespace Snake3
{
    /**
     * Represents the Game Over Menu.
     *
     * This menu comes up when the snake dies or wins. It goes away with the scene exiting.
     *
     * The field _scoreText expects a Text GameObject containing the text that should be formatted with the actual
     * score. Expects "{0}" somewhere in the string.
     */
    public class GameOverMenuPanel : MonoBehaviour
    {
        // ----------------------------
        // Fields
        // ----------------------------

        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Text _scoreText;

        private string _unformattedText;

        // ----------------------------
        // Event Functions
        // ----------------------------

        private void Awake()
        {
            // We need to save the source text at the start, so that we can format it later multiple times.
            _unformattedText = string.Copy(_scoreText.text);
        }

        public void OnSimulationEnded()
        {
            // We need to set the object to active first, so that Awake() is called.
            gameObject.SetActive(true);
            _scoreText.text = string.Format(_unformattedText, _gameManager.Score);
        }
    }
}
