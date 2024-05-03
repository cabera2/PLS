using UnityEngine;
public enum KeyType
{
    Jump, Slash, Shoot, Teleport, Shield, Warp, Map
}
public class MyInputManager
{
    public bool GetButton(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Jump:
                return Input.GetButton("ButtonA") || Input.GetKey(SysSaveSC._Keys[5]);
            case KeyType.Shoot:
                return Input.GetButton("ButtonB") || Input.GetKey(SysSaveSC._Keys[7]);
            case KeyType.Shield:
                return Input.GetAxisRaw("Shield") >= 0.5f || Input.GetKey(SysSaveSC._Keys[14]);
            case KeyType.Warp:
                return Input.GetButton("Warp") || Input.GetKey(SysSaveSC._Keys[13]);
            case KeyType.Map:
                return Input.GetButton("Map") || Input.GetKey(SysSaveSC._Keys[4]);
            default:
                Debug.Log("Unknown GetButton");
                return false;
        }
    }
    public bool GetButtonDown(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Jump:
                return Input.GetButtonDown("ButtonA") || Input.GetKeyDown(SysSaveSC._Keys[5]);
            case KeyType.Slash:
                return Input.GetButtonDown("ButtonX") || Input.GetKeyDown(SysSaveSC._Keys[6]);
            case KeyType.Warp:
                return Input.GetButtonDown("Warp") || Input.GetKeyDown(SysSaveSC._Keys[13]);
            default:
                Debug.Log("Unknown GetButtonDown");
                return false;
        }
    }
    public bool GetButtonUp(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Jump:
                return Input.GetButtonUp("ButtonA") || Input.GetKeyUp(SysSaveSC._Keys[5]);
            case KeyType.Shoot:
                return Input.GetButtonUp("ButtonB") || Input.GetKeyUp(SysSaveSC._Keys[7]);
            case KeyType.Teleport:
                return Input.GetButtonUp("ButtonR1") || Input.GetKeyUp(SysSaveSC._Keys[8]);
            case KeyType.Warp:
                return Input.GetButtonUp("Warp") || Input.GetKeyUp(SysSaveSC._Keys[13]);
            default:
                Debug.Log("Unknown GetButtonUp");
                return false;
        }
    }
}
