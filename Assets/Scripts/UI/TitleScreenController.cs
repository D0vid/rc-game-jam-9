using System.Collections;
using Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class TitleScreenController : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private InputChannel inputChannel;
    
        private void Awake() => DontDestroyOnLoad(gameObject);

        private IEnumerator LoadMainMenu()
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
            yield return FadeTransition.Instance.DoFade();
            yield return SceneManager.LoadSceneAsync(1);
            yield return FadeTransition.Instance.UndoFade();
            Destroy(gameObject);
        }

        private void OnMouseClicked(Vector2 mousePos) => StartCoroutine(LoadMainMenu());

        private void OnEnable() => inputChannel.mouseClickEvent += OnMouseClicked;

        private void OnDisable() => inputChannel.mouseClickEvent -= OnMouseClicked;
    }
}
