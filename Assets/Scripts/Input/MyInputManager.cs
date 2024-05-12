using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum KeyType
{
    Map, Jump, Slash, Shoot, Teleport, Submit, Cancel, Pause, Status, Warp, Shield, LeftStick, RightStick
}
public class MyInputManager
{
    private PLS_InputActions _inputAction;
    private readonly Dictionary<KeyType, InputAction> _inputActions = new() ;
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
        _inputActions.Add(KeyType.LeftStick, _inputAction.LumiaActions.LeftStick);
        _inputActions.Add(KeyType.RightStick, _inputAction.LumiaActions.RightStick);
        _inputActions.Add(KeyType.Map, _inputAction.LumiaActions.Map);
        _inputActions.Add(KeyType.Jump, _inputAction.LumiaActions.Jump);
        _inputActions.Add(KeyType.Slash, _inputAction.LumiaActions.Slash);
        _inputActions.Add(KeyType.Shoot, _inputAction.LumiaActions.Shoot);
        _inputActions.Add(KeyType.Teleport, _inputAction.LumiaActions.Teleport);
        _inputActions.Add(KeyType.Submit, _inputAction.UIActions.Submit);
        _inputActions.Add(KeyType.Cancel, _inputAction.UIActions.Cancel);
        _inputActions.Add(KeyType.Pause, _inputAction.UIActions.Pause);
        _inputActions.Add(KeyType.Status, _inputAction.UIActions.Status);
        _inputActions.Add(KeyType.Warp, _inputAction.LumiaActions.Warp);
        _inputActions.Add(KeyType.Shield, _inputAction.LumiaActions.Shield);
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
            return _inputActions[KeyType.Shield].ReadValue<float>() > 0.5f;
        }
        return _inputActions.ContainsKey(keyType) && _inputActions[keyType].IsPressed();

    }
    public bool GetButtonDown(KeyType keyType)
    {
        return _inputActions.ContainsKey(keyType) && _inputActions[keyType].WasPressedThisFrame();
    }
    public bool GetButtonUp(KeyType keyType)
    {
        return _inputActions.ContainsKey(keyType) && _inputActions[keyType].WasReleasedThisFrame();
    }
    public Vector2Int GetAxis(KeyType keyType)
    {
        Vector2 input;
        Vector2Int output = Vector2Int.zero;
        switch (keyType)
        {
            case KeyType.LeftStick:
            {
                input = _inputActions[KeyType.LeftStick].ReadValue<Vector2>();
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
                
                input = _inputActions[KeyType.RightStick].ReadValue<Vector2>();
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
