using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class KeySettingSC : MonoBehaviour
{
    private MyInputManager _myInput;
    private InputAction[] _inputActions;
    private ControllerType _currentDevice;
    private int _SelectedControlType = 0;//0=Gamepad, 1=Keyboard
    private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    [SerializeField] private Text[] _KeyText;
    [SerializeField] private Button[] MovementKeys;
    [HideInInspector] public bool _Waiting;
    private int _Target;
    private string[] _WaitText = {
        "새 키 입력",
        "新しいキー入力"
    };
    private enum Buttons
    {
        ButtonNorth,
        ButtonEast,
        ButtonSouth,
        ButtonWest,
        LeftShoulder,
        RightShoulder,
        LeftTrigger,
        RightTrigger,
        Select,
        Start
    }
    private readonly Dictionary<Buttons, string> _buttonPathDic = new();
    private readonly Dictionary<ControllerType, Dictionary<string, string>> _deviceLayoutDic = new();

    void CreateDictionary()
    {
        _buttonPathDic.Add(Buttons.ButtonNorth, "<Gamepad>/buttonNorth");
        _buttonPathDic.Add(Buttons.ButtonEast, "<Gamepad>/buttonEast");
        _buttonPathDic.Add(Buttons.ButtonSouth, "<Gamepad>/buttonSouth");
        _buttonPathDic.Add(Buttons.ButtonWest, "<Gamepad>/buttonWest");
        _buttonPathDic.Add(Buttons.LeftShoulder, "<Gamepad>/leftShoulder");
        _buttonPathDic.Add(Buttons.RightShoulder, "<Gamepad>/rightShoulder");
        _buttonPathDic.Add(Buttons.LeftTrigger, "<Gamepad>/leftTrigger");
        _buttonPathDic.Add(Buttons.RightTrigger, "<Gamepad>/rightTrigger");
        _buttonPathDic.Add(Buttons.Select, "<Gamepad>/select");
        _buttonPathDic.Add(Buttons.Start, "<Gamepad>/start");
        Dictionary<string, string> dualSence = new();
        dualSence.Add(_buttonPathDic[Buttons.ButtonNorth], "△");
        dualSence.Add(_buttonPathDic[Buttons.ButtonEast], "○");
        dualSence.Add(_buttonPathDic[Buttons.ButtonSouth], "×");
        dualSence.Add(_buttonPathDic[Buttons.ButtonWest], "□");
        dualSence.Add(_buttonPathDic[Buttons.LeftShoulder], "L1");
        dualSence.Add(_buttonPathDic[Buttons.RightShoulder], "R1");
        dualSence.Add(_buttonPathDic[Buttons.LeftTrigger], "L2");
        dualSence.Add(_buttonPathDic[Buttons.RightTrigger], "R2");
        dualSence.Add(_buttonPathDic[Buttons.Select], "Create");
        dualSence.Add(_buttonPathDic[Buttons.Start], "Options");
        _deviceLayoutDic.Add(ControllerType.DualSence, dualSence);
        Dictionary<string, string> dualShock = new();
        dualShock.Add(_buttonPathDic[Buttons.ButtonNorth], "△");
        dualShock.Add(_buttonPathDic[Buttons.ButtonEast], "○");
        dualShock.Add(_buttonPathDic[Buttons.ButtonSouth], "×");
        dualShock.Add(_buttonPathDic[Buttons.ButtonWest], "□");
        dualShock.Add(_buttonPathDic[Buttons.LeftShoulder], "L1");
        dualShock.Add(_buttonPathDic[Buttons.RightShoulder], "R1");
        dualShock.Add(_buttonPathDic[Buttons.LeftTrigger], "L2");
        dualShock.Add(_buttonPathDic[Buttons.RightTrigger], "R2");
        dualShock.Add(_buttonPathDic[Buttons.Select], "Select");
        dualShock.Add(_buttonPathDic[Buttons.Start], "Start");
        _deviceLayoutDic.Add(ControllerType.DualShock, dualShock);
        Dictionary<string, string> xbox = new();
        xbox.Add(_buttonPathDic[Buttons.ButtonNorth], "Y");
        xbox.Add(_buttonPathDic[Buttons.ButtonEast], "B");
        xbox.Add(_buttonPathDic[Buttons.ButtonSouth], "A");
        xbox.Add(_buttonPathDic[Buttons.ButtonWest], "X");
        xbox.Add(_buttonPathDic[Buttons.LeftShoulder], "LB");
        xbox.Add(_buttonPathDic[Buttons.RightShoulder], "RB");
        xbox.Add(_buttonPathDic[Buttons.LeftTrigger], "LT");
        xbox.Add(_buttonPathDic[Buttons.RightTrigger], "RT");
        xbox.Add(_buttonPathDic[Buttons.Select], "Back");
        xbox.Add(_buttonPathDic[Buttons.Start], "Start");
        _deviceLayoutDic.Add(ControllerType.Xbox, xbox);
        Dictionary<string, string> ns = new();
        ns.Add(_buttonPathDic[Buttons.ButtonNorth], "X");
        ns.Add(_buttonPathDic[Buttons.ButtonEast], "A");
        ns.Add(_buttonPathDic[Buttons.ButtonSouth], "B");
        ns.Add(_buttonPathDic[Buttons.ButtonWest], "Y");
        ns.Add(_buttonPathDic[Buttons.LeftShoulder], "L");
        ns.Add(_buttonPathDic[Buttons.RightShoulder], "R");
        ns.Add(_buttonPathDic[Buttons.LeftTrigger], "ZL");
        ns.Add(_buttonPathDic[Buttons.RightTrigger], "ZR");
        ns.Add(_buttonPathDic[Buttons.Select], "-");
        ns.Add(_buttonPathDic[Buttons.Start], "+");
        _deviceLayoutDic.Add(ControllerType.Switch, ns);
    }
    void Awake()
    {
        _myInput = MyInputManager.GetMyInput();
        CreateDictionary();
        _inputActions = new[]
        {
            _myInput.InputActionDic[KeyType.LeftStick],
            _myInput.InputActionDic[KeyType.LeftStick],
            _myInput.InputActionDic[KeyType.LeftStick],
            _myInput.InputActionDic[KeyType.LeftStick],
            _myInput.InputActionDic[KeyType.Map],
            _myInput.InputActionDic[KeyType.Jump],
            _myInput.InputActionDic[KeyType.Slash],
            _myInput.InputActionDic[KeyType.Shoot],
            _myInput.InputActionDic[KeyType.Teleport],
            _myInput.InputActionDic[KeyType.Submit],
            _myInput.InputActionDic[KeyType.Cancel],
            _myInput.InputActionDic[KeyType.Pause],
            _myInput.InputActionDic[KeyType.Status],
            _myInput.InputActionDic[KeyType.Warp],
            _myInput.InputActionDic[KeyType.Shield]
        };
    }
    void OnEnable()
    {
        DetectControllerType();
        //InputSystem.onAnyButtonPress.Call(DetectDevice);
        _UpdateKeys(1);
    }

    private void DetectControllerType()
    {
        InputDevice gamepadType = Gamepad.current;
        Debug.Log($"GamePadType is {gamepadType}");
        switch (gamepadType)
        {
            case DualSenseGamepadHID :
                _currentDevice = ControllerType.DualSence;
                break;
            case DualShockGamepad:
                _currentDevice = ControllerType.DualShock;
                break;
            case SwitchProControllerHID:
                _currentDevice = ControllerType.Switch;
                break;
            case XInputController :
                _currentDevice = ControllerType.Xbox;
                break;
            default:
                _currentDevice = ControllerType.Xbox;
                break;
        }
    }
    private void DetectDeviceTest(InputControl control)
    {
        
        if (control.device is Keyboard)
        {
            Debug.Log("This is Keyboard");
        }
        if (control.device is Gamepad)
        {
            Debug.Log("This is Gamepad");
        }
    }

    public void _UpdateKeys(int deviceIndex)
    {
        for (int i = 0; i < 4; i++)
        {
            MovementKeys[i].interactable = deviceIndex == 1;
        }
        _SelectedControlType = deviceIndex;
        if (deviceIndex == 0)
        {
            //GamePad
            DetectControllerType();
            for (int i = 0; i < _KeyText.Length; i++)
            {
                string text = _inputActions[i].bindings[deviceIndex].effectivePath;
                Dictionary<string, string> dic = _deviceLayoutDic[_currentDevice];
                if (dic.ContainsKey(text))
                {
                    _KeyText[i].text = dic[text];
                }
                else
                {
                    //Debug.Log($"Not Containing {text}");
                    _KeyText[i].text =InputControlPath.ToHumanReadableString(
                        text, InputControlPath.HumanReadableStringOptions.OmitDevice);
                }
            }
        }
        else if(deviceIndex == 1)
        {
            //KeyBoard
            for (int i = 0; i < 4; i++)
            {
                _KeyText[i].transform.parent.gameObject.SetActive(true);
                _KeyText[i].text = InputControlPath.ToHumanReadableString(
                    _myInput.InputActionDic[KeyType.LeftStick].bindings[deviceIndex + i + 1].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
            }
            for (int i = 4; i < _KeyText.Length; i++)
            {
                _KeyText[i].text = InputControlPath.ToHumanReadableString(
                    _inputActions[i].bindings[deviceIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
            }
        }
    }
    // void _UpdateKey(int index)
    // {
    //     _KeyText[index].text = SysSaveSC._Keys[index].ToString();
    // }
    public void _KeyChange(int index)
    {
        _myInput.PlayerActionOnOff(false);
        _myInput.UIActionOnOff(false);
        _KeyText[index].text = _WaitText[SysSaveSC._Language];
        string controllerType = _SelectedControlType == 0 ? "<Gamepad>" : "<Keyboard>";
        _rebindingOperation = _inputActions[index].PerformInteractiveRebinding()
            .WithControlsHavingToMatchPath(controllerType)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(_ => _KeyChangeComplete())
            .WithTargetBinding(_SelectedControlType)
            .Start();
        
        // if (_Waiting == false)
        // {
        //     _Waiting = true;
        //     _Target = index;
        //     _KeyText[index].text = _WaitText[SysSaveSC._Language];
        // }
        // else
        // {
        //     if (_Target == index)
        //     {
        //         _Waiting = false;
        //         //_UpdateKey(index);
        //     }
        //     else
        //     {
        //         //_UpdateKey(_Target);
        //         _Target = index;
        //         _KeyText[index].text = _WaitText[SysSaveSC._Language];
        //     }
        // }

    }

    private void _KeyChangeComplete()
    {
        if (_rebindingOperation != null)
        {
            _rebindingOperation.Dispose();
        }
        _UpdateKeys(_SelectedControlType);
        _myInput.PlayerActionOnOff(true);
        _myInput.UIActionOnOff(true);
    }
    public void _ResetDefault()
    {
        _myInput.PlsInputAction.RemoveAllBindingOverrides();
        _KeyChangeComplete();
        // KeyCode[] _DefaultKeys = new KeyCode[]
        // {
        //     KeyCode.UpArrow,
        //     KeyCode.DownArrow,
        //     KeyCode.LeftArrow,
        //     KeyCode.RightArrow,
        //     KeyCode.Tab,
        //     KeyCode.Z,
        //     KeyCode.X,
        //     KeyCode.C,
        //     KeyCode.F,
        //     KeyCode.Z,
        //     KeyCode.X,
        //     KeyCode.Escape,
        //     KeyCode.I,
        //     KeyCode.D,
        //     KeyCode.S
        // };
        // SysSaveSC._Keys = _DefaultKeys;
        // OnEnable();
    }
    // void OnGUI()
    // {
    //     if (_Waiting == true)
    //     {
    //         Event e = Event.current;
    //         if (e.isKey && e.type == EventType.KeyDown)
    //         {
    //             //Debug.Log("Detected key code: " + e.keyCode);
    //             _Waiting = false;
    //             SysSaveSC._Keys[_Target] = e.keyCode;
    //             //_UpdateKey(_Target);
    //         }
    //     }
    // }
}
