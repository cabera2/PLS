using System.Collections;
using System.Collections.Generic;
using Lumia;
using UnityEngine;

using UnityEngine.UI;

public class DialogueSC : MonoBehaviour
{
    public string[] _FileName;
    public int _StartLine;
    public int _EndLine;
    private CanvasGroup _TextWin;
    private Text _TextObj;
    private GameObject _TalkArrow;
    public string[] _TextStrings;
    private int _LastLang;
    private bool _Talking;
    public int _CurrentText;
    private int _CharCount;
    public GameObject _Lumia;
    public List<GameObject> _EndActive = new List<GameObject>();
    public bool _StartByTouch;
    public bool _EndDestroy;
    public bool _EndRebound;
    private readonly MyInputManager _myInput = new();
    void Update()
    {
        if (_Talking == true)
        {
            if (_myInput.GetButtonDown(KeyType.Submit) || _myInput.GetButtonDown(KeyType.Cancel))
            {
                if (_CharCount < _TextStrings[_CurrentText].Length)
                {
                    _CharCount = _TextStrings[_CurrentText].Length;
                    _TextObj.text = _TextStrings[_CurrentText].Replace("\\n", "\n");
                }
                else
                {
                    _CheckEnd();
                    _CharCount = 0;
                    if (_CurrentText < _EndLine)
                    {
                        _CurrentText += 1;
                        StartCoroutine(WriteText());
                    }
                    else
                    {
                        StartCoroutine(TextWinFadeOut());
                        if (_EndRebound == true)
                        {
                            SpriteRenderer _SR = _Lumia.GetComponent<SpriteRenderer>();
                            StageManagerSC._lumiaSc._AutoWalk = _SR.flipX == true ? 1f : _SR.flipX == false ? -1f : 0;
                            Invoke("_ControlUnlock", 0.5f);
                        }

                        if (_EndActive.Count > 0)
                        {
                            for (int i = 0; i < _EndActive.Count; i++)
                            {
                                _EndActive[i].SetActive(true);
                            }
                        }
                    }
                }
            }
            if (_CharCount >= _TextStrings[_CurrentText].Length && _TalkArrow.GetComponent<CanvasGroup>().alpha < 1)
            {
                _TalkArrow.GetComponent<CanvasGroup>().alpha += 5 * Time.unscaledDeltaTime;
            }
            else if (_CharCount < _TextStrings[_CurrentText].Length)
            {
                _TalkArrow.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }
    void _CheckEnd()
    {
        if (_CurrentText == _EndLine - 1)
        {
            _TalkArrow.GetComponent<Animator>().SetBool("_End", true);
        }
        else
        {
            _TalkArrow.GetComponent<Animator>().SetBool("_End", false);
        }
    }
    public void _StartTalk()
    {
        _CurrentText = _StartLine;
        Debug.Log("대화 시작. 첫 줄은" + _StartLine + "현재 줄은" + _CurrentText);
        _Talking = true;
        if (StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._TalkWinInst == null)
        {
            GameObject _NewTalkWin = Instantiate(StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._TalkWinPrefab);
            _NewTalkWin.transform.SetParent(StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._LocalCanvas.transform, false);
            StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._TalkWinInst = _NewTalkWin;
        }
        if (_TextWin == null)
        {
            _TextWin = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._TalkWinInst.GetComponent<CanvasGroup>();
        }
        if (_TextObj == null)
        {
            _TextObj = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._TalkWinInst.transform.GetChild(0).GetComponent<Text>();
        }
        if (_TalkArrow == null)
        {
            _TalkArrow = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._TalkWinInst.transform.GetChild(1).gameObject;
        }

        if (StageManagerSC._lumiaSc != null && StageManagerSC._lumiaSc._Canvas == true)
        {
            if (StageManagerSC._lumiaSc._Canvas.activeSelf == true)
            {
                StartCoroutine(StageManagerSC._lumiaSc._Canvas.GetComponent<PauseSC>()._Fade(false)); 
            }
        }
        if (StageManagerSC._lumiaSc != null)
        {
            StageManagerSC._lumiaSc._CanControl = false;
            StageManagerSC._lumiaSc.leftStickX = 0;
        }
        else if (_Lumia.GetComponent<LumiaSlimeSC>() != null)
        {
            _Lumia.GetComponent<LumiaSlimeSC>()._CanControl = false;
            _Lumia.GetComponent<LumiaSlimeSC>()._MoveInput = 0;
        }
        if (_TextStrings == null || _TextStrings.Length == 0 || _LastLang != SysSaveSC._Language)
        {
            _LastLang = SysSaveSC._Language;
            _TextStrings = System.IO.File.ReadAllLines(Application.streamingAssetsPath + @"/Texts/" + _FileName[SysSaveSC._Language] + ".txt");
        }
        _TextObj.text = null;
        _CheckEnd();
        StartCoroutine(TextWinFadeIn());
        StartCoroutine(WriteText());
    }
    IEnumerator TextWinFadeIn()
    {
        while (_TextWin.alpha < 1)
        {
            float _FadeSpeed = 3;
            _TextWin.alpha += _FadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator TextWinFadeOut()
    {
        if (StageManagerSC._lumiaSc != null)
        {
            StartCoroutine(StageManagerSC._lumiaSc._Canvas.GetComponent<PauseSC>()._Fade(true)); 
        }
        while (_TextWin.alpha > 0)
        {
            float _FadeSpeed = 3;
            _TextWin.alpha -= _FadeSpeed * Time.deltaTime;
            yield return null;
        }
        if (StageManagerSC._lumiaSc != null)
        {
            StageManagerSC._lumiaSc._CanControl = true;
        }
        else if (_Lumia.GetComponent<LumiaSlimeSC>() != null)
        {
            _Lumia.GetComponent<LumiaSlimeSC>()._CanControl = true;
        }
        _Talking = false;
        if (_EndDestroy == true)
        {
            Destroy(gameObject);
        }
    }
    void _ControlUnlock()
    {
        StageManagerSC._lumiaSc._CanControl = true;
        StageManagerSC._lumiaSc._AutoWalk = 0;
    }
    IEnumerator WriteText()
    {
        while (_CharCount < _TextStrings[_CurrentText].Length)
        {
            _CharCount += 1;
            _TextObj.text = _TextStrings[_CurrentText].Substring(0, _CharCount).Replace("\\n", "\n");
            yield return new WaitForSeconds(0.05f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Lumia" && _StartByTouch == true)
        {
            if (_Lumia == null)
            {
                _Lumia = col.gameObject;
            }
            _StartTalk();
        }
    }
}
