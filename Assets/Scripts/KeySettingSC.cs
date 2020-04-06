using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingSC : MonoBehaviour
{
    public Text[] _KeyText;
    [HideInInspector] public bool _Waiting;
    private int _Target;
    private string[] _WaitText = new string[]{
        "새 키 입력",
        "新しいキー入力"
    };
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnEnable()
    {
        for (int i = 0; i < _KeyText.Length; i++)
        {
            _UpdateKey(i);
        }
    }
    void _UpdateKey(int _ID)
    {
        _KeyText[_ID].text = SysSaveSC._Keys[_ID].ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _KeyChange(int _ID2)
    {
        if (_Waiting == false)
        {
            _Waiting = true;
            _Target = _ID2;
            _KeyText[_ID2].text = _WaitText[SysSaveSC._Language];
        }
        else
        {
            if (_Target == _ID2)
            {
                _Waiting = false;
                _UpdateKey(_ID2);
            }
            else
            {
                _UpdateKey(_Target);
                _Target = _ID2;
                _KeyText[_ID2].text = _WaitText[SysSaveSC._Language];
            }
        }

    }
    public void _ResetDefault()
    {
        KeyCode[] _DefaultKeys = new KeyCode[]
{
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.Tab,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.C,
        KeyCode.F,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.Escape,
        KeyCode.I,
        KeyCode.D,
        KeyCode.S
};
        SysSaveSC._Keys = _DefaultKeys;
        OnEnable();
    }
    void OnGUI()
    {
        if (_Waiting == true)
        {
            Event e = Event.current;
            if (e.isKey && e.type == EventType.KeyDown)
            {
                //Debug.Log("Detected key code: " + e.keyCode);
                _Waiting = false;
                SysSaveSC._Keys[_Target] = e.keyCode;
                _UpdateKey(_Target);
            }
        }
    }
}
