using System.Collections.Generic;
using UnityEngine;
public enum KeyType
{
    Map, Jump, Slash, Shoot, Teleport, Submit, Cancel, Pause, Status, Warp, Shield
}
public class MyInputManager
{
    public bool GetButton(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Map:
                return Input.GetButton("Map") || Input.GetKey(SysSaveSC._Keys[4]);
            case KeyType.Jump:
                return Input.GetButton("ButtonA") || Input.GetKey(SysSaveSC._Keys[5]);
            case KeyType.Shoot:
                return Input.GetButton("ButtonB") || Input.GetKey(SysSaveSC._Keys[7]);
            case KeyType.Warp:
                return Input.GetButton("Warp") || Input.GetKey(SysSaveSC._Keys[13]);
            case KeyType.Shield:
                return Input.GetAxisRaw("Shield") >= 0.5f || Input.GetKey(SysSaveSC._Keys[14]);
            default:
                //Debug.Log("Unknown GetButton");
                return false;
        }
    }
    public bool GetButtonDown(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Map:
                return Input.GetButtonDown("Map") || Input.GetKeyDown(SysSaveSC._Keys[4]);
            case KeyType.Jump:
                return Input.GetButtonDown("ButtonA") || Input.GetKeyDown(SysSaveSC._Keys[5]);
            case KeyType.Slash:
                return Input.GetButtonDown("ButtonX") || Input.GetKeyDown(SysSaveSC._Keys[6]);
            case KeyType.Submit:
                return Input.GetButtonDown("Submit") || Input.GetKeyDown(SysSaveSC._Keys[9]);
            case KeyType.Cancel:
                return Input.GetButtonDown("Cancel") || Input.GetKeyDown(SysSaveSC._Keys[10]);
            case KeyType.Warp:
                return Input.GetButtonDown("Warp") || Input.GetKeyDown(SysSaveSC._Keys[13]);
            default:
                //Debug.Log("Unknown GetButtonDown");
                return false;
        }
    }
    public bool GetButtonUp(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Map:
                return Input.GetButtonUp("Map") || Input.GetKeyUp(SysSaveSC._Keys[4]);
            case KeyType.Jump:
                return Input.GetButtonUp("ButtonA") || Input.GetKeyUp(SysSaveSC._Keys[5]);
            case KeyType.Shoot:
                return Input.GetButtonUp("ButtonB") || Input.GetKeyUp(SysSaveSC._Keys[7]);
            case KeyType.Teleport:
                return Input.GetButtonUp("ButtonR1") || Input.GetKeyUp(SysSaveSC._Keys[8]);
            case KeyType.Pause:
                return Input.GetButtonUp("Pause") || Input.GetKeyUp(SysSaveSC._Keys[11]);
            case KeyType.Status:
                return Input.GetButtonUp("Status") || Input.GetKeyUp(SysSaveSC._Keys[12]);
            default:
                //Debug.Log("Unknown GetButtonUp");
                return false;
        }
    }
}