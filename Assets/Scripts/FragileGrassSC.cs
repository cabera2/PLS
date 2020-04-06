using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileGrassSC : MonoBehaviour
{
    private ParticleSystem _PS;
    private AudioSource _AS;
    private SpriteRenderer _SR;
    public AudioClip[] _SFX;
    public Sprite _Bottom;
    public GameObject _WallChild;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_StartC());
    }
    IEnumerator _StartC()
    {
        _PS = GetComponent<ParticleSystem>();
        _SR = _WallChild.GetComponent<SpriteRenderer>();
        while (StageManagerSC._LumiaInst == null)
        {
            yield return null;
        }
        _AS = StageManagerSC._LumiaInst.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Slash(Clone)")
        {
            _PS.Play();
            _AS.PlayOneShot(_SFX[Random.Range(0, 2)], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            if (GetComponent<MapSaverSC>() != null)
            {
                GetComponent<MapSaverSC>()._SaveStatus();
            }
            GetComponent<BoxCollider2D>().enabled = false;
            if (_Bottom == null)
            {
                _WallChild.SetActive(false);
            }
            else
            {
                _SR.sprite = _Bottom;
            }
        }
    }
}
