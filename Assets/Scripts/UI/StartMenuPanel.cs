using UnityEngine;

namespace Snake3
{
    public class StartMenuPanel : MonoBehaviour
    {
        public void OnSimulationStarted()
        {
            gameObject.SetActive(false);
        }
    }
}
