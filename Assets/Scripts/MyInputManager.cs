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

    public MyInputManager()
    {
        Initialize();
    }

    private void Initialize()
    {
        _inputAction = new();
        _inputActions.Add(KeyType.LeftStick, _inputAction.LumiaAction.LeftStick);
        _inputActions.Add(KeyType.Map, _inputAction.LumiaAction.Map);
        _inputActions.Add(KeyType.Jump, _inputAction.LumiaAction.Jump);
        _inputActions.Add(KeyType.Slash, _inputAction.LumiaAction.Slash);
        _inputActions.Add(KeyType.Shoot, _inputAction.LumiaAction.Shoot);
        _inputActions.Add(KeyType.Teleport, _inputAction.LumiaAction.Teleport);
        foreach (var item in _inputActions)
        {
            item.Value.Enable();
        }
    }
    public bool GetButton(KeyType keyType)
    {
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
                if (input.x < 0)
                {
                    output.x = -1;
                }
                else if (input.x > 0)
                {
                    output.x = 1;
                }
                if (input.y < -0.5f)
                {
                    output.y = -1;
                }
                else if (input.y > 0.5f)
                {
                    output.y = 1;
                }
                break;
            }
            case KeyType.RightStick:
            {
                // input = new Vector2(Input.GetAxisRaw("RightStickX"), Input.GetAxisRaw("RightStickY"));
                // if (input.x < -0)
                // {
                //     output.x = -1;
                // }
                // else if (input.x > 0)
                // {
                //     output.x = 1;
                // }
                // if (input.y < 0)
                // {
                //     output.y = -1;
                // }
                // else if (input.y > 0)
                // {
                //     output.y = 1;
                // }
                break;
            }
        }
        return output;
    }
}
