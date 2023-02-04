using UnityEngine;

namespace Snake3
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            Time.fixedDeltaTime = 1.0f / 3;
        }
    }
}
