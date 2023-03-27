using UnityEngine;

namespace Snake3
{
    /**
     * Represents the Start Menu.
     *
     * This menu comes up automatically when the first game is loaded. It goes away when the simulation starts.
     */
    public class StartMenuPanel : MonoBehaviour
    {
        public void OnSimulationStarted()
        {
            gameObject.SetActive(false);
        }
    }
}
