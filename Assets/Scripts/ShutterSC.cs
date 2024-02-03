using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterSC : MonoBehaviour
{
    public Vector2 _OpenDir;
    public bool _Opened;
    public AudioClip[] _SFX;
    public Sprite[] _Sprites;
    private Vector3 _OpenPos;
    private bool _Shaking;
    private GameObject _ShutterChild;
    private SpriteRenderer _SR;
    public List<GameObject> _Swords = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {

        _ShutterChild = transform.GetChild(0).gameObject;
        _SR = _ShutterChild.GetComponent<SpriteRenderer>();
        Vector3 _OpenDirV3 = _OpenDir;
        _OpenPos = _ShutterChild.transform.position + _OpenDirV3;
        if (_Opened == true)
        {
            _ShutterChild.transform.position = _OpenPos;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public void _Open()
    {
        StartCoroutine(_OpenC());
    }
    IEnumerator _OpenC()
    {
        _Shaking = true;
        StageManagerSC._LumiaInst.GetComponent<AudioSource>().PlayOneShot(_SFX[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        StartCoroutine(_Shake());
        yield return new WaitForSeconds(1f);
        _Shaking = false;
        for (int i = 0; i < _Swords.Count; i++)
        {
            if (StageManagerSC._LumiaInst.transform.parent = _Swords[i].transform)
            {
                StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._CancelHanging();
            }
            _Swords[i].GetComponent<SwordSC>()._DestroySword(true);
        }
        GetComponent<BoxCollider2D>().enabled = false;
        _SR.sprite = _Sprites[1];
        while (transform.GetChild(0).position != _OpenPos)
        {
            transform.GetChild(0).position = Vector3.MoveTowards(transform.GetChild(0).position, _OpenPos, 1f);
            yield return null;
        }
        _SR.sprite = _Sprites[0];
    }
    IEnumerator _Shake()
    {
        GetComponent<ParticleSystem>().Play();
        while (_Shaking == true)
        {
            _ShutterChild.transform.localPosition = (new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).normalized * 0.05f;
            yield return null;
        }
        GetComponent<ParticleSystem>().Stop();
    }
    public void _Close()
    {
        StartCoroutine(_CloseC());
    }
    IEnumerator _CloseC()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        StageManagerSC._LumiaInst.GetComponent<AudioSource>().PlayOneShot(_SFX[1], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        _SR.sprite = _Sprites[1];
        while (transform.GetChild(0).position != transform.position)
        {
            transform.GetChild(0).position = Vector3.MoveTowards(transform.GetChild(0).position, transform.position, 1f);
            yield return null;
        }
        _SR.sprite = _Sprites[0];
        _Shaking = true;
        StartCoroutine(_Shake());
        yield return new WaitForSeconds(0.5f);
        _Shaking = false;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            _Swords.Add(col.gameObject);
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            _Swords.Remove(col.gameObject);
        }
    }
}
