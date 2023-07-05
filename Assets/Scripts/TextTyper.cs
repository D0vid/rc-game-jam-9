using System;
using System.Collections;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private string[] dialogLines;
    [SerializeField] private float textSpeed = 0.05f;
    [SerializeField] private float speedUpTextSpeed = 0.02f;
    
    private InputChannel _inputChannel;
    
    private TextMeshProUGUI _textMesh;
    private float _defaultTextSpeed;

    private void Awake()
    {
        _inputChannel = Resources.Load("Input/InputChannel") as InputChannel;
        _textMesh = GetComponent<TextMeshProUGUI>();
        _defaultTextSpeed = textSpeed;
    }

    private void Start()
    {
        StartCoroutine(DisplayLines());
    }

    private IEnumerator DisplayLines()
    {
        foreach (var line in dialogLines)
        {
            _textMesh.text = "";
            yield return DisplayLine(line);
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        foreach (var letter in line)
        {
            _textMesh.text += letter;
            yield return new WaitForSeconds(letter == ',' ? textSpeed * 2 : textSpeed);
        }
        _textMesh.text += " ";
        yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);
    }

    private void OnMouseButtonPressed(Vector2 arg0)
    {
        textSpeed = speedUpTextSpeed;
    }

    private void OnMouseButtonReleased(Vector2 arg0)
    {
        textSpeed = _defaultTextSpeed;
    }

    private void OnEnable()
    {
        _inputChannel.mouseBeginDragEvent += OnMouseButtonPressed;
        _inputChannel.mouseEndDragEvent += OnMouseButtonReleased;
    }

    private void OnDisable()
    {
        _inputChannel.mouseBeginDragEvent -= OnMouseButtonPressed;
        _inputChannel.mouseEndDragEvent -= OnMouseButtonReleased;
    }
}
