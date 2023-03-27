using UnityEngine;
using UnityEngine.UI;

namespace Snake3
{
    /**
     * Represents the Pause Menu.
     *
     * This menu comes up when the user hits Esc. It goes away with another Esc hit.
     *
     * The field _scoreText expects a Text GameObject containing the text that should be formatted with the actual
     * score. Expects "{0}" somewhere in the string.
     */
    public class PauseMenuPanel : MonoBehaviour
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
