using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snake3
{
    /**
     * Manages the game.
     *
     * Keeps track of the score and handles events that start, pause, resume and end the game.
     *
     * Is expected to be destroyed and instantiated again when the game restarts.
     *
     * There is one behavior, that should only happen before the first game when the program starts, and that is
     * showing the menu with the game title. We keep a static variable for this purpose - to remember whether to show
     * the StartGame screen or not. Although static variables are basically global variables and their use for flow
     * control is frowned upon, its usage here is justified by its simplicity. Other solutions include using singletons
     * or persistent GameObjects and lead to much more complicated code.
     */
    public class GameManager : MonoBehaviour
    {
        // ----------------------------
        // Fields
        // ----------------------------

        private static bool _firstGame = true;  // Static variable - see above for reasoning.

        [SerializeField] private EventManager _eventManager;

        public int Score;

        private ProgramState _programState;

        // ----------------------------
        // Event Functions
        // ----------------------------

        private void Start()
        {
            if (_firstGame)
            {
                // First thing after launching the program.
                Time.fixedDeltaTime = 1.0f / 5;
                _programState = ProgramState.ApplicationStarted;
                _firstGame = false;
            }
            else
            {
                // The user pressed restart at least once.
                // Skip the "Press any button to start" prompt.
                StartSimulation();
            }

            Score = 0;
        }

        /**
         * Every frame, check the input for pressed buttons and respond accordingly.
         */
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

        // ----------------------------
        // Methods
        // ----------------------------

        private void StartSimulation()
        {
            Debug.Log("Starting the simulation.");
            _programState = ProgramState.SimulationRunning;
            _eventManager.SimulationStarted.Invoke();
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
