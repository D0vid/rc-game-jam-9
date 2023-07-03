using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class FadeTransition : Singleton<FadeTransition>
{
    [SerializeField] private float fadeDuration = 0.2f;
    [SerializeField] private float blackOutDuration = 0.2f;

    private RawImage _rawImage;

    protected override void Awake()
    {
        base.Awake();
        _rawImage = GetComponentInChildren<RawImage>();
    }

    public IEnumerator DoFade()
    {
        _rawImage.enabled = true;
        float alpha = 0;
        while (alpha < 1)
        {
            _rawImage.color = new Color(0, 0, 0, alpha);
            alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
        // Ensure that color alpha is 1 at the end of the loop
        _rawImage.color = new Color(0, 0, 0, 1);
        yield return null;
    }

    public IEnumerator WaitForBlackout()
    {
        yield return new WaitForSeconds(blackOutDuration);
    }

    public IEnumerator UndoFade()
    {
        float alpha = 1;
        while (alpha >= 0)
        {
            _rawImage.color = new Color(0, 0, 0, alpha);
            alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        // Ensure that color alpha is 0 at the end of the loop
        _rawImage.color = new Color(0, 0, 0, 0);
        _rawImage.enabled = false;
        yield return null;
    }
}