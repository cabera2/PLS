using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1SC : EnemyParentSC
{
    public bool _IsJumping;
    public float _Jumpforce;
    public float _Result;
    public GameObject _Bubble;
    public ParticleSystem[] _PS;
    public AudioClip[] _SFX;
    public LayerMask _GroundLayer;
    private bool _IsGrounded;
    private List<GameObject> _BBPool = new List<GameObject>();
    private EnemyCommonSC ECSC;
    private CircleCollider2D _CC;
    private AudioSource _AS;
    private bool _MakingBB;
    private Transform _LumiaTF;
    public Animator _FXAni;
    private int _JumpCount;
    private bool _BattleStart;
    [Header("AfterFinish")]
    public MeetBossSC _MBSC;
    public ShutterSC[] _SSC;
    private bool _Dead;

    // Start is called before the first frame update
    void Start()
    {
        base._Caching();
        ECSC = GetComponent<EnemyCommonSC>();
        _CC = GetComponent<CircleCollider2D>();
        _AS = GetComponent<AudioSource>();
        _Calculate();
        _LumiaTF = StageManagerSC._LumiaInst.transform;
        StartCoroutine(StartC());
    }
    IEnumerator StartC()
    {
        float _StartPos = transform.position.y;

        while (transform.position.y > _StartPos - 5)
        {
            yield return null;
        }
        _CC.isTrigger = false;
    }
    void _Calculate()
    {
        _Result = _Jumpforce / (-Physics2D.gravity.y * _RB.gravityScale) * 2;
    }
    void _NextPattern()
    {
        if (_JumpCount < 3)
        {
            StartCoroutine(_JumpC());
        }
        else
        {
            StartCoroutine(_BubblePhase());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_Common._HP <= 0 && _Dead == false)
        {
            _Dead = true;
            if (GetComponent<MapSaverSC>() != null)
            {
                GetComponent<MapSaverSC>()._SaveStatus();
            }
            StopAllCoroutines();
            StartCoroutine(_ClearEvent());
            StartCoroutine(_LastShake());
            _FXAni.SetTrigger("_Hit");
        }
        if (_IsGrounded == false && _CC.isTrigger == false && _Common._HP > 0)
        {
            _IsGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, _GroundLayer);
            if (_IsGrounded == true)
            {
                _AS.PlayOneShot(_SFX[2], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                _PS[0].Play();
                StageManagerSC._CamSC._Shake(0.5f);
                if (_BattleStart == false)
                {
                    StartCoroutine(_StartEvent());
                }
                else
                {
                    _Ani.SetBool("_IsJumping", false);
                    _IsJumping = false;
                    _NextPattern();
                }

            }
        }
    }
    IEnumerator _StartEvent()
    {
        StartCoroutine(StageManagerSC._lumiaSc._ChangeMusic(null));
        yield return new WaitForSeconds(2.0f);

        _AS.PlayOneShot(_SFX[1], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        _Ani.SetInteger("_BubblePhase", 2);
        _FXAni.SetBool("_Roar", true);
        StageManagerSC._CamSC._CoolDownLock = true;
        StageManagerSC._CamSC._Shake(0.5f);
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(StageManagerSC._lumiaSc._ChangeMusic(_SFX[0]));
        StageManagerSC._CamSC._CoolDownLock = false;
        _Ani.SetInteger("_BubblePhase", 0);
        _FXAni.SetBool("_Roar", false);
        StageManagerSC._lumiaSc._CanControl = true;
        _BattleStart = true;
        _NextPattern();
    }
    IEnumerator _JumpC()
    {
        _JumpCount += 1;
        yield return new WaitForSeconds(2f);
        float _YDistance = _LumiaTF.position.x - transform.position.x;
        _SR.flipX = _YDistance < 0 ? false : _YDistance > 0 ? true : false;
        float _XSpeed = _YDistance / _Result;
        _RB.velocity = Vector2.up * _Jumpforce;
        _Ani.SetBool("_IsJumping", true);
        _IsJumping = true;
        Invoke("Changebool", 0.1f);
        while (_IsJumping == true)
        {
            Vector2 _V2 = transform.position;
            if (_XSpeed < 0 && Physics2D.OverlapCircle(_V2 + Vector2.left * 1.4f, 0.1f, _GroundLayer) == true || _XSpeed > 0 && Physics2D.OverlapCircle(_V2 + Vector2.right * 1.4f, 0.1f, _GroundLayer) == true)
            {
                _IsJumping = false;
            }
            _RB.velocity = new Vector2(_XSpeed, _RB.velocity.y);
            yield return null;
        }
    }
    IEnumerator _BubblePhase()
    {
        _JumpCount = 0;
        yield return new WaitForSeconds(2f);
        float _YDistance = _LumiaTF.position.x - transform.position.x;
        _SR.flipX = _YDistance < 0 ? false : _YDistance > 0 ? true : false;
        _Ani.SetInteger("_BubblePhase", 1);
        yield return new WaitForSeconds(2f);
        _Ani.SetInteger("_BubblePhase", 2);
        _MakingBB = true;
        StartCoroutine(_MakeBubble());
        yield return new WaitForSeconds(3f);
        _MakingBB = false;
        _Ani.SetInteger("_BubblePhase", 0);
        _NextPattern();
    }
    IEnumerator _MakeBubble()
    {
        int _BossDir = _SR.flipX == true ? 1 : _SR.flipX == false ? -1 : -1;
        while (_MakingBB == true)
        {
            _AS.PlayOneShot(_SFX[3], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            int _BossDir2 = _BossDir;
            GameObject _BubbleInst = null;
            if (_BBPool.Count > 0)
            {
                _BubbleInst = _BBPool[0];
                _BubbleInst.SetActive(true);
                _BBPool.RemoveAt(0);
            }
            else
            {
                _BubbleInst = Instantiate(_Bubble);
                _BubbleInst.GetComponent<BubbleSC>()._BBPool = _BBPool;
            }
            Vector2 _V2 = transform.position;
            _BubbleInst.transform.position = _V2 + Vector2.up * 1.3f;
            _BossDir2 *= Random.Range(7, 60);
            Vector2 _BBDir = new Vector2(_BossDir2, Random.Range(-1f, 1f));
            _BubbleInst.GetComponent<Rigidbody2D>().velocity = _BBDir;
            yield return new WaitForSeconds(0.5f);
        }
    }
    void Changebool()
    {
        _IsGrounded = false;
    }
    IEnumerator _ClearEvent()
    {
        StartCoroutine(StageManagerSC._lumiaSc._ChangeMusic(null));
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(3f);
        if (_MBSC != null)
        {
            _MBSC._RevertCam();
        }

        for (int i = 0; i < _SSC.Length; i++)
        {
            _SSC[i]._Open();
        }
        StageManagerSC._lumiaSc._MusicCheck();
    }
    IEnumerator _LastShake()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        while (Physics2D.OverlapCircle(transform.position, 0.1f, _GroundLayer) == false)
        {
            yield return null;
        }
        _AS.PlayOneShot(_SFX[2], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        _PS[0].Play();
        StageManagerSC._CamSC._Shake(0.5f);
    }
}
