using System.Collections;
using TMPro;
using UnityEngine;

public class TextFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float fadeDelay = 1f;
    [SerializeField] private float minAlpha = 0f;
    
    private TextMeshProUGUI _textMesh;
    private bool _isFading;

    void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!_isFading)
        {
            _isFading = true;
            StartCoroutine(FadeInAndOut());
        }
    }
    
    private IEnumerator FadeInAndOut()
    {
        while (true)
        {
            yield return StartCoroutine(DoFade(minAlpha, fadeDuration));
            yield return new WaitForSeconds(fadeDelay);
            yield return StartCoroutine(DoFade(1f, fadeDuration));
            yield return new WaitForSeconds(fadeDelay);
        }
    }
    
    private IEnumerator DoFade(float targetAlpha, float duration)
    {
        float startTime = Time.time;
        Color startColor = _textMesh.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            _textMesh.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        _textMesh.color = targetColor;
    }
}
