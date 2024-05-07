//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/PLS_InputActions.inputactions
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

public partial class @PLS_InputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PLS_InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PLS_InputActions"",
    ""maps"": [
        {
            ""name"": ""LumiaAction"",
            ""id"": ""6dba18fb-b79c-4e81-a7b9-55401bcaf6be"",
            ""actions"": [
                {
                    ""name"": ""LeftStick"",
                    ""type"": ""Value"",
                    ""id"": ""03eafb36-11e2-4c9c-bd98-8fa516f06790"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Map"",
                    ""type"": ""Button"",
                    ""id"": ""07d26349-678b-4001-a958-bdebb61e45c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""7289debe-8ec6-490e-bdaa-5f24f7517168"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slash"",
                    ""type"": ""Button"",
                    ""id"": ""8caf7d3f-350d-4254-993b-b72b8b402245"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""1361a9cf-a5de-4c0b-96f1-123473bc5d78"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Teleport"",
                    ""type"": ""Button"",
                    ""id"": ""8c172794-5a60-4b29-8496-04ef20b0e2ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""101be294-667c-42d7-b7be-eaf64d8e3361"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfb03a10-6669-4bc7-b067-471f51c0b2bc"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44f53370-1195-4b7a-b7a1-81ab7e3f4d9f"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Slash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31c1ae33-b7f9-4d18-aa19-aae4b4fc4078"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Slash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79ae022f-5912-4ff5-89f2-072819b6be94"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2125381f-926b-4343-afc9-f3022227d5ae"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0eb5287a-8640-4956-85a9-210ee7ac4ed2"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Key"",
                    ""id"": ""0c61dc18-c84b-470a-94fe-c4428344b215"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftStick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""66ebc686-bd11-43d0-8d6b-26feaa7db7be"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dd6e3666-8c4c-46e7-b5bd-903d759409f7"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f291b67a-fbfe-4ea8-a578-a4a9572aca70"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""07136f73-55b3-45a9-bced-62a1a3232953"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9c253837-0b20-4835-8483-4a626b94f766"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c385d0e-b2e2-48bb-be57-b8f737bf7043"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edbb04d2-1c65-42a4-901b-3df1f34ef0d1"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Teleport"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d289f7d4-50c0-4fdb-af8a-e1de3b08b60d"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Teleport"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // LumiaAction
        m_LumiaAction = asset.FindActionMap("LumiaAction", throwIfNotFound: true);
        m_LumiaAction_LeftStick = m_LumiaAction.FindAction("LeftStick", throwIfNotFound: true);
        m_LumiaAction_Map = m_LumiaAction.FindAction("Map", throwIfNotFound: true);
        m_LumiaAction_Jump = m_LumiaAction.FindAction("Jump", throwIfNotFound: true);
        m_LumiaAction_Slash = m_LumiaAction.FindAction("Slash", throwIfNotFound: true);
        m_LumiaAction_Shoot = m_LumiaAction.FindAction("Shoot", throwIfNotFound: true);
        m_LumiaAction_Teleport = m_LumiaAction.FindAction("Teleport", throwIfNotFound: true);
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

    // LumiaAction
    private readonly InputActionMap m_LumiaAction;
    private List<ILumiaActionActions> m_LumiaActionActionsCallbackInterfaces = new List<ILumiaActionActions>();
    private readonly InputAction m_LumiaAction_LeftStick;
    private readonly InputAction m_LumiaAction_Map;
    private readonly InputAction m_LumiaAction_Jump;
    private readonly InputAction m_LumiaAction_Slash;
    private readonly InputAction m_LumiaAction_Shoot;
    private readonly InputAction m_LumiaAction_Teleport;
    public struct LumiaActionActions
    {
        private @PLS_InputActions m_Wrapper;
        public LumiaActionActions(@PLS_InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftStick => m_Wrapper.m_LumiaAction_LeftStick;
        public InputAction @Map => m_Wrapper.m_LumiaAction_Map;
        public InputAction @Jump => m_Wrapper.m_LumiaAction_Jump;
        public InputAction @Slash => m_Wrapper.m_LumiaAction_Slash;
        public InputAction @Shoot => m_Wrapper.m_LumiaAction_Shoot;
        public InputAction @Teleport => m_Wrapper.m_LumiaAction_Teleport;
        public InputActionMap Get() { return m_Wrapper.m_LumiaAction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LumiaActionActions set) { return set.Get(); }
        public void AddCallbacks(ILumiaActionActions instance)
        {
            if (instance == null || m_Wrapper.m_LumiaActionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_LumiaActionActionsCallbackInterfaces.Add(instance);
            @LeftStick.started += instance.OnLeftStick;
            @LeftStick.performed += instance.OnLeftStick;
            @LeftStick.canceled += instance.OnLeftStick;
            @Map.started += instance.OnMap;
            @Map.performed += instance.OnMap;
            @Map.canceled += instance.OnMap;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Slash.started += instance.OnSlash;
            @Slash.performed += instance.OnSlash;
            @Slash.canceled += instance.OnSlash;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @Teleport.started += instance.OnTeleport;
            @Teleport.performed += instance.OnTeleport;
            @Teleport.canceled += instance.OnTeleport;
        }

        private void UnregisterCallbacks(ILumiaActionActions instance)
        {
            @LeftStick.started -= instance.OnLeftStick;
            @LeftStick.performed -= instance.OnLeftStick;
            @LeftStick.canceled -= instance.OnLeftStick;
            @Map.started -= instance.OnMap;
            @Map.performed -= instance.OnMap;
            @Map.canceled -= instance.OnMap;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Slash.started -= instance.OnSlash;
            @Slash.performed -= instance.OnSlash;
            @Slash.canceled -= instance.OnSlash;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @Teleport.started -= instance.OnTeleport;
            @Teleport.performed -= instance.OnTeleport;
            @Teleport.canceled -= instance.OnTeleport;
        }

        public void RemoveCallbacks(ILumiaActionActions instance)
        {
            if (m_Wrapper.m_LumiaActionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ILumiaActionActions instance)
        {
            foreach (var item in m_Wrapper.m_LumiaActionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_LumiaActionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public LumiaActionActions @LumiaAction => new LumiaActionActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    public interface ILumiaActionActions
    {
        void OnLeftStick(InputAction.CallbackContext context);
        void OnMap(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSlash(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnTeleport(InputAction.CallbackContext context);
    }
}
