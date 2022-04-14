// GENERATED AUTOMATICALLY FROM 'Assets/Input System/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""GameControl"",
            ""id"": ""55b5772c-ef56-4115-88f8-79c4add267c5"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""8c0ed59a-5c47-41fd-92f5-e178eaf97912"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5fbb073d-dd9e-4cd5-b2a8-56ab9423fbae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""18e7b1db-e576-4160-bdb4-57c533a2b947"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pick"",
                    ""type"": ""Button"",
                    ""id"": ""44f96cbc-fd12-49d7-89a5-1737d62cb07b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""4de972ce-981e-4392-83fa-9f3bb2e99564"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Punch"",
                    ""type"": ""Button"",
                    ""id"": ""ff42cead-2a4c-46e7-9722-a1fdb71356a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7798e730-b5af-4b97-a409-ce4d02d4e1df"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""f5917b2c-6f21-46c3-9bbd-09c6f97caec9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7ee29b0b-be16-41fa-ac48-7a154446d052"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4724677b-b935-4909-971c-fd39193db0cc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""225cc93b-e0e8-48a6-9b8b-e809af597315"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4ac71d88-3cd6-4e09-b5ca-de42b53833ed"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""54d7b81e-e2ff-4e61-b4f9-d7523574a956"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""559e7ae1-54f6-4182-9940-911c7ecf2446"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2563e700-3c47-46e9-b88f-1ecefd824e84"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d0c388e-7279-472e-b31b-a69be2efa9ba"",
                    ""path"": ""<Mouse>/forwardButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Punch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameControl
        m_GameControl = asset.FindActionMap("GameControl", throwIfNotFound: true);
        m_GameControl_Move = m_GameControl.FindAction("Move", throwIfNotFound: true);
        m_GameControl_Jump = m_GameControl.FindAction("Jump", throwIfNotFound: true);
        m_GameControl_Look = m_GameControl.FindAction("Look", throwIfNotFound: true);
        m_GameControl_Pick = m_GameControl.FindAction("Pick", throwIfNotFound: true);
        m_GameControl_Drop = m_GameControl.FindAction("Drop", throwIfNotFound: true);
        m_GameControl_Punch = m_GameControl.FindAction("Punch", throwIfNotFound: true);
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

    // GameControl
    private readonly InputActionMap m_GameControl;
    private IGameControlActions m_GameControlActionsCallbackInterface;
    private readonly InputAction m_GameControl_Move;
    private readonly InputAction m_GameControl_Jump;
    private readonly InputAction m_GameControl_Look;
    private readonly InputAction m_GameControl_Pick;
    private readonly InputAction m_GameControl_Drop;
    private readonly InputAction m_GameControl_Punch;
    public struct GameControlActions
    {
        private @PlayerInput m_Wrapper;
        public GameControlActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_GameControl_Move;
        public InputAction @Jump => m_Wrapper.m_GameControl_Jump;
        public InputAction @Look => m_Wrapper.m_GameControl_Look;
        public InputAction @Pick => m_Wrapper.m_GameControl_Pick;
        public InputAction @Drop => m_Wrapper.m_GameControl_Drop;
        public InputAction @Punch => m_Wrapper.m_GameControl_Punch;
        public InputActionMap Get() { return m_Wrapper.m_GameControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameControlActions set) { return set.Get(); }
        public void SetCallbacks(IGameControlActions instance)
        {
            if (m_Wrapper.m_GameControlActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnJump;
                @Look.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnLook;
                @Pick.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnPick;
                @Pick.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnPick;
                @Pick.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnPick;
                @Drop.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnDrop;
                @Punch.started -= m_Wrapper.m_GameControlActionsCallbackInterface.OnPunch;
                @Punch.performed -= m_Wrapper.m_GameControlActionsCallbackInterface.OnPunch;
                @Punch.canceled -= m_Wrapper.m_GameControlActionsCallbackInterface.OnPunch;
            }
            m_Wrapper.m_GameControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Pick.started += instance.OnPick;
                @Pick.performed += instance.OnPick;
                @Pick.canceled += instance.OnPick;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @Punch.started += instance.OnPunch;
                @Punch.performed += instance.OnPunch;
                @Punch.canceled += instance.OnPunch;
            }
        }
    }
    public GameControlActions @GameControl => new GameControlActions(this);
    public interface IGameControlActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnPick(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnPunch(InputAction.CallbackContext context);
    }
}
