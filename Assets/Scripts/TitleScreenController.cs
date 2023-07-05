using System.Collections;
using Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource audioSource;
    
    private InputChannel _inputChannel;
    
    private void Awake()
    {
        _inputChannel = Resources.Load("Input/InputChannel") as InputChannel;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadMainMenu()
    {
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        yield return FadeTransition.Instance.DoFade();
        yield return SceneManager.LoadSceneAsync(1);
        yield return FadeTransition.Instance.UndoFade();
        Destroy(gameObject);
    }

    private void OnMouseClicked(Vector2 mousePos)
    {
        StartCoroutine(LoadMainMenu());
    }

    private void OnEnable()
    {
        _inputChannel.mouseClickEvent += OnMouseClicked;
    }

    private void OnDisable()
    {
        _inputChannel.mouseClickEvent -= OnMouseClicked;
    }
}
