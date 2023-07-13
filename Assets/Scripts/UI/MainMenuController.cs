using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void OnNewGame()
        {
            StartCoroutine(LoadNewGame());
        }

        public void OnContinue()
        {
            Debug.Log("TODO : Continue game progress");
            StartCoroutine(LoadNewGame());
        }
    
        private IEnumerator LoadNewGame()
        {
            yield return FadeTransition.Instance.DoFade();
            yield return SceneManager.LoadSceneAsync(2);
            yield return FadeTransition.Instance.UndoFade();
            Destroy(gameObject);
        }
    }
}
