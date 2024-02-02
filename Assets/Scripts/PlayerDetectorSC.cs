using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectorSC : MonoBehaviour
{
    public GameObject _Lumia;
    private LumiaSC _lumiaSc;
    private SwordSC _swordSc;
    public bool _Grounded;
    private bool _CanPick;

    // Use this for initialization
    void Start()
    {
        _swordSc = transform.parent.GetComponent<SwordSC>();
        _Lumia = _swordSc._Lumia;
        _lumiaSc = _Lumia.GetComponent<LumiaSC>();
    }
    void OnEnable()
    {
        _CanPick = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 && _Grounded == true)
        {
            Invoke("_CanPickOn", Time.deltaTime);
            if (_lumiaSc._IsWarp == false && _lumiaSc._IsGrounded == false && _lumiaSc._RB.constraints == RigidbodyConstraints2D.FreezeRotation && transform.parent.rotation != Quaternion.Euler(0, 0, 0))
            {
                _lumiaSc.ToSword();
            }
            else if (_lumiaSc._IsWarp == true)
            {
                _lumiaSc._IsWarp = false;
            }
            _swordSc._RemoveFromList();
        }
    }
    void _CanPickOn()
    {
        _CanPick = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 9 && _Grounded == true && _CanPick == true)
        {
            //_Lumia.GetComponent<AudioSource>().PlayOneShot(_SwordPickSFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            _lumiaSc._SwordStock += 1;
            _lumiaSc._Canvas.GetComponent<PauseSC>()._UpdateSwordCurrent();
            
            Debug.Log("Test1");
            _swordSc._DestroySword(true);
        }

    }
}