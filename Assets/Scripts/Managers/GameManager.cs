using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snake3
{
    public class GameManager : MonoBehaviour
    {
        public int Score;

        private ProgramState _programState;
        private EventManager _eventManager;

        private void Awake()
        {
            _eventManager = FindObjectOfType<EventManager>();
        }

        private void Start()
        {
            Time.fixedDeltaTime = 1.0f / 3;
            Score = 0;
            _programState = ProgramState.SimulationRunning;
        }

        private void Update()
        {
            switch (_programState)
            {
                case ProgramState.SimulationPaused:
                    if (Input.GetKeyDown("escape")) ResumeSimulation();
                    else if (Input.GetKeyDown("r")) RestartSimulation();
                    else if (Input.GetKeyDown("q")) QuitApplication();
                    break;
                case ProgramState.SimulationRunning:
                    if (Input.GetKeyDown("escape")) PauseSimulation();
                    break;
                case ProgramState.SimulationEnded:
                    if (Input.GetKeyDown("r")) RestartSimulation();
                    else if (Input.GetKeyDown("q")) QuitApplication();
                    break;
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException(
                        nameof(_programState), (int)_programState, _programState.GetType());
            }
        }

        public void OnSimulationFoodEaten()
        {
            Score++;
        }

        public void OnSimulationEnded()
        {
            _programState = ProgramState.SimulationEnded;
            Debug.Log("Ending the simulation.");
        }

        private void PauseSimulation()
        {
            Debug.Log("Pausing the simulation.");
            _programState = ProgramState.SimulationPaused;
            _eventManager.SimulationPaused.Invoke();
        }

        private void ResumeSimulation()
        {
            Debug.Log("Resuming the simulation.");
            _programState = ProgramState.SimulationRunning;
            _eventManager.SimulationResumed.Invoke();
        }

        private void RestartSimulation()
        {
            Debug.Log("Restarting the simulation.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void QuitApplication()
        {
            Debug.Log("Quitting the application.");
            Application.Quit();
        }
    }
}
