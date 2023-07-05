using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Channels/InputChannel")]
    public class InputChannel : ScriptableObject, DefaultInput.IDefaultActions
    {
        public event UnityAction<Vector2> mousePositionEvent;
        public event UnityAction<Vector2> mouseClickEvent;
        public event UnityAction<Vector2> mouseBeginDragEvent;
        public event UnityAction<Vector2> mouseEndDragEvent;
        public event UnityAction<int> hotkeyPressed;
        public event UnityAction readySkipTurn;
        public event UnityAction actionCanceled;

        private DefaultInput _defaultInput;

        private void OnEnable()
        {
            if (_defaultInput == null)
            {
                _defaultInput = new DefaultInput();
                _defaultInput.Default.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void OnMouseClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 mousePosition = _defaultInput.Default.MousePosition.ReadValue<Vector2>();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mouseClickEvent?.Invoke(mousePosition);
            }

            if (context.started)
            {
                Vector2 mousePosition = _defaultInput.Default.MousePosition.ReadValue<Vector2>();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mouseBeginDragEvent?.Invoke(mousePosition);
            }

            if (context.canceled)
            {
                Vector2 mousePosition = _defaultInput.Default.MousePosition.ReadValue<Vector2>();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mouseEndDragEvent?.Invoke(mousePosition);
            }
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 mousePosition = _defaultInput.Default.MousePosition.ReadValue<Vector2>();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePositionEvent?.Invoke(mousePosition);
            }
        }

        public void OnHotkey1(InputAction.CallbackContext context)
        {
            if (context.performed)
                hotkeyPressed?.Invoke(1);
        }

        public void OnHotkey2(InputAction.CallbackContext context)
        {
            if (context.performed)
                hotkeyPressed?.Invoke(2);
        }

        public void OnHotkey3(InputAction.CallbackContext context)
        {
            if (context.performed)
                hotkeyPressed?.Invoke(3);
        }

        public void OnHotkey4(InputAction.CallbackContext context)
        {
            if (context.performed)
                hotkeyPressed?.Invoke(4);
        }

        public void OnReadySkipTurn(InputAction.CallbackContext context)
        {
            if (context.performed)
                readySkipTurn?.Invoke();
        }

        public void OnCancelAction(InputAction.CallbackContext context)
        {
            if (context.performed)
                actionCanceled?.Invoke();
        }

        private void EnableGameplayInput()
        {
            _defaultInput.Default.Enable();
        }

        private void DisableAllInput()
        {
            _defaultInput.Default.Disable();
        }
    }
}