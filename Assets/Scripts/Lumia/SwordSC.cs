using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSC : MonoBehaviour
{
    private AudioSource _AS;
    public AudioClip _SFX;
    public GameObject _Lumia;
    private LumiaSC _LSC;
    private PlayerDetectorSC _PDSC;
    private StageManagerSC _SMSC;
    private Animator _Ani;
    private bool _Grounded;
    public GameObject _Particle;
    public AudioClip _SwordPickSFX;
    private Rigidbody2D _RB;
    public LayerMask _GroundLayer;
    public LayerMask _EnemyLayer;
    public float _Height;

    // Use this for initialization
    void Start()
    {
        _RB = GetComponent<Rigidbody2D>();
        _LSC = _Lumia.GetComponent<LumiaSC>();
        _AS = GetComponent<AudioSource>();
        _PDSC = transform.GetChild(0).GetComponent<PlayerDetectorSC>();
        _SMSC = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>();
        _Ani = GetComponent<Animator>();
    }
    void OnEnable()
    {
        _Grounded = false;
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (_RB.velocity == Vector2.zero && col.gameObject.layer == 8)
        {
            if (_PDSC._Grounded == false)
            {
                Invoke("_GroundedOn", Time.deltaTime);
                _LSC._SwordList.Add(gameObject);
                _LSC._SwordDistanceList.Add(Vector2.Distance(_Lumia.transform.position, transform.position));
                _PDSC._Grounded = true;
                if (col.gameObject.layer == 8 && col.gameObject.tag == "MovingPlatform")
                {
                    transform.parent = col.gameObject.transform.parent.transform;
                }
            }
            else if (_Grounded == true)
            {
                if (StageManagerSC._LumiaInst.transform.parent == gameObject.transform)
                {
                    StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._CancelHanging();
                }
                _DestroySword(true);
            }
        }
    }
    void _GroundedOn()
    {
        _Grounded = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 13)
        {
            _DestroySword(true);
        }
    }
    public void _Shake()
    {
        _AS.PlayOneShot(_SFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        StageManagerSC._CamSC._Shake(0.5f);
        Collider2D[] _Enemies = Physics2D.OverlapCircleAll(transform.position, 3f, _EnemyLayer);
        for (int i = 0; i < _Enemies.Length; i++)
        {
            _Enemies[i].gameObject.GetComponent<EnemyCommonSC>()._BoomHit(transform.position);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Gate")
        {
            _DestroySword(false);
        }
    }
    public void _DestroySword(bool _MakeParticle)
    {
        _SMSC._SwordPool.Add(gameObject);
        _PDSC._Grounded = false;
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        if (StageManagerSC._LumiaInst.transform.parent == transform)
        {
            StageManagerSC._lumiaSc._CancelHanging();
        }
        _RemoveFromList();
        if (_MakeParticle == true)
        {
            GameObject _ParticleInst;
            _ParticleInst = Instantiate(_Particle);
            _ParticleInst.transform.position = transform.position;
        }
        gameObject.SetActive(false);
    }
    public void _Boom()
    {
        _Ani.SetTrigger("_Boom");
    }
    public void _AfterBoom()
    {
        _DestroySword(false);
    }
    public void _RemoveFromList()
    {
        if (_LSC._SwordList.Contains(gameObject))
        {
            int _ListPos;
            _ListPos = _LSC._SwordList.IndexOf(gameObject);
            _LSC._SwordList.RemoveAt(_ListPos);
            _LSC._SwordDistanceList.RemoveAt(_ListPos);
        }
    }
}
