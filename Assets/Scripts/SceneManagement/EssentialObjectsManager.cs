using UnityEngine;

namespace SceneManagement
{
    public class EssentialObjectsManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
