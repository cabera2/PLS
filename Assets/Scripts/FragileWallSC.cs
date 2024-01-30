using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileWallSC : MonoBehaviour
{
    public int _HitCount;
    private ParticleSystem _PS;
    private AudioSource _AS;
    private SpriteRenderer _SR;
    public AudioClip[] _SFX;
    public Sprite[] _FragSpr;
    public Sprite _Bottom;
    public GameObject _FrageObj;
    public GameObject _WallChild;
    public int _FragCount;
    private List<GameObject> _Swords = new List<GameObject>();
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
    IEnumerator _Shake()
    {
        float _ShakeAmount = 0.1f;
        while (_ShakeAmount > 0)
        {
            _ShakeAmount -= 0.01f;
            yield return null;
            _WallChild.transform.localPosition = (new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).normalized * _ShakeAmount;
        }
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
            if (_HitCount > 1)
            {
                _HitCount -= 1;
                StartCoroutine(_Shake());
                _AS.PlayOneShot(_SFX[Random.Range(0, 1)], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            }
            else
            {
                for (int i = 0; i < _Swords.Count; i++)
                {
                    if (StageManagerSC._LumiaInst.transform.parent = _Swords[i].transform)
                    {
                        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._CancelHanging();
                    }
                    _Swords[i].GetComponent<SwordSC>()._DestroySword(true);
                }
                _AS.PlayOneShot(_SFX[2], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                if (GetComponent<MapSaverSC>() != null)
                {
                    GetComponent<MapSaverSC>()._SaveStatus();
                }
                StageManagerSC._CamSC._Shake(0.2f);
                GetComponent<BoxCollider2D>().enabled = false;
                if (_Bottom == null)
                {
                    _WallChild.SetActive(false);
                }
                else
                {
                    _SR.sprite = _Bottom;
                }
                for (int i = 0; i < _FragCount; i++)
                {
                    GameObject FragInst;
                    FragInst = Instantiate(_FrageObj);
                    FragInst.GetComponent<SpriteRenderer>().sprite = _FragSpr[Random.Range(0, _FragSpr.Length)];
                    FragInst.AddComponent(typeof(PolygonCollider2D));
                    FragInst.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-2f, 2f), 0);
                    float _Dir = transform.position.x - col.transform.position.x;
                    float _Force = _Dir > 0 ? 300 : _Dir < 0 ? -300 : 0;
                    FragInst.GetComponent<Rigidbody2D>().AddForce(new Vector2(_Force, 0f));
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            _Swords.Add(col.gameObject);
        }
    }
}
