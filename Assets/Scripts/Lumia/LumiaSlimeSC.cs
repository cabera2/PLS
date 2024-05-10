using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Lumia
{
    public class LumiaSlimeSC : MonoBehaviour
    {
        public GameObject _NewLumia;
        public GameObject _NewCanvas;
        private Rigidbody2D _RB;
        private Animator _ANI;
        private SpriteRenderer _SR;
        private LumiaCamSC _lumiaCamSc;
        private bool _camActive = false;
        public GameObject _MyCamera;
        private GameObject _CamArea;
        public Vector2 _CameraOffset;
        private Vector3 _CamPos1;
        private bool _InCamArea;
        public float _CamSpeed;
        public bool _CanControl;
        [HideInInspector] public float leftStickX;
        private float leftStickY;
        public float _WalkSpeed;
        public bool _CamActive;
        public CanvasGroup _InterActionArrow;
        public CanvasGroup _FadeObj;
        public GameObject _ImageBoard;
        public List<Sprite> _EventCG = new List<Sprite>();
        private GameObject _InteractObj;
        private bool _Guide;
        public GameObject _GuideObj;
        private GameObject _Event;
        private AudioSource _AS;
        public AudioClip[] _SFX;
        private LumiaCamSC _CamSC;
        private MyInputManager _myInput;
        void Start()
        {
            _myInput = MyInputManager.GetMyInput();
            _RB = GetComponent<Rigidbody2D>();
            _ANI = GetComponent<Animator>();
            _SR = GetComponent<SpriteRenderer>();
            _AS = GetComponent<AudioSource>();
            _lumiaCamSc = GetComponent<LumiaCamSC>();
        }
        void _WalkSFX()
        {
            _AS.PlayOneShot(_SFX[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        }
        void Move()
        {
            Vector2Int leftStickInput = _myInput.GetAxis(KeyType.LeftStick);
            if (_CanControl)
            {
                leftStickX = leftStickInput.x;
                leftStickY = leftStickInput.y;
                if (leftStickY != 0)
                {
                    if (leftStickY < -0.5f)
                    {
                        leftStickY = -1f;
                    }

                    if (leftStickY > 0.5f)
                    {
                        leftStickY = 1f;
                    }
                }
            }
            _RB.velocity = new Vector2(leftStickX * _WalkSpeed, _RB.velocity.y);
            _ANI.SetFloat("_XInput", Mathf.Abs(leftStickX));
            if (leftStickX > 0)
            {
                _SR.flipX = false;
            }
            else if (leftStickX < 0)
            {
                _SR.flipX = true;
            }

        }
        void Update()
        {
            Move();
            if (_InteractObj != null)
            {
                if (_CanControl && GetComponent<BoxCollider2D>().IsTouching(_InteractObj.GetComponent<BoxCollider2D>()) && Mathf.Abs(leftStickY) >= 0.5)
                {
                    _CanControl = false;
                    leftStickX = 0;
                    StartCoroutine(_ArrowFadeOutC());
                    if (_InteractObj.name == "SealSword")
                    {
                        StartCoroutine(_FirstSword());
                    }
                }
            }
            if (_lumiaCamSc != null && _camActive)
            {
                _lumiaCamSc.UpdateCamera();
            }
        }
        public void _OpEvent()
        {
            StartCoroutine(_OpEventC());
        }
        public IEnumerator _OpEventC()
        {
            StageManagerSC._LumiaInst = _NewLumia;
            StageManagerSC._CanvasInst = _NewCanvas;
            _CamSC = GetComponent<LumiaCamSC>();
            _camActive = true;
            StageManagerSC._CamSC = _CamSC;
            yield return new WaitForSeconds(6f);
            _ANI.enabled = true;
            yield return new WaitForSeconds(3f);
            _Event = GameObject.Find("Event1");
            _Event.GetComponent<DialogueSC>()._Lumia = gameObject;
            _Event.GetComponent<DialogueSC>()._StartTalk();
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "InteractObj")
            {
                _InteractObj = col.gameObject;
                _InterActionArrow.GetComponent<WorldTextSC>()._TargetObj = col.gameObject;
                _InterActionArrow.gameObject.SetActive(true);
                StartCoroutine(_ArrowFadeInC());
                if (_Guide == false)
                {
                    _Guide = true;
                    _GuideObj.GetComponent<LangChangeSC>()._LangText[0] = "↑ 조사";
                    _GuideObj.GetComponent<LangChangeSC>()._LangText[1] = "↑ 調査";
                    _GuideObj.SetActive(true);
                }
            }
        }
        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "InteractObj")
            {
                StartCoroutine(_ArrowFadeOutC());
            }
        }
        IEnumerator _ArrowFadeInC()
        {
            while (_InterActionArrow.alpha < 1)
            {
                _InterActionArrow.alpha += 3f * Time.deltaTime;
                yield return null;
            }
        }
        IEnumerator _ArrowFadeOutC()
        {
            while (_InterActionArrow.alpha > 0)
            {
                _InterActionArrow.alpha -= 3f * Time.deltaTime;
                yield return null;
            }
            _InterActionArrow.gameObject.SetActive(false);
        }
        IEnumerator _FirstSword()
        {
            StageManagerSC._CamSC._CoolDownLock = true;
            StageManagerSC._CamSC._Shake(0.1f);
            _FadeObj.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            _FadeObj.gameObject.SetActive(true);
            _FadeObj.alpha = 1;
            yield return new WaitForSeconds(0.1f);
            _FadeObj.alpha = 0;
            float _FadeSpeed = 1f;
            //_InteractObj.GetComponent<ParticleSystem>().Clear(true);
            _InteractObj.GetComponent<Animator>().SetTrigger("_GoFast");
            yield return new WaitForSeconds(3f);
            _InteractObj.GetComponent<Animator>().SetTrigger("_End");
            yield return new WaitForSeconds(2f);
            _FadeObj.alpha = 1;
            Destroy(_InteractObj);
            _SR.enabled = false;
            GameObject _Lumia = StageManagerSC._LumiaInst;
            _CamSC._CoolDownLock = false;
            _Lumia.SetActive(true);
            _NewCanvas.SetActive(true);
            _Lumia.GetComponent<LumiaSC>()._MyCamera = _MyCamera;
            StageManagerSC._lumiaSc = _Lumia.GetComponent<LumiaSC>();
            GetComponent<MapSaverSC>()._SaveStatus();
            _CamSC.enabled = false;
            StageManagerSC._CamSC = _Lumia.GetComponent<LumiaCamSC>();
            _ImageBoard.GetComponent<UIFaderSC>().enabled = false;
            for (int i = 0; i < _EventCG.Count; i++)
            {
                _ImageBoard.GetComponent<Image>().sprite = _EventCG[i];
                _ImageBoard.SetActive(true);
                CanvasGroup _BoardGroup = _ImageBoard.GetComponent<CanvasGroup>();
                while (_BoardGroup.alpha < 1)
                {
                    _BoardGroup.alpha += _FadeSpeed * Time.deltaTime;
                    yield return null;
                }
                yield return new WaitForSeconds(1f);
                while (_BoardGroup.alpha > 0)
                {
                    _BoardGroup.alpha -= _FadeSpeed * Time.deltaTime;
                    yield return null;
                }
            }
            yield return new WaitForSeconds(1f);
            while (_FadeObj.alpha > 0)
            {
                _FadeObj.alpha -= _FadeSpeed * Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            _Event.GetComponent<DialogueSC>()._Lumia = _Lumia;
            _Event.GetComponent<DialogueSC>()._StartLine = 2;
            _Event.GetComponent<DialogueSC>()._EndLine = 10;
            _Event.GetComponent<DialogueSC>()._EndActive.Clear();
            _Event.SetActive(true);
            _Event.GetComponent<DialogueSC>()._StartTalk();
            Destroy(gameObject);
        }
    }
}
