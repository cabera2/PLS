using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusWinSC : MonoBehaviour
{
    public Image Vessel;
    public Sprite[] VesselImg;
    public GameObject _BlueCircle;
    private ParticleSystem _PS;
    public float _BlueCircleSpeed;
    public float _ResizeSpeed;
    public GameObject[] _Icons;
    private RectTransform[] _IconRect;
    public Text[] _TextObj;
    private string[] _SkillNames;
    public string[] _SkillNamesKO;
    public string[] _SkillNamesJP;
    public CostTableSC[] _Costs;
    private int _CurrentCost;
    public AudioClip[] _SFX;
    private AudioSource _AS;
    // Start is called before the first frame update
    void Start()
    {
        _PS = _BlueCircle.GetComponent<ParticleSystem>();
        _AS = StageManagerSC._LumiaInst.GetComponent<AudioSource>();
        _IconRect = new RectTransform[_Icons.Length];
        for (int i = 0; i < _Icons.Length; i++)
        {
            _IconRect[i] = _Icons[i].GetComponent<RectTransform>();
            _GaugeUpdate(i);
        }
    }
    void _GaugeUpdate(int _Num)
    {
        int _LvlValue = 0;
        if (_Num == 0)
        {
            _LvlValue = StageManagerSC._LSC._SlashAtkLv;
        }
        else if (_Num == 1)
        {
            _LvlValue = StageManagerSC._LSC._SwordSizeLv;
        }
        else if (_Num == 2)
        {
            _LvlValue = StageManagerSC._LSC._AtkSpeedLv;
        }
        else if (_Num == 3)
        {
            _LvlValue = StageManagerSC._LSC._ShotAtkLv;
        }
        for (int i = 0; i < _Icons[_Num].transform.childCount; i++)
        {
            GameObject _Gauge = _Icons[_Num].transform.GetChild(i).gameObject;
            if (i <= _LvlValue)
            {
                _Gauge.SetActive(true);
            }
            else
            {
                _Gauge.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _BlueCircle.transform.position = Vector2.MoveTowards(_BlueCircle.transform.position, EventSystem.current.currentSelectedGameObject.transform.position, _BlueCircleSpeed);
        for (int i = 0; i < _Icons.Length; i++)
        {
            if (_Icons[i] == EventSystem.current.currentSelectedGameObject)
            {
                _IconRect[i].localScale = Vector3.MoveTowards(_IconRect[i].localScale, new Vector3(1f, 1f, 1f), _ResizeSpeed);
            }
            else
            {
                _IconRect[i].localScale = Vector3.MoveTowards(_IconRect[i].localScale, new Vector3(0.8f, 0.8f, 1f), _ResizeSpeed);
            }
        }
    }
    void OnEnable()
    {
        if (SysSaveSC._Language == 1)
        {
            _SkillNames = _SkillNamesJP;
        }
        else
        {
            _SkillNames = _SkillNamesKO;
        }

        EventSystem.current.SetSelectedGameObject(_Icons[0]);
        _SelectChange(0);
        _UpdateCost(0);
        if (StageManagerSC._LSC._HaveVessel == false)
        {
            Vessel.sprite = VesselImg[0];
        }
        else
        {
            Vessel.sprite = VesselImg[1];
        }
    }
    public void _LevelUp(int _Num)
    {
        int _LvlValue = 0;
        if (_Num == 0)
        {
            _LvlValue = StageManagerSC._LSC._SlashAtkLv;
        }
        else if (_Num == 1)
        {
            _LvlValue = StageManagerSC._LSC._SwordSizeLv;
        }
        else if (_Num == 2)
        {
            _LvlValue = StageManagerSC._LSC._AtkSpeedLv;
        }
        else if (_Num == 3)
        {
            _LvlValue = StageManagerSC._LSC._ShotAtkLv;
        }
        if (_CurrentCost <= StageManagerSC._LSC._Money && _Costs[_Num]._Cost.Length > _LvlValue)
        {
            _AS.PlayOneShot(_SFX[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            if (_Num == 0 && StageManagerSC._LSC._SlashAtkLv < StageManagerSC._LSC._SlashAtkVar.Length)
            {
                StageManagerSC._LSC._SlashAtkLv += 1;
            }
            else if (_Num == 1 && StageManagerSC._LSC._SwordSizeLv < StageManagerSC._LSC._SwordSizeVar.Length)
            {
                StageManagerSC._LSC._SwordSizeLv += 1;
            }
            else if (_Num == 2 && StageManagerSC._LSC._AtkSpeedLv < StageManagerSC._LSC._AtkSpeedVar.Length)
            {
                StageManagerSC._LSC._AtkSpeedLv += 1;
            }
            else if (_Num == 3 && StageManagerSC._LSC._ShotAtkLv < StageManagerSC._LSC._ShotAtkVar.Length)
            {
                StageManagerSC._LSC._ShotAtkLv += 1;
            }
            _PS.Play();
            StageManagerSC._LSC._Money -= _CurrentCost;
            _GaugeUpdate(_Num);
            _UpdateCost(_Num);
        }
        else
        {
            _AS.PlayOneShot(_SFX[1], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            StartCoroutine(_Shake(_Num));
        }
    }
    public void _SelectChange(int _Num)
    {
        if (_SkillNames[_Num] != null)
        {
            _TextObj[0].text = _SkillNames[_Num];
        }
        _UpdateCost(_Num);
    }
    public void _UpdateCost(int _Num)
    {
        if (_Costs[_Num] != null)
        {
            int _LvlValue = 0;
            if (_Num == 0)
            {
                _LvlValue = StageManagerSC._LSC._SlashAtkLv;
            }
            else if (_Num == 1)
            {
                _LvlValue = StageManagerSC._LSC._SwordSizeLv;
            }
            else if (_Num == 2)
            {
                _LvlValue = StageManagerSC._LSC._AtkSpeedLv;
            }
            else if (_Num == 3)
            {
                _LvlValue = StageManagerSC._LSC._ShotAtkLv;
            }
            if (_Costs[_Num]._Cost.Length > _LvlValue)
            {
                _TextObj[1].text = "Cost: " + _Costs[_Num]._Cost[_LvlValue].ToString();
                _CurrentCost = _Costs[_Num]._Cost[_LvlValue];
                if (StageManagerSC._LSC._Money < _Costs[_Num]._Cost[_LvlValue])
                {
                    _TextObj[1].color = new Color(0.5f, 0.5f, 0.5f, 1f);

                }
                else
                {
                    _TextObj[1].color = new Color(1f, 1f, 1f, 1f);
                }
            }
            else
            {
                _TextObj[1].color = new Color(0.5f, 0.5f, 0.5f, 1f);
                if (SysSaveSC._Language == 1)
                {
                    _TextObj[1].text = "最大レベル";
                }
                else
                {
                    _TextObj[1].text = "최대 레벨";
                }
            }



        }
    }
    IEnumerator _Shake(int _Num)
    {
        Vector3 _OriginPos = _Icons[_Num].transform.position;
        for (int i = 0; i < 10; i++)
        {
            _Icons[_Num].transform.position = _OriginPos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            yield return null;
        }
        _Icons[_Num].transform.position = _OriginPos;
    }
}
