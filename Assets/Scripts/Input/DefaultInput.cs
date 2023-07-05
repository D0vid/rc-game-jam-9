//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/Scripts/Input/DefaultInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @DefaultInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultInput"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""debd4a2e-8bfa-41ec-a85a-4a6b4d3ab94d"",
            ""actions"": [
                {
                    ""name"": ""MouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""8459c80c-0c57-44b7-a856-28b9c6b64442"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""56443ef3-0c5c-4063-8f2c-eac6bf72eab4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Hotkey1"",
                    ""type"": ""Button"",
                    ""id"": ""6731fb8f-8e22-4b80-aed6-ec2182b0325a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hotkey2"",
                    ""type"": ""Button"",
                    ""id"": ""d0e7e97b-cbd7-4211-a8dc-a3fc688486f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hotkey3"",
                    ""type"": ""Button"",
                    ""id"": ""beac1ec8-bb7b-415b-bd54-1e53f0202fbc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hotkey4"",
                    ""type"": ""Button"",
                    ""id"": ""d4c7aaab-1388-438c-b165-22f93bc3b498"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ReadySkipTurn"",
                    ""type"": ""Button"",
                    ""id"": ""6342bcc4-890f-41c1-a44f-57bfcfdd6af9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CancelAction"",
                    ""type"": ""Button"",
                    ""id"": ""74c70dc9-c7ec-4cbe-a310-43403d0899be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2b5bab3f-2a47-4435-bbdc-93d0ee57dbe5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d468cb4-48b1-4a72-96c7-eec9774a69d2"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6dbe563d-b52c-4371-ba8f-f5256254e580"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Hotkey1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7a78e79-c48b-4bfa-b09d-17f9b8473547"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Hotkey2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e98f6d0-8de4-49e9-a77d-be4380f7cf1f"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Hotkey3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c89cfa3-fb85-43a7-b022-74302b60adf5"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Hotkey4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4df70acb-c2ed-4b93-94f2-bcd9098d8f72"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ReadySkipTurn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e68d9470-e53f-46b3-8d5e-140d1cb5c3c0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""CancelAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": []
        }
    ]
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_MouseClick = m_Default.FindAction("MouseClick", throwIfNotFound: true);
        m_Default_MousePosition = m_Default.FindAction("MousePosition", throwIfNotFound: true);
        m_Default_Hotkey1 = m_Default.FindAction("Hotkey1", throwIfNotFound: true);
        m_Default_Hotkey2 = m_Default.FindAction("Hotkey2", throwIfNotFound: true);
        m_Default_Hotkey3 = m_Default.FindAction("Hotkey3", throwIfNotFound: true);
        m_Default_Hotkey4 = m_Default.FindAction("Hotkey4", throwIfNotFound: true);
        m_Default_ReadySkipTurn = m_Default.FindAction("ReadySkipTurn", throwIfNotFound: true);
        m_Default_CancelAction = m_Default.FindAction("CancelAction", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Default
    private readonly InputActionMap m_Default;
    private List<IDefaultActions> m_DefaultActionsCallbackInterfaces = new List<IDefaultActions>();
    private readonly InputAction m_Default_MouseClick;
    private readonly InputAction m_Default_MousePosition;
    private readonly InputAction m_Default_Hotkey1;
    private readonly InputAction m_Default_Hotkey2;
    private readonly InputAction m_Default_Hotkey3;
    private readonly InputAction m_Default_Hotkey4;
    private readonly InputAction m_Default_ReadySkipTurn;
    private readonly InputAction m_Default_CancelAction;
    public struct DefaultActions
    {
        private @DefaultInput m_Wrapper;
        public DefaultActions(@DefaultInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseClick => m_Wrapper.m_Default_MouseClick;
        public InputAction @MousePosition => m_Wrapper.m_Default_MousePosition;
        public InputAction @Hotkey1 => m_Wrapper.m_Default_Hotkey1;
        public InputAction @Hotkey2 => m_Wrapper.m_Default_Hotkey2;
        public InputAction @Hotkey3 => m_Wrapper.m_Default_Hotkey3;
        public InputAction @Hotkey4 => m_Wrapper.m_Default_Hotkey4;
        public InputAction @ReadySkipTurn => m_Wrapper.m_Default_ReadySkipTurn;
        public InputAction @CancelAction => m_Wrapper.m_Default_CancelAction;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void AddCallbacks(IDefaultActions instance)
        {
            if (instance == null || m_Wrapper.m_DefaultActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Add(instance);
            @MouseClick.started += instance.OnMouseClick;
            @MouseClick.performed += instance.OnMouseClick;
            @MouseClick.canceled += instance.OnMouseClick;
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
            @Hotkey1.started += instance.OnHotkey1;
            @Hotkey1.performed += instance.OnHotkey1;
            @Hotkey1.canceled += instance.OnHotkey1;
            @Hotkey2.started += instance.OnHotkey2;
            @Hotkey2.performed += instance.OnHotkey2;
            @Hotkey2.canceled += instance.OnHotkey2;
            @Hotkey3.started += instance.OnHotkey3;
            @Hotkey3.performed += instance.OnHotkey3;
            @Hotkey3.canceled += instance.OnHotkey3;
            @Hotkey4.started += instance.OnHotkey4;
            @Hotkey4.performed += instance.OnHotkey4;
            @Hotkey4.canceled += instance.OnHotkey4;
            @ReadySkipTurn.started += instance.OnReadySkipTurn;
            @ReadySkipTurn.performed += instance.OnReadySkipTurn;
            @ReadySkipTurn.canceled += instance.OnReadySkipTurn;
            @CancelAction.started += instance.OnCancelAction;
            @CancelAction.performed += instance.OnCancelAction;
            @CancelAction.canceled += instance.OnCancelAction;
        }

        private void UnregisterCallbacks(IDefaultActions instance)
        {
            @MouseClick.started -= instance.OnMouseClick;
            @MouseClick.performed -= instance.OnMouseClick;
            @MouseClick.canceled -= instance.OnMouseClick;
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
            @Hotkey1.started -= instance.OnHotkey1;
            @Hotkey1.performed -= instance.OnHotkey1;
            @Hotkey1.canceled -= instance.OnHotkey1;
            @Hotkey2.started -= instance.OnHotkey2;
            @Hotkey2.performed -= instance.OnHotkey2;
            @Hotkey2.canceled -= instance.OnHotkey2;
            @Hotkey3.started -= instance.OnHotkey3;
            @Hotkey3.performed -= instance.OnHotkey3;
            @Hotkey3.canceled -= instance.OnHotkey3;
            @Hotkey4.started -= instance.OnHotkey4;
            @Hotkey4.performed -= instance.OnHotkey4;
            @Hotkey4.canceled -= instance.OnHotkey4;
            @ReadySkipTurn.started -= instance.OnReadySkipTurn;
            @ReadySkipTurn.performed -= instance.OnReadySkipTurn;
            @ReadySkipTurn.canceled -= instance.OnReadySkipTurn;
            @CancelAction.started -= instance.OnCancelAction;
            @CancelAction.performed -= instance.OnCancelAction;
            @CancelAction.canceled -= instance.OnCancelAction;
        }

        public void RemoveCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDefaultActions instance)
        {
            foreach (var item in m_Wrapper.m_DefaultActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IDefaultActions
    {
        void OnMouseClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnHotkey1(InputAction.CallbackContext context);
        void OnHotkey2(InputAction.CallbackContext context);
        void OnHotkey3(InputAction.CallbackContext context);
        void OnHotkey4(InputAction.CallbackContext context);
        void OnReadySkipTurn(InputAction.CallbackContext context);
        void OnCancelAction(InputAction.CallbackContext context);
    }
}
