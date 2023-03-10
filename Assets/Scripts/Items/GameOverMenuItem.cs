using UnityEngine;
using UnityEngine.UI;

namespace Snake3
{
    public class GameOverMenuItem : MonoBehaviour
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private Text _scoreText;

        public void OnSimulationEnded()
        {
            _scoreText.text = string.Format(_scoreText.text, _gameManager.Score);
            gameObject.SetActive(true);
        }
    }
}
