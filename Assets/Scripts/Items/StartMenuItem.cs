using UnityEngine;

namespace Snake3
{
    public class StartMenuItem : MonoBehaviour
    {
        public void OnSimulationStarted()
        {
            gameObject.SetActive(false);
        }
    }
}
