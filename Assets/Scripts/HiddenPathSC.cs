using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenPathSC : MonoBehaviour
{
    private AudioSource _AS;
    private SpriteRenderer _SR;
    private MapSaverSC _MSSC;
    public AudioClip _SFX;
    private bool _Found;
    // Start is called before the first frame update
    void Start()
    {
        _SR = GetComponent<SpriteRenderer>();
        if (GetComponent<MapSaverSC>() != null)
        {
            _MSSC = GetComponent<MapSaverSC>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Lumia" && _Found == false)
        {
            _AS = col.gameObject.GetComponent<AudioSource>();
            _Found = true;
            if (_MSSC == null || _MSSC != null && _MSSC._Used == false)
            {
                _AS.PlayOneShot(_SFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            }
            StartCoroutine(_FoundEvent());
        }
    }
    IEnumerator _FoundEvent()
    {
        Color tmpColor = _SR.color;
        while (_SR.color.a > 0)
        {
            tmpColor.a -= 0.1f;
            _SR.color = tmpColor;
            yield return null;
        }
    }
}
