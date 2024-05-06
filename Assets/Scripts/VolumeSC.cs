using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeSC : MonoBehaviour
{
    public bool _CanControl;
    public int _Type;
    private float _Timer;
    public string[] _Text;
    public AudioClip _TestSFX;
    public Text _NumTextObj;
    public RectTransform _GaugeObj;
    public Animator[] _ArrowAni;
    private int _Value;
    private MyInputManager _myInput = new();

    // Start is called before the first frame update
    void Start()
    {
        _Value = _Type == 0 ? SysSaveSC._Vol_Master : _Type == 1 ? SysSaveSC._Vol_BGM : _Type == 2 ? SysSaveSC._Vol_SFX : 10;
        _NumTextObj.text = _Value.ToString();
        _GaugeObj.sizeDelta = new Vector2(20 * _Value, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (_CanControl)
        {
            int xInput = _myInput.GetAxis(KeyType.LeftStick).x;
            if (xInput != 0)
            {
                if (_Timer == 0)
                {
                    if (xInput > 0 && _Value < 10)
                    {
                        _ValueChange(1);
                    }
                    else if (xInput < 0 && _Value > 0)
                    {
                        _ValueChange(-1);
                    }
                }
                _Timer += Time.unscaledDeltaTime;
                if (_Timer >= 0.2f)
                {
                    _Timer = 0;
                }
            }
            else
            {
                _Timer = 0;
            }
        }
    }
    public void _On()
    {
        _CanControl = true;
    }
    public void _Off()
    {
        _CanControl = false;
    }
    public void _ValueChange(int _Amount)
    {
        Debug.Log(gameObject + "" + _Timer);
        _ArrowAni[_Amount < 0 ? 0 : _Amount > 0 ? 1 : 0].SetTrigger("_Clicked");
        if (_Amount > 0 && _Value < 10 || _Amount < 0 && _Value > 0)
        {
            _Value += _Amount;
            _NumTextObj.text = _Value.ToString();
            _GaugeObj.sizeDelta = new Vector2(20 * _Value, 10);
            if (_Type == 0)
            {
                SysSaveSC._Vol_Master = _Value;
            }
            else if (_Type == 1)
            {
                SysSaveSC._Vol_BGM = _Value;
            }
            else if (_Type == 2)
            {
                SysSaveSC._Vol_SFX = _Value;
            }
            if (_Type <= 1)
            {
                if (StageManagerSC._lumiaSc != null)
                {
                    StageManagerSC._lumiaSc._Hitbox.GetComponent<AudioSource>().volume = SysSaveSC._Vol_Master * SysSaveSC._Vol_BGM * 0.01f;
                }
            }
            else
            {
                if (StageManagerSC._LumiaInst != null && StageManagerSC._LumiaInst.activeSelf == true)
                {
                    StageManagerSC._LumiaInst.GetComponent<AudioSource>().PlayOneShot(_TestSFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                }
                else
                {
                    GameObject _Lumia = GameObject.FindGameObjectWithTag("Lumia");
                    _Lumia.GetComponent<AudioSource>().PlayOneShot(_TestSFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                }
            }
        }
    }
}
