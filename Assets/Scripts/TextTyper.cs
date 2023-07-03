using System.Collections;
using TMPro;
using UnityEngine;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private string[] dialogLines;
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float speedUpTextSpeed = 0.02f;
    
    private TextMeshProUGUI _textMesh;
    private float _savedTextSpeed;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _savedTextSpeed = textSpeed;
    }

    private void Start()
    {
        StartCoroutine(DisplayLines());
    }

    private void Update()
    {
        textSpeed = Input.GetMouseButton(1) ? speedUpTextSpeed : _savedTextSpeed;
    }

    private IEnumerator DisplayLines()
    {
        foreach (var line in dialogLines)
        {
            _textMesh.text = "";
            yield return DisplayLine(line);
        }
        _textMesh.text = "";
    }

    private IEnumerator DisplayLine(string line)
    {
        foreach (var letter in line)
        {
            _textMesh.text += letter;
            yield return new WaitForSeconds(letter == ',' ? textSpeed * 2 : textSpeed);
        }
        _textMesh.text += " ";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }
}
