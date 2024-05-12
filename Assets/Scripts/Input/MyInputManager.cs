using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MyInputManager
{
    private PLS_InputActions _inputAction;
    public readonly Dictionary<KeyType, InputAction> InputActions = new() ;
    private static MyInputManager _staticMyInput;
    public Vector2 DeadZone = new (0.1f, 0.5f);

    public static MyInputManager GetMyInput()
    {
        if (_staticMyInput == null)
        {
            _staticMyInput = new MyInputManager();
            _staticMyInput.Initialize();
        }
        return _staticMyInput;
    }

    private void Initialize()
    {
        _inputAction = new();
        InputActions.Add(KeyType.LeftStick, _inputAction.LumiaActions.LeftStick);
        InputActions.Add(KeyType.RightStick, _inputAction.LumiaActions.RightStick);
        InputActions.Add(KeyType.Map, _inputAction.LumiaActions.Map);
        InputActions.Add(KeyType.Jump, _inputAction.LumiaActions.Jump);
        InputActions.Add(KeyType.Slash, _inputAction.LumiaActions.Slash);
        InputActions.Add(KeyType.Shoot, _inputAction.LumiaActions.Shoot);
        InputActions.Add(KeyType.Teleport, _inputAction.LumiaActions.Teleport);
        InputActions.Add(KeyType.Submit, _inputAction.UIActions.Submit);
        InputActions.Add(KeyType.Cancel, _inputAction.UIActions.Cancel);
        InputActions.Add(KeyType.Pause, _inputAction.UIActions.Pause);
        InputActions.Add(KeyType.Status, _inputAction.UIActions.Status);
        InputActions.Add(KeyType.Warp, _inputAction.LumiaActions.Warp);
        InputActions.Add(KeyType.Shield, _inputAction.LumiaActions.Shield);
        _inputAction.LumiaActions.Enable();
        _inputAction.UIActions.Enable();

    }
    public void PlayerActionOnOff(bool on)
    {
        if (on)
        {
            _inputAction.LumiaActions.Enable();
        }
        else
        {
            _inputAction.LumiaActions.Disable();
        }
    }
    public bool GetButton(KeyType keyType)
    {
        if (keyType == KeyType.Shield)
        {
            return InputActions[KeyType.Shield].ReadValue<float>() > 0.5f;
        }
        return InputActions.ContainsKey(keyType) && InputActions[keyType].IsPressed();

    }
    public bool GetButtonDown(KeyType keyType)
    {
        bool result = InputActions.ContainsKey(keyType) && InputActions[keyType].WasPressedThisFrame();
        // if (result)
        // {
        //     InputSystem.onAnyButtonPress.Call(DetectDevice);
        // }
        return result;
    }

    private void DetectDevice(InputControl control)
    {
        if (control.device is Keyboard)
        {
            Debug.Log("This is Keyboard");
        }
        else if (control.device is Gamepad)
        {
            Debug.Log("This is Gamepad");
        }
    }
    public bool GetButtonUp(KeyType keyType)
    {
        return InputActions.ContainsKey(keyType) && InputActions[keyType].WasReleasedThisFrame();
    }
    public Vector2Int GetAxis(KeyType keyType)
    {
        Vector2 input;
        Vector2Int output = Vector2Int.zero;
        switch (keyType)
        {
            case KeyType.LeftStick:
            {
                input = InputActions[KeyType.LeftStick].ReadValue<Vector2>();
                if (input.x < -DeadZone.x)
                {
                    output.x = -1;
                }
                else if (input.x > DeadZone.x)
                {
                    output.x = 1;
                }
                if (input.y < -DeadZone.y)
                {
                    output.y = -1;
                }
                else if (input.y > DeadZone.y)
                {
                    output.y = 1;
                }
                break;
            }
            case KeyType.RightStick:
            {
                
                input = InputActions[KeyType.RightStick].ReadValue<Vector2>();
                if (input.x < -DeadZone.x)
                {
                    output.x = -1;
                }
                else if (input.x > DeadZone.x)
                {
                    output.x = 1;
                }
                if (input.y < -DeadZone.y)
                {
                    output.y = -1;
                }
                else if (input.y > DeadZone.y)
                {
                    output.y = 1;
                }
                break;
            }
        }
        return output;
    }
}
