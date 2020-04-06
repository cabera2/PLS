using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSC : MonoBehaviour
{
    private Animator _Ani;
    private MapSaverSC _MSSC;
    public bool _On;
    public AudioClip _SFX;
    public GameObject[] _Target;
    public Sprite _OnSprite;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MapSaverSC>() != null)
        {
            _MSSC = GetComponent<MapSaverSC>();
            if (_MSSC._Used == true)
            {
                _On = true;
                GetComponent<SpriteRenderer>().sprite = _OnSprite;
            }
            else
            {
                _Ani = GetComponent<Animator>();
                _Ani.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.name == "PlayerDetector" && col.GetComponent<PlayerDetectorSC>()._Grounded == false) || col.gameObject.name == "Slash(Clone)")
        {
            col.GetComponent<HitFXSC>()._GenFX();
            StageManagerSC._LumiaInst.GetComponent<AudioSource>().PlayOneShot(_SFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            if (_On == false)
            {
                _On = true;
                StageManagerSC._CamSC._Shake(0.3f);
                if (_MSSC != null)
                {
                    _MSSC._SaveStatus();
                }
                _Ani.SetBool("_On", true);
                for (int i = 0; i < _Target.Length; i++)
                {
                    if (_Target[i].GetComponent<ShutterSC>() != null)
                    {
                        _Target[i].GetComponent<ShutterSC>()._Open();
                    }
                    else if (_Target[i].GetComponent<MovingPlatformSC>() != null)
                    {
                        _Target[i].GetComponent<MovingPlatformSC>()._TurnOn();
                    }
                }
            }
            else
            {
                if (GetComponent<Animator>().enabled == false)
                {
                    _Ani = GetComponent<Animator>();
                    _Ani.enabled = true;
                }
                _Ani.SetTrigger("_Shake");
            }
        }
    }
}
