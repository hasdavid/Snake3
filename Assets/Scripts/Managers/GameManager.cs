using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snake3
{
    public class GameManager : MonoBehaviour
    {
        // We use a static variable here to remember whether to show the StartGame screen or not.
        // This usage is justified by its simplicity, as other solutions are e.g. singletons and persistent GameObjects.
        private static bool _firstGame = true;

        public int Score;

        private ProgramState _programState;
        private EventManager _eventManager;

        private void Awake()
        {
            _eventManager = FindObjectOfType<EventManager>();
        }

        private void Start()
        {
            if (_firstGame)
            {
                // First thing after launching the game.
                Time.fixedDeltaTime = 1.0f / 3;
                _programState = ProgramState.ApplicationStarted;
                _firstGame = false;
            }
            else
            {
                // The user pressed restart at least once.
                _programState = ProgramState.SimulationRunning;
                // Skip the "Press any button to start" prompt.
                StartSimulation();
            }

            Score = 0;
        }

        private void Update()
        {
            switch (_programState)
            {
                case ProgramState.ApplicationStarted:
                    if (Input.anyKeyDown) StartSimulation();
                    break;
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

        private void StartSimulation()
        {
            Debug.Log("Starting the simulation.");
            _programState = ProgramState.SimulationRunning;
            _eventManager.SimulationStarted.Invoke();
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
