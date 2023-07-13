using TMPro;
using UnityEngine;

namespace UI
{
    public class ButtonController : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;

        private void Awake()
        {
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void OnPointerEnter()
        {
            _textMesh.color = Color.yellow;
            _textMesh.text = $"> {_textMesh.text} <";
        }

        public void OnPointerExit()
        {
            _textMesh.color = Color.white;
            _textMesh.text = _textMesh.text.Replace("> ", "");
            _textMesh.text = _textMesh.text.Replace(" <", "");
        }
    }
}
