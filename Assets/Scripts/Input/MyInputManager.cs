using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MyInputManager
{
    public PLS_InputActions PlsInputAction;
    public readonly Dictionary<KeyType, InputAction> InputActionDic = new() ;
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
        PlsInputAction = new();
        InputActionDic.Add(KeyType.LeftStick, PlsInputAction.LumiaActions.LeftStick);
        InputActionDic.Add(KeyType.RightStick, PlsInputAction.LumiaActions.RightStick);
        InputActionDic.Add(KeyType.Map, PlsInputAction.LumiaActions.Map);
        InputActionDic.Add(KeyType.Jump, PlsInputAction.LumiaActions.Jump);
        InputActionDic.Add(KeyType.Slash, PlsInputAction.LumiaActions.Slash);
        InputActionDic.Add(KeyType.Shoot, PlsInputAction.LumiaActions.Shoot);
        InputActionDic.Add(KeyType.Teleport, PlsInputAction.LumiaActions.Teleport);
        InputActionDic.Add(KeyType.Submit, PlsInputAction.UIActions.Submit);
        InputActionDic.Add(KeyType.Cancel, PlsInputAction.UIActions.Cancel);
        InputActionDic.Add(KeyType.Pause, PlsInputAction.UIActions.Pause);
        InputActionDic.Add(KeyType.Status, PlsInputAction.UIActions.Status);
        InputActionDic.Add(KeyType.Warp, PlsInputAction.LumiaActions.Warp);
        InputActionDic.Add(KeyType.Shield, PlsInputAction.LumiaActions.Shield);
        PlsInputAction.LumiaActions.Enable();
        PlsInputAction.UIActions.Enable();

    }
    public void PlayerActionOnOff(bool on)
    {
        if (on)
        {
            PlsInputAction.LumiaActions.Enable();
        }
        else
        {
            PlsInputAction.LumiaActions.Disable();
        }
    }

    public void UIActionOnOff(bool on)
    {
        if (on)
        {
            PlsInputAction.UIActions.Enable();
        }
        else
        {
            PlsInputAction.UIActions.Disable();
        }
    }
    public bool GetButton(KeyType keyType)
    {
        if (keyType == KeyType.Shield)
        {
            return InputActionDic[KeyType.Shield].ReadValue<float>() > 0.5f;
        }
        return InputActionDic.ContainsKey(keyType) && InputActionDic[keyType].IsPressed();

    }
    public bool GetButtonDown(KeyType keyType)
    {
        bool result = InputActionDic.ContainsKey(keyType) && InputActionDic[keyType].WasPressedThisFrame();
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
        return InputActionDic.ContainsKey(keyType) && InputActionDic[keyType].WasReleasedThisFrame();
    }
    public Vector2Int GetAxis(KeyType keyType)
    {
        Vector2 input;
        Vector2Int output = Vector2Int.zero;
        switch (keyType)
        {
            case KeyType.LeftStick:
            {
                input = InputActionDic[KeyType.LeftStick].ReadValue<Vector2>();
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
                
                input = InputActionDic[KeyType.RightStick].ReadValue<Vector2>();
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
