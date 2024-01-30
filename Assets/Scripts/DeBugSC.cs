using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeBugSC : MonoBehaviour
{
    private LumiaSC _LSC;
    private LumiaHitboxSC _LHBSC;
    private PauseSC _UISC;
    public Text[] _DebugText;

    // Start is called before the first frame update
    void Start()
    {
        _LSC = StageManagerSC._LSC;
        _LHBSC = _LSC._Hitbox.GetComponent<LumiaHitboxSC>();
        _UISC = transform.parent.GetComponent<PauseSC>();
        _NoDamageToggle(false);
        _NoHitToggle(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _NoDamageToggle(bool _Change)
    {
        if (_Change == true)
        {
            _LHBSC._NoDamage = !_LHBSC._NoDamage;
        }
        if (_LHBSC._NoDamage == true)
        {
            _DebugText[0].text = "No Damage: ON";
        }
        else
        {
            _DebugText[0].text = "No Damage: OFF";
        }
    }
    public void _NoHitToggle(bool _Change)
    {
        if (_Change == true)
        {
            _LHBSC._NoHit = !_LHBSC._NoHit;
        }
        if (_LHBSC._NoHit == true)
        {
            _DebugText[1].text = "No Hit: ON";
        }
        else
        {
            _DebugText[1].text = "No Hit: OFF";
        }
    }
    public void _ChangeHP(int _Value)
    {
        _LHBSC._HP_Max += _Value;
        _LHBSC._HP_Current = _LHBSC._HP_Max;
        _UISC._UpdateMaxHp();
        _UISC._UpdateCurrentHp();
    }
    public void _ChangeSword(int _Value)
    {
        _LSC._SwordMax += _Value;
        _LSC._SwordStock = _LSC._SwordMax;
        _UISC._UpdateSwordMax();
        _UISC._UpdateSwordCurrent();
    }
    public void _MakeMoney()
    {
        _LSC._Money = 1000000;
    }
    public void _PosReset()
    {
        StageManagerSC._LumiaInst.transform.position = _LSC._RespawnPoint;
        _LSC._RB.bodyType = RigidbodyType2D.Dynamic;
        _LSC._CanControl = true;
    }
}
