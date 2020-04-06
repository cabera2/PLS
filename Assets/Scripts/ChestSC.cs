using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSC : MonoBehaviour
{
    public AudioClip _SFX;
    public Sprite _Opended;
    private Animator _Ani;
    private MapSaverSC _MSSC;
    private bool _OpenedB;
    // Start is called before the first frame update
    void Start()
    {

        if (GetComponent<MapSaverSC>() != null)
        {
            _MSSC = GetComponent<MapSaverSC>();
            if (_MSSC._Used == true)
            {
                _OpenedB = true;
                GetComponent<SpriteRenderer>().sprite = _Opended;
            }
            else
            {
                _Ani = GetComponent<Animator>();
                _Ani.enabled = true;
            }
        }
        else
        {
            _Ani = GetComponent<Animator>();
            _Ani.enabled = true;
        }



        if (GetComponent<MapSaverSC>() != null)
        {

        }
        else { }
        _Ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (_OpenedB == false && col.gameObject.name == "Slash(Clone)")
        {
            StageManagerSC._LumiaInst.GetComponent<AudioSource>().PlayOneShot(_SFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            _Ani.SetTrigger("_Opening");
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("_Flash");
            _OpenedB = true;
            StageManagerSC._CamSC._Shake(0.3f);
            if (GetComponent<MakeCoinSC>() != null)
            {
                GetComponent<MakeCoinSC>()._Make();
            }
            if (_MSSC != null)
            {
                _MSSC._SaveStatus();
            }
        }
    }
}
