using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource audioSource;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LoadMainMenu());
        }
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
}
