using System.Collections;
using System.Collections.Generic;
using Lumia;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseSC : MonoBehaviour
{
    public GameObject _FadeObj;
    public GameObject _FlashObj;

    public GameObject[] _MenuPages;
    //public GameObject _Lumia;
    public GameObject _Heart;

    [Header("Map")]
    public GameObject _MapWin;
    public GameObject _MapPin;
    private Vector2 _MapOffset;
    private Animator _MapPinAni;
    [Header("Status")]
    public GameObject _StatusWin;
    private LumiaSC _LSC;
    private LumiaHitboxSC _HBSC;
    public float _HeartGap;
    private int _Hp;
    public List<GameObject> _HeartList = new List<GameObject>();
    public GameObject _GaugePrefab;
    private int _Stock;
    public List<GameObject> _GaugeList = new List<GameObject>();
    public bool _IsPaused = false;
    private bool _MapOpen = false;
    private bool _StatusOpen = false;

    private float _PrevFadeSpeed;
    public Text _MoneyText;
    public Text _SwordText;
    private float _SwordStockTimer;
    private Animator _Ani;
    private CanvasGroup _CG;
    private readonly MyInputManager _myInput = new();

    // Start is called before the first frame update
    void Start()
    {
        _MapOffset = _MapWin.transform.GetChild(0).localPosition;
        _MapPinAni = _MapPin.GetComponent<Animator>();
        _Ani = GetComponent<Animator>();
        _CG = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartC());
    }
    IEnumerator StartC()
    {
        while (StageManagerSC._LumiaInst == null)
        {
            yield return null;
        }
        _LSC = StageManagerSC._LumiaInst.GetComponent<LumiaSC>();
        _HBSC = StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._Hitbox.GetComponent<LumiaHitboxSC>();
        _UpdateMaxHp();
        _UpdateSwordMax();
    }
    public IEnumerator _Fade(bool _FadeIn)
    {
        if (_FadeIn == true)
        {
            while (_CG.alpha < 1)
            {
                _CG.alpha = Mathf.MoveTowards(_CG.alpha, 1, 3f * Time.fixedDeltaTime);
                yield return null;
            }
        }
        else
        {
            while (_CG.alpha > 0)
            {
                _CG.alpha = Mathf.MoveTowards(_CG.alpha, 0, 3f * Time.fixedDeltaTime);
                yield return null;
            }
        }
    }
    void Update()
    {
        if (_MapOpen == false && _myInput.GetButtonDown(KeyType.Map) && _IsPaused == false && _StatusOpen == false && _LSC._IsGrounded == true && _LSC._CanControl == true)
        {
            _MapOpen = true;
            _DarkScreen(true);
            _MapWin.SetActive(true);
            StartCoroutine(_MoveMapPin());
        }
        else if (_MapOpen == true && _myInput.GetButtonUp(KeyType.Map) || _LSC._IsGrounded == false || _LSC._CanControl == false)
        {
            _MapOpen = false;
            _DarkScreen(false);
            _MapWin.GetComponent<UIFaderSC>()._FadeOut();
        }

        if (_myInput.GetButtonUp(KeyType.Pause) && _MapOpen == false && _StatusOpen == false)
        {
            if (_IsPaused == false)
            {
                _IsPaused = true;
                _DarkScreen(true);
                _MenuPages[1].SetActive(false);
                _MenuPages[1].GetComponent<CanvasGroup>().alpha = 0f;
                _MenuPages[2].SetActive(true);
                _MenuPages[2].GetComponent<CanvasGroup>().alpha = 1f;
                _MenuPages[3].SetActive(false);
                _MenuPages[3].GetComponent<CanvasGroup>().alpha = 0f;

                _MenuPages[0].SetActive(true);
                EventSystem.current.SetSelectedGameObject(_MenuPages[0].transform.GetChild(1).gameObject);
                Invoke("_FirstMenu", Time.unscaledDeltaTime);
                _MenuPages[0].transform.GetChild(1).GetComponent<Animator>().ResetTrigger("_Off");
                _MenuPages[0].transform.GetChild(1).GetComponent<Animator>().SetTrigger("_On");

                Time.timeScale = 0f;
                Cursor.visible = true;
            }
            else
            {
                _ClosePause();
            }
        }
        if (_myInput.GetButtonUp(KeyType.Status) && _MapOpen == false && _IsPaused == false)
        {
            if (_StatusOpen == false)
            {
                Cursor.visible = true;
                _StatusOpen = true;
                _DarkScreen(true);
                _StatusWin.SetActive(true);
                _LSC._CanControl = false;
            }
            else
            {
                _CloseStatus();
            }
        }
    }
    public void _CloseStatus()
    {
        Cursor.visible = false;
        _StatusOpen = false;
        _DarkScreen(false);
        _StatusWin.GetComponent<UIFaderSC>()._FadeOut();
        _LSC._CanControl = true;
    }
    void _DarkScreen(bool _Dark)
    {
        if (_Dark == true)
        {
            _PrevFadeSpeed = _FadeObj.GetComponent<UIFaderSC>()._FadeSpeed;
            _FadeObj.GetComponent<UIFaderSC>()._FadeSpeed = _MenuPages[0].GetComponent<UIFaderSC>()._FadeSpeed;
            _FadeObj.GetComponent<UIFaderSC>()._TargetAlpha = 0.5f;
            _FadeObj.SetActive(true);
        }
        else
        {
            _FadeObj.GetComponent<UIFaderSC>()._TargetAlpha = 1f;
            _FadeObj.GetComponent<UIFaderSC>()._FadeOut();
            StartCoroutine(_ResetFadeSpeed());
        }
    }
    IEnumerator _MoveMapPin()
    {
        while (_MapOpen == true)
        {
            Vector2 _V2 = StageManagerSC._LumiaInst.transform.position;
            _MapPin.transform.localPosition = (_V2 + StageManagerSC._MapOffsetNow) * 4 + _MapOffset;
            if (_LSC._MoveInput != 0)
            {
                _MapPinAni.SetBool("_Walking", true);
            }
            else
            {
                _MapPinAni.SetBool("_Walking", false);
            }
            yield return null;
        }
    }
    public void _ClosePause()
    {
        _IsPaused = false;
        _DarkScreen(false);
        if (_MenuPages[1].activeSelf == true || _MenuPages[2].activeSelf == true)
        {
            SysSaveSC._SysSave();
            if (_MenuPages[3].activeSelf == true)
            {
                _MenuPages[3].GetComponent<KeySettingSC>()._Waiting = false;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            if (_MenuPages[i].activeSelf == true)
            {
                _MenuPages[i].GetComponent<UIFaderSC>()._FadeOut();
            }

        }
        Time.timeScale = 1f;
        Cursor.visible = false;
    }
    void _FirstMenu()
    {
        _MenuPages[0].transform.GetChild(1).GetComponent<Button>().Select();
    }
    void FixedUpdate()
    {
        if (_SwordText.GetComponent<CanvasGroup>().alpha > 0)
        {
            _SwordText.transform.position = StageManagerSC._LumiaInst.transform.position + Vector3.up * 1.8f;
        }
    }

    public void _UpdateMaxHp()
    {
        _Hp = _HBSC._HP_Current;
        while (_HeartList.Count < _HBSC._HP_Max)
        {
            GameObject _HeartInst;
            Vector3 _Offset;
            _Offset = new Vector3(-9f, 4.8f, -0.5f);
            _HeartInst = Instantiate(_Heart, transform.position + _Offset + (Vector3.right * _HeartList.Count * _HeartGap), transform.rotation, transform);
            _HeartList.Add(_HeartInst);
        }
        if (_HeartList.Count > _HBSC._HP_Max)
        {
            Destroy(_HeartList[_HeartList.Count - 1]);
            _HeartList.RemoveAt(_HeartList.Count - 1);
        }
    }
    public void _UpdateCurrentHp()
    {
        while (_Hp != _HBSC._HP_Current)
        {
            if (_Hp > _HBSC._HP_Current)
            {
                _HeartList[_Hp - 1].GetComponent<Animator>().SetBool("_Empty", true);
                _Hp -= 1;
            }
            else if (_Hp < _HBSC._HP_Current)
            {
                _HeartList[_Hp].GetComponent<Animator>().SetBool("_Empty", false);
                _Hp += 1;
            }
        }
    }

    public void _UpdateSwordMax()
    {
        _Stock = _LSC._SwordMax + 1;
        while (_GaugeList.Count < _LSC._SwordMax + 1)
        {
            GameObject _GaugeInst;
            Vector3 _Offset;
            _Offset = new Vector3(-10.5f, 4.3f, -0.5f);
            _GaugeInst = Instantiate(_GaugePrefab, transform.position + _Offset + (Vector3.right * _GaugeList.Count * _HeartGap), transform.rotation, transform);
            _GaugeInst.GetComponent<RectTransform>().sizeDelta = new Vector2(_HeartGap * 100 + 13, 20);
            _GaugeList.Add(_GaugeInst);
        }
        if (_GaugeList.Count > _LSC._SwordMax + 1)
        {
            Destroy(_GaugeList[_GaugeList.Count - 1]);
            _GaugeList.RemoveAt(_GaugeList.Count - 1);
        }
    }
    public void _UpdateSwordCurrent()
    {
        _SwordText.text = (_LSC._SwordStock + 1).ToString();
        _SwordStockTimer = 1;
        if (_SwordText.GetComponent<CanvasGroup>().alpha > 0)
        {
            _SwordText.GetComponent<CanvasGroup>().alpha = 1;
        }
        else if (_SwordText.GetComponent<CanvasGroup>().alpha <= 0)
        {

            StartCoroutine(_SwordStockAlpha());
        }

        while (_Stock != _LSC._SwordStock + 1)
        {
            if (_Stock > _LSC._SwordStock + 1)
            {
                _GaugeList[_Stock - 1].GetComponent<Animator>().SetBool("_Empty", true);
                _Stock -= 1;
            }
            else if (_Stock < _LSC._SwordStock + 1)
            {
                _GaugeList[_Stock].GetComponent<Animator>().SetBool("_Empty", false);
                _Stock += 1;
            }
        }
    }
    IEnumerator _ResetFadeSpeed()
    {
        while (_FadeObj.GetComponent<CanvasGroup>().alpha > 0)
        {
            yield return null;
        }
        _FadeObj.GetComponent<UIFaderSC>()._FadeSpeed = _PrevFadeSpeed;
    }
    public void _ToTitle()
    {
        _LSC._TemporaryFlag.Clear();
        _LSC._PermanentFlag.Clear();
        _MenuPages[0].SetActive(false);
        _FadeObj.GetComponent<UIFaderSC>()._TargetAlpha = 1f;
        _FadeObj.GetComponent<UIFaderSC>()._FadeSpeed = 1;
        _FadeObj.GetComponent<UIFaderSC>().OnEnable();
        StartCoroutine(_LSC._LoadScene("TitleScreen", false));
        StartCoroutine(_ToTitleC());
    }
    IEnumerator _ToTitleC()
    {
        while (_FadeObj.GetComponent<CanvasGroup>().alpha < 1f)
        {
            yield return null;
        }
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
    }
    IEnumerator _SwordStockAlpha()
    {
        _SwordText.GetComponent<CanvasGroup>().alpha = 1;
        while (_SwordText.GetComponent<CanvasGroup>().alpha > 0)
        {
            _SwordStockTimer -= Time.deltaTime;
            if (_SwordStockTimer <= 0)
            {
                _SwordText.GetComponent<CanvasGroup>().alpha -= 10 * Time.deltaTime;
            }
            yield return null;
        }
    }
    public void _ScreenFlash()
    {
        _FlashObj.transform.SetSiblingIndex(transform.childCount - 1);
        _Ani.SetTrigger("_Flash");
    }
}