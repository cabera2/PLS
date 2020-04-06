using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectorSC : MonoBehaviour
{
    public GameObject _Lumia;
    private Lumia_SC _LSC;
    public GameObject _Parent;
    public GameObject _Particle;
    public AudioClip _SwordPickSFX;
    public int _ListPos;
    public bool _Grounded;
    private bool _CanPick;

    // Use this for initialization
    void Start()
    {
        _Parent = transform.parent.gameObject;
        _Lumia = _Parent.GetComponent<SwordSC>()._Lumia;
        _LSC = _Lumia.GetComponent<Lumia_SC>();
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
            if (_LSC._IsWarp == false && _LSC._IsGrounded == false && _LSC._RB.constraints == RigidbodyConstraints2D.FreezeRotation && _Parent.transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                _LSC.ToSword();
            }
            else if (_LSC._IsWarp == true)
            {
                _LSC._IsWarp = false;
            }
            _Parent.GetComponent<SwordSC>()._RemoveFromList();
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
            _LSC._SwordStock += 1;
            _LSC._Canvas.GetComponent<PauseSC>()._UpdateSwordCurrent();
            _Parent.GetComponent<SwordSC>()._DestroySword(true);
        }

    }
}