using System;
using System.Collections;
using Lumia;
using UnityEngine;

namespace Enemy
{
    public class EnemyCommonSC : MonoBehaviour
    {
        public bool _SpinAtDeath;
        private const string SHADER_COLOR_NAME = "_Color";
        private Material material;
        private Rigidbody2D _RB;
        private Animator _ANI;
        private SpriteRenderer _SR;
        private AudioSource _AS;
        public AudioClip[] _SFX;
        private GameObject _Lumia;
        public GameObject _BleedFX;
        private LumiaSC lumiaSC;
        public int _HP;
        public GameObject _CoinPrefab;
        private float WhiteAlpha;
        public float _FadeSpeed;
        public float _StunTime;
        public float _KnockBackForce;
        public float _DeathKnockBack = 10f;
        [Header("Guard")]
        public bool _GuardTop;
        public bool _GuardFront;
        public bool _GuardBack;
        [HideInInspector] public WaveEventSC _WaveController;


        // Use this for initialization
        void Start()
        {
            _SR = GetComponent<SpriteRenderer>();
            material = _SR.material;
            _RB = GetComponent<Rigidbody2D>();
            _ANI = GetComponent<Animator>();
            StartCoroutine(_FindLumia());
        }
        IEnumerator _FindLumia()
        {
            while (StageManagerSC._LumiaInst == null)
            {
                yield return null;
            }
            if (_Lumia == null)
            {
                _Lumia = StageManagerSC._LumiaInst;
            }
            if (lumiaSC == null)
            {
                lumiaSC = _Lumia.GetComponent<LumiaSC>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            WhiteAlpha = Mathf.MoveTowards(WhiteAlpha, 0, _FadeSpeed * Time.deltaTime);
            SetColor(new Color(1, 1, 1, WhiteAlpha));
        }
        IEnumerator _DeathEvent()
        {
            yield return new WaitForSeconds(0.05f);
            while (_RB.velocity != Vector2.zero)
            {
                yield return null;
            }
            if (_ANI != null)
            {
                _ANI.SetInteger("_DeathPhase", 2);
                _SR.color = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if ((col.gameObject.name == "PlayerDetector" && col.GetComponent<PlayerDetectorSC>()._Grounded == false) || col.gameObject.name == "Slash(Clone)")
            {
                float _AtkAngle = col.transform.eulerAngles.z;
                if (_HP > 0)
                {
                    col.GetComponent<HitFXSC>()._GenFX();
                    if (col.gameObject.name == "Slash(Clone)")
                    {
                        if (_AtkAngle == 90)
                        {
                            if (_GuardFront == true && _SR.flipX == false || _GuardBack == true && _SR.flipX == true)
                            {
                                _Blocked(col);
                            }
                            else
                            {
                                _Hit(col);
                            }
                        }
                        else if (_AtkAngle == 270)
                        {
                            if (_GuardFront == true && _SR.flipX == true || _GuardBack == true && _SR.flipX == false)
                            {
                                _Blocked(col);
                            }
                            else
                            {
                                _Hit(col);
                            }
                        }
                        else if (_AtkAngle == 0)
                        {
                            if (_GuardTop == true)
                            {
                                _Blocked(col);
                            }
                            else
                            {
                                _Hit(col);
                            }
                        }
                        else if (_AtkAngle == 180)
                        {
                            _Hit(col);
                        }
                    }
                    else
                    {
                        _Hit(col);
                    }
                }
            }
        }
        void _Blocked(Collider2D col)
        {
            col.gameObject.GetComponent<SlashSC>()._Reject();
        }
        void _Hit(Collider2D col)
        {
            _Lumia.GetComponent<AudioSource>().PlayOneShot(_SFX[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            GameObject _FXInst;
            _FXInst = Instantiate(_BleedFX);
            _FXInst.transform.position = transform.position + (transform.up * 0.7f);
            _FXInst.transform.rotation = col.transform.rotation;
            if (col.gameObject.name == "Slash(Clone)")
            {
                _GetDamage(lumiaSC.levelData.attackValues[lumiaSC.levelData.attackLv], col.transform.rotation * Vector3.down);
            }
            else if (col.gameObject.name == "PlayerDetector" && col.GetComponent<PlayerDetectorSC>()._Grounded == false)
            {
                _GetDamage((Int16)(Math.Round(lumiaSC.levelData.attackValues[lumiaSC.levelData.attackLv] * lumiaSC.levelData.shotValues[lumiaSC.levelData.shotLv], 0, MidpointRounding.AwayFromZero)), col.transform.rotation * Vector3.down);
            }
        }
        public void _BoomHit(Vector3 _Pos)
        {
            Debug.Log("BoomHit!");
            _GetDamage(20, transform.position - _Pos);
        }
        void _GetDamage(int _DamageValue, Vector2 _HitDir)
        {
            if (_HP > _DamageValue)
            {
                Invoke("_Activate", _StunTime);
                _RB.velocity = _HitDir.normalized * _KnockBackForce;
            }
            else if (_HP <= _DamageValue)
            {
                if (GetComponent<MapSaverSC>() != null)
                {
                    GetComponent<MapSaverSC>()._SaveStatus();
                }
                _Lumia.GetComponent<AudioSource>().PlayOneShot(_SFX[1], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                StageManagerSC._CamSC._Shake(0.5f);
                if (_SpinAtDeath == true)
                {
                    _RB.constraints = RigidbodyConstraints2D.None;
                }
                if (_ANI != null)
                {
                    _ANI.SetInteger("_DeathPhase", 1);
                    StartCoroutine(_DeathEvent());
                }
                if (_WaveController != null)
                {
                    _WaveController.Summon(true);
                }

                _RB.gravityScale = 5;
                float _DeathDir = _HitDir.x;
                _SR.flipX = _DeathDir < 0 ? true : _DeathDir > 0 ? false : false;
                int _DeathDirNormal = _DeathDir > 0 ? 1 : _DeathDir < 0 ? -1 : 0;
                _RB.velocity = new Vector2(_DeathDirNormal * _DeathKnockBack, _DeathKnockBack);
                if (GetComponent<MakeCoinSC>() != null)
                {
                    GetComponent<MakeCoinSC>()._Make();
                }
            }
            _HP -= _DamageValue;
            WhiteAlpha = 1.5f;
        }
        void SetColor(Color color)
        {
            if (material != null)
            {
                material.SetColor(SHADER_COLOR_NAME, color);
            }
        }
        void _Activate()
        {
            GetComponent<EnemyParentSC>().enabled = true;
        }
    }
}
