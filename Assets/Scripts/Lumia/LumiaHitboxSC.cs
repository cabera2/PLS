using System.Collections;
using Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lumia
{
    public class LumiaHitboxSC : MonoBehaviour
    {
        [Header("Debug")]
        public bool _NoDamage;
        public bool _NoHit;
        [Header("Other")]
        public int _HP_Max;
        public int _HP_Current;
        private Rigidbody2D _RB;
        private Animator _ANI;
        private AudioSource _AS;
        public AudioClip[] _SFX;
        private GameObject _Lumia;
        public GameObject _Particle;
        public GameObject _FadeObj;
        public GameObject _DeathImpact;
        public GameObject _Canvas;
        private LumiaSC _LumiaSC;
        public float _HitStopTime;
        public float _HitStopTimer;
        private bool _DeathAniPlayed;
        public string _RestartScene;
        public Vector3 _RestartPos;
    
        void Start()
        {
            _Lumia = transform.parent.gameObject;
            _RB = _Lumia.GetComponent<Rigidbody2D>();
            _ANI = _Lumia.GetComponent<Animator>();
            _AS = _Lumia.GetComponent<AudioSource>();
            _LumiaSC = transform.parent.GetComponent<LumiaSC>();
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (_NoHit == false)
            {
                if (transform.parent.GetComponent<LumiaSC>()._InvincibleTimer <= 0 && (col.gameObject.layer == 12 || col.gameObject.tag == "Lazor"))
                {
                    if ((col.gameObject.layer == 12 && (col.gameObject.GetComponent<EnemyCommonSC>() != null && col.gameObject.GetComponent<EnemyCommonSC>()._HP > 0 || col.gameObject.GetComponent<EnemyCommonSC>() == null) || col.gameObject.tag == "Lazor") && _RB.bodyType == RigidbodyType2D.Dynamic)
                    {
                        transform.parent.GetComponent<LumiaSC>()._InvincibleTimer = transform.parent.GetComponent<LumiaSC>()._InvincibleTime;
                        _Damage();
                        if (_HP_Current >= 1)
                        {
                            transform.parent.GetComponent<LumiaSC>()._KnockbackCounter = 0.5f;
                            float a = 0;
                            if (col.gameObject.layer == 12)
                            {
                                a = transform.position.x - col.transform.position.x;
                            }
                            else if (col.gameObject.tag == "Lazor")
                            {
                                Vector2 LazorVector;
                                Transform launcher = col.gameObject.transform;
                                float _Test;
                                LazorVector = launcher.rotation * Vector3.down;
                                _Test = -(launcher.position.y - transform.position.y) * LazorVector.x / LazorVector.y;
                                a = transform.position.x - (launcher.position.x + _Test);
                            }
                            float b = a > 0 ? 1 : a < 0 ? -1 : 0;
                            _RB.velocity = new Vector2(10 * b, 10);
                        }
                        else if (_HP_Current <= 0)
                        {
                            StartCoroutine(_DeathEvent());
                        }
                    }
                }

                else if (col.gameObject.layer == 13)
                {
                    _LumiaSC._KnockbackCounter = 1f;
                    _ANI.SetBool(LumiaSC.AniIsSpiked, true);
                    _Damage();
                    if (_HP_Current >= 1)
                    {
                        _RB.bodyType = RigidbodyType2D.Static;
                        StartCoroutine(_SpikeEvent());
                    }
                    else if (_HP_Current <= 0)
                    {
                        StartCoroutine(_DeathEvent());
                    }

                }
            }
        }
        IEnumerator _DeathEvent()
        {
            _DeathAniPlayed = true;
            _ANI.SetBool(LumiaSC.AniIsSpiked, true);
            _RB.bodyType = RigidbodyType2D.Static;
            GetComponent<AudioSource>().Stop();
            _AS.PlayOneShot(_SFX[1], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            _AS.PlayOneShot(_SFX[2], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            GameObject _DeathImpactInst;
            _DeathImpactInst = Instantiate(_DeathImpact);
            _DeathImpactInst.transform.position = transform.position;
            yield return new WaitForSeconds(0.3f);
            _DeathImpactInst.GetComponent<ParticleSystem>().Play();
            StageManagerSC._CamSC._CoolDownLock = true;
            StageManagerSC._CamSC._Shake(0.5f);
            yield return new WaitForSeconds(0.2f);
            _FadeObj.SetActive(true);
            while (_FadeObj.GetComponent<CanvasGroup>().alpha < 1f)
            {
                yield return null;
            }
            _DeathImpactInst.GetComponent<ParticleSystem>().Stop();
            StageManagerSC._CamSC._CoolDownLock = false;
            yield return new WaitForSeconds(1f);
            _LumiaSC._SwordList.Clear();
            _LumiaSC._SwordDistanceList.Clear();
            _LumiaSC._SwordStock = _LumiaSC._SwordMax;
            AsyncOperation op = SceneManager.LoadSceneAsync(_LumiaSC._SavedScene);
            op.allowSceneActivation = false;
            DontDestroyOnLoad(transform.parent);
            while (op.progress < 0.9f)
            {
                yield return null;
            }
            op.allowSceneActivation = true;
            GetComponent<AudioSource>().Play();
            _HP_Current = _HP_Max;
            _Canvas.GetComponent<PauseSC>()._UpdateCurrentHp();
            _LumiaSC._SwordStock = _LumiaSC._SwordMax;
            _Canvas.GetComponent<PauseSC>()._UpdateSwordCurrent();
            _ANI.SetBool(LumiaSC.AniIsSpiked, false);
            _ANI.SetTrigger("_AfterSpike");
            _RB.bodyType = RigidbodyType2D.Dynamic;
            StageManagerSC._lumiaSc._ChairRespawn = true;
            SysSaveSC._CharSave();
        }
        void _Damage()
        {
            _AS.PlayOneShot(_SFX[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            _LumiaSC._MyCamera.GetComponent<AudioLowPassFilter>().cutoffFrequency = 10;
            _ANI.SetTrigger(LumiaSC.AniDoDamage);
            GameObject particleInst;
            particleInst = Instantiate(_Particle);
            particleInst.transform.position = transform.position + Vector3.up * 0.7f;
            StartCoroutine(_HitStop(0.3f));
            StageManagerSC._CamSC._Shake(0.7f);
            if (_NoDamage == false)
            {
                _HP_Current -= 1;
            }
            _Canvas.GetComponent<PauseSC>()._UpdateCurrentHp();
        }

        IEnumerator _SpikeEvent()
        {
            yield return new WaitForSeconds(0.3f);
            _FadeObj.SetActive(true);
            while (_FadeObj.GetComponent<CanvasGroup>().alpha < 1f)
            {
                yield return null;
            }
            transform.parent.transform.position = transform.parent.GetComponent<LumiaSC>()._RespawnPoint;
            transform.parent.GetComponent<LumiaSC>()._InvincibleTimer = transform.parent.GetComponent<LumiaSC>()._InvincibleTime;
            _ANI.SetBool(LumiaSC.AniIsSpiked, false);
            _ANI.SetTrigger("_AfterSpike");
            _RB.bodyType = RigidbodyType2D.Dynamic;
            _FadeObj.GetComponent<UIFaderSC>()._FadeOut();
        }
        IEnumerator _HitStop(float _Value)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(_Value);
            Time.timeScale = 1f;
        }
    }
}
