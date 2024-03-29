﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public partial class LumiaSC : MonoBehaviour
{
    [Header("Stats")]
    public int _FileNumber;
    public string _SavedScene;
    public float _PlayTime;
    public int _SwordMax;
    public int _Money;
    public bool _HaveVessel;
    public LevelData levelData = new();

    [Header("Etc")]
    public AudioClip[] sfx;
    public Rigidbody2D _RB;
    public GameObject _Canvas;
    public StageManagerSC _stageManagerSc;
    public ParticleSystem _PS;
    private const string SHADER_COLOR_NAME = "_Color";
    private Material material;
    public float _SR_Bright;
    [Header("Movement")]
    public bool _CanControl;
    public bool _SmoothMove;
    public float _DefaultWalkSpeed;
    private float _CurrentWalkSpeed;
    public float _MaxSpeed;
    public float _JumpForce;
    [HideInInspector] public float _MoveInput;
    [HideInInspector] public float _UpDownInput;
    public float _CoyoteTime;
    private float _CoyoteTimer;

    public bool _IsGrounded;
    public LayerMask _GroundLayer;

    private float _JumpTimeCounter;
    public float _JumpMaxTime;
    private bool _IsJumping;
    private int _JumpCountCounter;
    public int _JumpMaxCount;
    public float _MinHightForHang;
    private float _DefaultGravity;
    public bool _AutoGlide;
    [Header("Auto Control")]
    public bool _AutoJumping;
    public float _AutoWalk;
    private GameObject _LastPlatform;
    private bool _PassingGate;


    [Header("Damage Control")]
    public GameObject _Hitbox;
    public float _InvincibleTime;
    public float _InvincibleTimer;
    public float _BlinkSpeed;
    private float _BlinkColor = 1;
    private bool _BlinkGetDark;
    public float _FrquencySpeed;
    public Vector2 _RespawnPoint;
    [HideInInspector] public float _KnockbackCounter;
    [HideInInspector] public bool _ChairRespawn;

    [Header("Sword Control")]
    public GameObject _SwordObj;
    public GameObject _SlashObj;
    public GameObject _SwordHanger;
    public GameObject _TargetMark;
    private Animator _TarAni;
    private SpriteRenderer _TarSR;
    public GameObject _DustBurstObj;
    public GameObject[] _Particle;
    public GameObject[] _BackSwords;
    public float _ParticleGap;
    public int _SwordStock;
    public float _SwordSpeed;
    public float _TargetMarkSpeed;
    public float _ShotRebound;
    private int _NearestSword;
    private int _NearestSword_Old;
    private bool _InRange;
    private float _AtkTimer;
    public float _WarpDistance;
    [HideInInspector] public List<GameObject> _SwordList = new List<GameObject>();
    [HideInInspector] public List<float> _SwordDistanceList = new List<float>();
    private Vector2 _ShootVector;
    public bool _IsWarp;
    [Header("Reload Control")]
    public float _ReloadTime;
    public float _ShootTime;
    public float _ReloadTimer;
    private bool _IsReloading;
    public float _StartReloadTime;
    private float WhiteAlpha;
    public float _ColorFadeSpeed;
    public ParticleSystem _ReloadParticle;
    private bool _reloadEffectPlaying;

    [Header("CameraMovement")]
    [HideInInspector] public GameObject _MyCamera;
    private float _LookUpDownTimer;
    public float _LookUpDownDistance;

    [Header("Gate Control")]
    public static string _CurrentScene;
    public int _GateNumber;
    public float _MusicFadeSpeed;
    private bool _JumpGate;
    private float _JumpGateDir;

    [Header("Warp Control")]
    public Animator _WarpSwordHolder;
    public GameObject _PortalMark;
    public GameObject _CurrentPortal;
    public string _WarpScene;
    public Vector2 _WarpPos;
    private float _WarpTimer;
    private bool _WarpChargeSet;
    private bool _WarpChargeMove;
    private bool _PassingPortal;
    public Transform _Shield;
    [Header("Flags")]
    public List<int> _PermanentFlag = new List<int>();
    public List<int> _TemporaryFlag = new List<int>();

    // Use this for initialization
    void Start()
    {
        //Debug.Log(Input.GetJoystickNames()[0] + "연결");
        CacheComponents();
        StartCoroutine(StartC());
        _CurrentWalkSpeed = _DefaultWalkSpeed;
        _DefaultGravity = _RB.gravityScale;
        material = _mainSpriteRenderer.material;
        _bgmPlayer.clip = _MyCamera.GetComponent<StageManagerSC>()._StageBGM;
    }
    void CacheComponents()
    {
        _mainSpriteRenderer = GetComponent<SpriteRenderer>();
        _glowSpriteRenderer = _Hitbox.GetComponent<SpriteRenderer>();
        _mainAnimator = GetComponent<Animator>();
        _mainAudioSource = GetComponent<AudioSource>();
        _bgmPlayer = _Hitbox.GetComponent<AudioSource>();
        _Canvas.GetComponent<Canvas>().worldCamera = _MyCamera.GetComponent<Camera>();
        _stageManagerSc = _MyCamera.GetComponent<StageManagerSC>();
        _reloadAudioSource = _SwordHanger.GetComponent<AudioSource>();
        _lumiaCamSc = GetComponent<LumiaCamSC>();
    }
    IEnumerator StartC()
    {
        while (_TargetMark == null)
        {
            yield return null;
        }
        DontDestroyOnLoad(_TargetMark);
        _TarAni = _TargetMark.GetComponent<Animator>();
        _TarSR = _TargetMark.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateBackSwords();
        Blink();
        CheckGrounded();
        Move();
        _Canvas.GetComponent<PauseSC>()._MoneyText.text = _Money.ToString();
        ParticleSystem.EmissionModule _em = _PS.emission;
        if (_IsGrounded == false)
        {
            _em.rateOverDistanceMultiplier -= 15 * Time.deltaTime;
        }
        else if (_IsGrounded == true)
        {
            _em.rateOverDistanceMultiplier = 3;
        }

        //Debug.Log("파티클 발생량: " + _em.rateOverDistanceMultiplier);
        if (_CanControl == true)
        {
            _mainAnimator.SetFloat(AniXInputAbs, Mathf.Abs(_MoveInput));
        }
        else
        {
            _mainAnimator.SetFloat(AniXInputAbs, Mathf.Abs(_AutoWalk));
        }
        //_mainAnimator.SetFloat("_YInput", _UpDownInput);
        _mainAnimator.SetFloat(AniYVelocity, _RB.velocity.y);
        _mainAnimator.SetBool(AniIsGrounded, _IsGrounded);
        if (_CanControl == true)
        {
            if (_MoveInput > 0)
            {
                _mainSpriteRenderer.flipX = false;
            }
            else if (_MoveInput < 0)
            {
                _mainSpriteRenderer.flipX = true;
            }
        }
        else
        {
            if (_AutoWalk > 0)
            {
                _mainSpriteRenderer.flipX = false;
            }
            else if (_AutoWalk < 0)
            {
                _mainSpriteRenderer.flipX = true;
            }
        }
        _glowSpriteRenderer.flipX = _mainSpriteRenderer.flipX;

        //공격 딜레이 타이머
        if (_AtkTimer > 0)
        {
            _AtkTimer -= Time.deltaTime;
        }

        //활공 중지
        if (_RB.gravityScale != _DefaultGravity && ((Input.GetButtonUp("ButtonA") || Input.GetKeyUp(SysSaveSC._Keys[5])) || _CanControl == false || _IsGrounded == true || _RB.constraints == RigidbodyConstraints2D.FreezeAll))
        {
            _mainAnimator.SetBool(AniIsGliding, false);
            _RB.gravityScale = _DefaultGravity;
        }

        //의자 앉을 때/설 때 등의 검 위치 조정
        if (_mainAnimator.GetBool(AniIsSitting) == false)
        {
            _SwordHanger.transform.localPosition = new Vector2(_mainSpriteRenderer.flipX == true ? 0.3f : _mainSpriteRenderer.flipX == false ? -0.3f : 0, 0);
            _Shield.localPosition = new Vector2(_mainSpriteRenderer.flipX == true ? -0.8f : _mainSpriteRenderer.flipX == false ? 0.8f : 0, 0.7f);
        }
        else
        {
            _SwordHanger.transform.localPosition = new Vector2(0, 0.1f);
        }
        FindNearestSword();

        float _RSY = Input.GetAxisRaw("RightStickY");
        if (Input.GetButton("Warp") == false && Input.GetKey(SysSaveSC._Keys[13]) == false)
        {
            if (_CanControl == false || _IsGrounded == false || _MoveInput != 0)
            {
                _LookUpDownTimer = 0;
                _lumiaCamSc._LookUpDown = 0;
            }
            else
            {
                if (_UpDownInput == 0)
                {
                    _LookUpDownTimer = 0;
                }
                int _LUDR;
                _LUDR = _RSY > 0 ? 1 : _RSY < 0 ? -1 : 0;

                int LUDL = 0;
                if (_LookUpDownTimer < 0.5f)
                {
                    if (Mathf.Abs(_UpDownInput) > 0.5)
                    {
                        _LookUpDownTimer += Time.deltaTime;
                    }
                }
                if (_LookUpDownTimer >= 0.5f)
                {
                    float b = _UpDownInput;
                    LUDL = b > 0 ? 1 : b < 0 ? -1 : 0;
                }
                int LUDS = LUDL + _LUDR;
                int LUDS2 = LUDS > 0 ? 1 : LUDS < 0 ? -1 : 0;
                _lumiaCamSc._LookUpDown = Mathf.MoveTowards(_lumiaCamSc._LookUpDown, LUDS2 * _LookUpDownDistance, Time.deltaTime * 30);
            }

        }
        else
        {
            _LookUpDownTimer = 0;
        }
        
        if (_CanControl == true && Input.GetButton("Map") == false && Input.GetKey(SysSaveSC._Keys[4]) == false && _KnockbackCounter <= 0)
        {
            //Reloading
            if (_IsGrounded == true && _RB.constraints == RigidbodyConstraints2D.FreezeRotation)
            {
                if ((Input.GetButtonDown("ButtonB") || Input.GetKeyDown(SysSaveSC._Keys[7])) && _IsReloading == false && _ReloadTimer <= 0 && _SwordStock < _SwordMax)
                {
                    //_reloadAudioSource.Play();
                    _IsReloading = true;
                    _ReloadTimer += Time.deltaTime;
                }
                if ((Input.GetButton("ButtonB") || Input.GetKey(SysSaveSC._Keys[7])) && _IsReloading == true)
                {
                    _ReloadTimer += Time.deltaTime;
                    if (_ReloadTimer >= _StartReloadTime && _IsReloading == true)
                    {
                        if (_reloadEffectPlaying == false)
                        {
                            ReloadingEffect(true);
                        }
                        if (_ReloadTimer >= _ReloadTime)
                        {
                            if (_reloadEffectPlaying == true)
                            {
                                ReloadingEffect(false);
                                _IsReloading = false;
                            }

                            _reloadAudioSource.Stop();
                            StartCoroutine(_Reload(true));
                            StartCoroutine(_WhiteFlash());
                            StartCoroutine(_ReloadSwordAni());
                        }
                    }
                }
            }
            else
            {
                _IsReloading = false;
                _ReloadTimer = 0;
            }
            //Warp Charge
            if (_KnockbackCounter == 0 && _IsGrounded == true && _SwordStock >= 5)
            {
                if (Mathf.Abs(_UpDownInput) > 0.5f && (Input.GetButtonDown("Warp") || Input.GetKeyDown(SysSaveSC._Keys[13])))
                {
                    _WarpSwordHolder.SetBool("_Down", false);
                    if (_UpDownInput > 0.5f && string.IsNullOrEmpty(_WarpScene) == false)
                    {
                        _reloadAudioSource.Play();
                        _WarpChargeMove = true;
                        _mainAnimator.SetBool(AniIsWarpMoving, true);
                        _ReloadParticle.Play();
                    }
                    else if (_UpDownInput < 0.5f)
                    {
                        _reloadAudioSource.Play();
                        _WarpChargeSet = true;
                        _mainAnimator.SetBool(AniIsWarpSetting, true);
                        _ReloadParticle.Play();
                    }
                }

                if ((_WarpChargeMove == true || _WarpChargeSet == true) && (Input.GetButton("Warp") || Input.GetKey(SysSaveSC._Keys[13])) && _KnockbackCounter == 0 && _IsGrounded == true)
                {
                    _WarpTimer += Time.deltaTime;
                }
                else
                {
                    _WarpTimer = 0;
                    _WarpChargeMove = false;
                    _WarpChargeSet = false;
                    _mainAnimator.SetBool(AniIsWarpMoving, false);
                    _mainAnimator.SetBool(AniIsWarpSetting, false);
                    if (_IsReloading == false)
                    {
                        _ReloadParticle.Stop();
                    }
                }
                if (_WarpChargeSet == true && _WarpTimer >= 1.4f)
                {
                    _reloadAudioSource.Stop();
                    StartCoroutine(_WhiteFlash());
                    _WarpSwordHolder.SetBool("_Down", true);
                    _WarpScene = SceneManager.GetActiveScene().name;
                    _WarpPos = transform.position;
                    _SetPortal();
                    _WarpChargeSet = false;
                }
                else if (_WarpChargeMove == true && _WarpTimer >= 2.25f)
                {
                    _CanControl = false;
                    _reloadAudioSource.Stop();
                    _WarpSwordHolder.SetBool("_Down", true);
                    if (_Canvas.GetComponent<PauseSC>()._FadeObj.activeSelf == false)
                    {
                        _Canvas.GetComponent<PauseSC>()._FadeObj.SetActive(true);
                    }
                    _PassingPortal = true;
                    StartCoroutine(_LoadScene(_WarpScene, true));
                    _WarpChargeMove = false;
                }
            }

            //Shield
            if ((Input.GetAxisRaw("Shield") >= 0.5f || Input.GetKey(SysSaveSC._Keys[14])) && _SwordStock >= 2 && _IsGrounded == true)
            {
                _mainAnimator.SetBool(AniIsShielding, true);
            }
            else
            {
                _mainAnimator.SetBool(AniIsShielding, false);
            }
            //SwordShot
            if ((Input.GetButtonUp("ButtonB") || Input.GetKeyUp(SysSaveSC._Keys[7])) && Time.timeScale > 0 && _AtkTimer <= 0)
            {
                Shot();
            }

            //When not Reloading
            if ((_ReloadTimer < _StartReloadTime || _ReloadTimer >= _ReloadTime || _IsReloading == false) && Time.timeScale > 0 && Input.GetButton("Warp") == false && Input.GetKey(SysSaveSC._Keys[13]) == false && _mainAnimator.GetBool(AniIsShielding) == false)
            {
                //Glide
                if (_SwordStock >= 3 && _IsGrounded == false && _RB.constraints == RigidbodyConstraints2D.FreezeRotation)
                {
                    if ((Input.GetButtonDown("ButtonA") || Input.GetKeyDown(SysSaveSC._Keys[5])) && _AutoGlide == false)
                    {
                        _mainAnimator.SetBool(AniIsGliding, true);
                        _RB.velocity = Vector2.up * 0f;
                        _RB.gravityScale = 0.5f;
                    }
                    else if ((Input.GetButton("ButtonA") || Input.GetKey(SysSaveSC._Keys[5])) && _AutoGlide == true && _RB.velocity.y < 0)
                    {
                        _mainAnimator.SetBool(AniIsGliding, true);
                        _RB.gravityScale = 0.5f;
                    }
                }

                //GroundPass
                if ((Input.GetButtonDown("ButtonA") || Input.GetKeyDown(SysSaveSC._Keys[5])))
                {
                    _CoyoteTimer = _CoyoteTime;
                    if (_IsGrounded == true && _UpDownInput <= -0.5f)
                    {
                        PlatformEffector2D _PassGround = FindObjectOfType<PlatformEffector2D>();
                        if (_PassGround != null)
                        {
                            _PassGround.colliderMask = 64823;
                            CheckGrounded();
                            StartCoroutine(_ResetPass());
                        }
                    }
                    if (_IsGrounded == true || _RB.constraints == RigidbodyConstraints2D.FreezeAll)
                    {
                        if (_RB.constraints == RigidbodyConstraints2D.FreezeAll)
                        {
                            _CancelHanging();
                        }
                        else if (_IsGrounded == true)
                        {
                            GameObject _DustInst = null;
                            if (_stageManagerSc._JumpDustPool.Count == 0)
                            {
                                _DustInst = Instantiate(_DustBurstObj);
                                _DustInst.GetComponent<ParticlePoolingSC>()._TargetPool = _stageManagerSc._JumpDustPool;
                            }
                            else if (_stageManagerSc._JumpDustPool.Count >= 1)
                            {
                                _DustInst = _stageManagerSc._JumpDustPool[0];
                                _DustInst.SetActive(true);
                                _stageManagerSc._JumpDustPool.RemoveAt(0);
                            }
                            _DustInst.transform.position = transform.position;
                        }
                        _IsJumping = true;
                        _JumpCountCounter = _JumpMaxCount;
                        _JumpTimeCounter = _JumpMaxTime;
                    }
                }

                //Teleport
                if ((Input.GetButtonUp("ButtonR1") || Input.GetKeyUp(SysSaveSC._Keys[8])) && _SwordList.Count > 0 && _SwordDistanceList[_NearestSword] <= _WarpDistance)
                {
                    Teleport();
                }

                //Slash
                if ((Input.GetButtonDown("ButtonX") || Input.GetKeyDown(SysSaveSC._Keys[6])) & _AtkTimer <= 0)
                {
                    Slash();
                }
            }
        }
        if (_MyCamera != null && _MyCamera.GetComponent<AudioLowPassFilter>().cutoffFrequency < 22000)
        {
            _MyCamera.GetComponent<AudioLowPassFilter>().cutoffFrequency += _FrquencySpeed * Time.unscaledDeltaTime;
        }
        if (_lumiaCamSc != null)
        {
            _lumiaCamSc.UpdateCamera();
        }
    }
    void _SetPortal()
    {
        if (_CurrentPortal == null)
        {
            _CurrentPortal = Instantiate(_PortalMark);
        }
        _CurrentPortal.transform.position = _WarpPos;
    }
    void TargetPosReset()
    {
        _TargetOnOff(false);
        _InRange = false;
        Vector3 ResetOffset = new Vector3(0, 0.7f, 0f);
        _TargetMark.transform.position = Vector3.MoveTowards(_TargetMark.transform.position, transform.position + ResetOffset, _TargetMarkSpeed * Time.deltaTime);
    }
    void _TargetOnOff(bool _On)
    {
        if (_TarSR != null)
        {
            _TarSR.enabled = _On;
        }
    }
    IEnumerator _Reload(bool _Manual)
    {
        _SwordList.Clear();
        _SwordDistanceList.Clear();

        GameObject[] _Swords = GameObject.FindGameObjectsWithTag("Sword");

        if (_Manual == true)
        {
            for (int i = 0; i < _Swords.Length; i++)
            {
                if (_SwordStock <= _SwordMax - 4)
                {
                    _Swords[i].GetComponent<SwordSC>()._Boom();
                }
                else
                {
                    _Swords[i].GetComponent<SwordSC>()._DestroySword(true);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        _SwordStock = _SwordMax;
        _Canvas.GetComponent<PauseSC>()._UpdateSwordCurrent();
        UpdateBackSwords();

    }
    public IEnumerator _WhiteFlash()
    {
        _mainAudioSource.PlayOneShot(sfx[2], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        WhiteAlpha = 1.5f;
        _Hitbox.GetComponent<ParticleSystem>().Play();
        while (WhiteAlpha > 0)
        {
            WhiteAlpha -= _ColorFadeSpeed * Time.deltaTime;
            SetColor(new Color(1, 1, 1, WhiteAlpha));
            yield return null;
        }
    }
    IEnumerator _ReloadSwordAni()
    {
        yield return null;
        _BackSwords[3].GetComponent<Animation>().Play("BackSwordAni");
        yield return new WaitForSeconds(0.05f);
        _BackSwords[1].GetComponent<Animation>().Play("BackSwordAni");
        yield return new WaitForSeconds(0.05f);
        _BackSwords[0].GetComponent<Animation>().Play("BackSwordAni");
        yield return new WaitForSeconds(0.05f);
        _BackSwords[2].GetComponent<Animation>().Play("BackSwordAni");
        yield return new WaitForSeconds(0.05f);
        _BackSwords[4].GetComponent<Animation>().Play("BackSwordAni");
    }
    void CheckGrounded()
    {
        Collider2D ground;
        ground = Physics2D.OverlapBox(transform.position, new Vector2(0.4f, 0.1f), 0f, _GroundLayer);
        if (ground != null && _RB.velocity.y <= 0.01 && ((ground.gameObject.GetComponent<PlatformEffector2D>() == null) || (ground.gameObject.GetComponent<PlatformEffector2D>() != null && ground.gameObject.GetComponent<PlatformEffector2D>().colliderMask == -1)))
        {
            _IsGrounded = true;
            _CoyoteTimer = 0;
        }
        else
        {
            if (_CoyoteTimer < _CoyoteTime)
            {
                _CoyoteTimer += Time.deltaTime;
            }
            else
            {
                _IsGrounded = false;
            }
        }
    }
    IEnumerator _ResetPass()
    {
        while (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y) + GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size - new Vector2(0.2f, 0f), 0f, _GroundLayer) == true)
        {
            yield return null;
        }
        PlatformEffector2D _PassGround = FindObjectOfType<PlatformEffector2D>();
        if (_PassGround != null)
        {
            _PassGround.colliderMask = -1;
        }

    }
    public void ToSword()
    {
        RaycastHit2D _LumiaRCH = Physics2D.Raycast(_SwordList[_NearestSword].transform.position, Vector2.down, Mathf.Infinity, _GroundLayer);
        float _Height = _LumiaRCH.distance;
        if (_Height >= _MinHightForHang)
        {
            _RB.constraints = RigidbodyConstraints2D.FreezeAll;
            Vector3 WarpOffset = new Vector3(0, -1.7f, 0f);
            transform.position = _SwordList[_NearestSword].transform.position + WarpOffset;
            transform.parent = _SwordList[_NearestSword].transform;
            _mainAnimator.SetBool(AniIsHanging, true);
            _mainAnimator.SetTrigger(AniDoHang);
        }
        else if (_Height < _MinHightForHang)
        {
            _CancelHanging();
            Vector3 WarpOffset = new Vector3(0, -0.6f, 0f);
            transform.position = _SwordList[_NearestSword].transform.position + WarpOffset;
        }
    }
    public void UpdateBackSwords()
    {
        bool _SlashingB = false;
        if (_mainAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlashSide") == true || _mainAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlashUp") == true || _mainAnimator.GetCurrentAnimatorStateInfo(0).IsName("SlashDown") == true)
        {
            _SlashingB = true;
        }
        int _SlashingInt = _SlashingB == true ? 0 : _SlashingB == false ? 1 : 0;
        int _Zero = 1;
        if (_mainAnimator.GetBool(AniIsGliding) == true || _WarpTimer != 0)
        {
            _Zero = 0;
        }
        else
        {
            _Zero = 1;
        }
        int _BackSwordsCount = _BackSwords.Length;
        for (int i = 0; i < _BackSwordsCount; i++)
        {
            if (i >= (_SwordStock + _SlashingInt) * _Zero)
            {
                _BackSwords[i].SetActive(false);
            }
            else if (i < (_SwordStock + _SlashingInt) * _Zero)
            {
                _BackSwords[i].SetActive(true);
            }
        }
    }
    void _FootStepSFX()
    {
        _mainAudioSource.PlayOneShot(sfx[3], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
    }
    void Blink()
    {
        if (_InvincibleTimer > 0)
        {
            _InvincibleTimer -= Time.deltaTime;
            if (_BlinkGetDark == true)
            {
                _BlinkColor -= Time.unscaledDeltaTime * _BlinkSpeed * _SR_Bright;
            }
            else if (_BlinkGetDark == false)
            {
                _BlinkColor += Time.unscaledDeltaTime * _BlinkSpeed * _SR_Bright;
            }

            if (_BlinkColor <= 0)
            {
                _BlinkGetDark = false;
            }
            else if (_BlinkColor >= _SR_Bright)
            {
                _BlinkGetDark = true;
            }
        }
        else if (_InvincibleTimer <= 0)
        {
            _BlinkColor = _SR_Bright;
        }
        _mainSpriteRenderer.color = new Color(_BlinkColor, _BlinkColor, _BlinkColor);
    }
    public void _MusicCheck()
    {
        if (_bgmPlayer == null)
        {
            _bgmPlayer = _Hitbox.GetComponent<AudioSource>();
        }
        if (_bgmPlayer.clip != _stageManagerSc._StageBGM)
        {
            StartCoroutine(_ChangeMusic(_stageManagerSc._StageBGM));
        }
    }
    public IEnumerator _ChangeMusic(AudioClip _Clip)
    {
        while (_bgmPlayer.volume > 0 && SysSaveSC._Vol_Master * SysSaveSC._Vol_BGM > 0)
        {
            _bgmPlayer.volume -= Time.deltaTime * _MusicFadeSpeed * SysSaveSC._Vol_Master * SysSaveSC._Vol_BGM * 0.01f;
            yield return null;
        }
        _bgmPlayer.clip = _Clip;
        _bgmPlayer.volume = _MusicFadeSpeed * SysSaveSC._Vol_Master * SysSaveSC._Vol_BGM * 0.01f;
        _bgmPlayer.Play();
    }
    void SetColor(Color color)
    {
        material.SetColor(SHADER_COLOR_NAME, color);
    }
    public IEnumerator _LoadScene(string _TargetScene, bool _KeepLumia)
    {
        while (_Canvas.GetComponent<PauseSC>()._FadeObj.GetComponent<CanvasGroup>().alpha < 1f)
        {
            yield return null;
        }
        AsyncOperation op = SceneManager.LoadSceneAsync(_TargetScene);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        DontDestroyOnLoad(gameObject);
        op.allowSceneActivation = true;
        _CurrentScene = _TargetScene;
        if (_KeepLumia == true)
        {
            StartCoroutine(_Reload(false));
        }
        else
        {
            Destroy(_Canvas);
            Destroy(_TargetMark);
            Destroy(gameObject);
        }

    }
    public void _WhenSceneLoad()
    {
        _stageManagerSc = _MyCamera.GetComponent<StageManagerSC>();
        if (SceneManager.GetActiveScene().name == _WarpScene)
        {
            _SetPortal();
        }
        if (_ChairRespawn == true)
        {
            _ChairRespawn = false;
            _stageManagerSc._ChairStart();
            _RB.bodyType = RigidbodyType2D.Dynamic;
        }
        if (_PassingPortal == true)
        {
            _PassingPortal = false;
            transform.position = _WarpPos;
            _CanControl = true;
        }
        if (_PassingGate == true)
        {
            _PassingGate = false;
            transform.position = _stageManagerSc._Gates[_GateNumber].position;

            if (_JumpGate == false)
            {
                if (_AutoWalk != 0)
                {
                    StartCoroutine(_StopAutoMove());
                }
                else
                {
                    StartCoroutine(_FallGateEvent());
                }
            }

            else if (_JumpGate == true)
            {
                transform.position = transform.position + Vector3.up * 2f;
                if (_JumpGateDir == 0)
                {
                    _AutoWalk = _mainSpriteRenderer.flipX == false ? 0.5f : _mainSpriteRenderer.flipX == true ? -0.5f : 0;
                }
                else if (_JumpGateDir != 0)
                {
                    _AutoWalk = _JumpGateDir * 0.5f;
                    _mainSpriteRenderer.flipX = _JumpGateDir > 0 ? false : _JumpGateDir < 0 ? true : false;
                }
                StartCoroutine(_JumpGateEvent());
            }
        }
        _MusicCheck();
        _Canvas.GetComponent<Canvas>().worldCamera = _MyCamera.GetComponent<Camera>();
        _Canvas.GetComponent<Canvas>().sortingLayerName = "UI";
        _Canvas.GetComponent<PauseSC>()._FadeObj.GetComponent<UIFaderSC>()._FadeOut();
        _TargetMark.transform.position = transform.position;
    }
    IEnumerator _StopAutoMove()
    {
        yield return new WaitForSeconds(0.5f);
        _AutoWalk = 0f;
        _CanControl = true;
        _RespawnPoint = transform.position;
    }
    IEnumerator _JumpGateEvent()
    {
        yield return new WaitForSeconds(0.3f);
        _AutoJumping = false;
        while (_IsGrounded == false)
        {
            yield return null;
        }
        _RespawnPoint = transform.position;
        _CanControl = true;
        _JumpGate = false;
        _AutoWalk = 0;
        _JumpGateDir = 0;
    }
    IEnumerator _FallGateEvent()
    {
        yield return new WaitForSeconds(0.3f);
        _CanControl = true;
        while (_IsGrounded == false)
        {
            yield return null;
        }
        _RespawnPoint = transform.position;
    }
    public void _CancelHanging()
    {
        _mainAnimator.SetBool(AniIsHanging, false);
        _RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (transform.parent != null)
        {
            transform.parent = null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "RespawnArea")
        {
            _RespawnPoint = col.gameObject.GetComponent<RespawnAreaSC>()._RespawnPoint;
        }
        else if (col.gameObject.tag == "Gate" && _CanControl == true && _AutoJumping == false && _RB.constraints == RigidbodyConstraints2D.FreezeRotation)
        {
            _CanControl = false;
            _PassingGate = true;
            GateSC _GSC = col.gameObject.GetComponent<GateSC>();
            GameObject _FadeObj = _Canvas.GetComponent<PauseSC>()._FadeObj;
            UIFaderSC _UIFSC = _FadeObj.GetComponent<UIFaderSC>();
            StartCoroutine(_LoadScene(_GSC._TargetScene, true));
            _GateNumber = _GSC._TargetGateNumber;
            if (_Canvas.GetComponent<PauseSC>()._FadeObj.activeSelf == false)
            {
                _Canvas.GetComponent<PauseSC>()._FadeObj.SetActive(true);
            }
            else
            {
                _UIFSC.StopAllCoroutines();
                _UIFSC._Working = false;
                _UIFSC._TargetAlpha = 1;
                _UIFSC._FadeSpeed = 1;
                _UIFSC.OnEnable();
            }

            if (_GSC._WallGate == true)
            {
                _AutoWalk = _mainSpriteRenderer.flipX == false ? 1 : _mainSpriteRenderer.flipX == true ? -1 : 0;
            }
            else if (_GSC._WallGate == false)
            {
                if (_RB.velocity.y > 0)
                {
                    _AutoJumping = true;
                    _JumpGate = true;
                    _JumpGateDir = _GSC._ForceDir;
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 8 && col.gameObject.tag == "MovingPlatform" && _RB.constraints == RigidbodyConstraints2D.FreezeRotation)
        {
            transform.parent = col.gameObject.transform.parent.transform;
            _LastPlatform = col.gameObject;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == 8 && col.gameObject.tag == "MovingPlatform" && col.gameObject == _LastPlatform)
        {
            transform.parent = null;
        }
    }
}
