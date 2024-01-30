using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManagerSC : MonoBehaviour
{
    public bool _GenerateAtStart;
    public GameObject _LumiaPrefab;
    public static GameObject _LumiaInst;
    public static GameObject _WorkingCam;
    public static GameObject _CanvasInst;
    public static LumiaSC _LSC;
    public static LumiaCamSC _CamSC;
    public GameObject _TargetMarkPrefab;
    public GameObject _CanvasPrefab;
    public AudioClip _StageBGM;
    public Vector2 _CamMinPos;
    public Vector2 _CamMaxPos;
    public Vector2 _StartPos;
    public Vector2 _MapOffset;
    public static Vector2 _MapOffsetNow;
    public Transform[] _Gates;
    public GameObject _Chair;
    public GameObject _LocalCanvas;
    public GameObject _InteractArrowPrefab;
    public GameObject _InteractArrowInst;
    public GameObject _TalkWinPrefab;
    public GameObject _TalkWinInst;
    [Header("Pools")]
    [HideInInspector] public List<GameObject> _SwordPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> _WarpParticlePool = new List<GameObject>();
    [HideInInspector] public List<GameObject> _HitFX1Pool = new List<GameObject>();
    [HideInInspector] public List<GameObject> _HitFX2Pool = new List<GameObject>();
    [HideInInspector] public List<GameObject> _SlashPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> _JumpDustPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> _CoinPool = new List<GameObject>();
    [HideInInspector] public List<GameObject> _CoinFXPool = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _MapOffsetNow = _MapOffset;
        _WorkingCam = gameObject;
        if (_GenerateAtStart == true && _LumiaInst == null)
        {
            Debug.Log("루미아 생성");
            //필수 Instantiate 생성
            _LumiaInst = Instantiate(_LumiaPrefab);
            _LSC = _LumiaInst.GetComponent<LumiaSC>();
            GameObject _TargetMark = Instantiate(_TargetMarkPrefab);
            _CanvasInst = Instantiate(_CanvasPrefab);

            //주인공 캐싱
            _LSC._TargetMark = _TargetMark;
            _LSC._Canvas = _CanvasInst;
            LumiaHitboxSC _LHBSC = _LSC._Hitbox.GetComponent<LumiaHitboxSC>();
            _LHBSC._Canvas = _CanvasInst;
            _LHBSC._FadeObj = _CanvasInst.GetComponent<PauseSC>()._FadeObj;


            //저장된 능력치 로드
            if (SysSaveSC._Load_Required == true)
            {
                SysSaveSC._Load_Required = false;
                _LHBSC._HP_Max = SysSaveSC._Loaded_HP_Max;
                _LHBSC._HP_Current = SysSaveSC._Loaded_HP_Max;
                _LSC._SavedScene = SysSaveSC._Loaded_SavedScene;
                _LSC._FileNumber = SysSaveSC._Loaded_FileNumber;
                _LSC._PlayTime = SysSaveSC._Loaded_PlayTime;
                _LSC._SwordMax = SysSaveSC._Loaded_SwordMax;
                _LSC._SwordStock = SysSaveSC._Loaded_SwordMax;
                _LSC._Money = SysSaveSC._Loaded_Money;
                _LSC._HaveVessel = SysSaveSC._Loaded_HaveVessel;
                _LSC._SlashAtkLv = SysSaveSC._Loaded_SlashAtkLv;
                _LSC._ShotAtkLv = SysSaveSC._Loaded_ShotAtkLv;
                _LSC._AtkSpeedLv = SysSaveSC._Loaded_AtkSpeedLv;
                _LSC._WarpLv = SysSaveSC._Loaded_WarpLv;
                _LSC._SwordSizeLv = SysSaveSC._Loaded_SwordSizeLv;
                _LSC._PermanentFlag = SysSaveSC._Loaded_PermanentFlag;
            }
            
            //의자 존재여부 확인
            if (_Chair != null && _Chair.activeSelf == true)
            {                
                _ChairStart();
            }
            else if (_StartPos != Vector2.zero)
            {
                _LumiaInst.transform.position = _StartPos;
            }

            //시스템 설정 불러오기
            SysSaveSC._SysLoad();
        }

        //스테이지 초기화
        if (_LumiaInst != null)
        {
            if (_LSC == null)
            {
                _LSC = _LumiaInst.GetComponent<LumiaSC>();
            }
            _LSC._MyCamera = gameObject;
            _CamSC = _LumiaInst.GetComponent<LumiaCamSC>();
            _CamSC._MyCamera = gameObject;
            _LSC._WhenSceneLoad();
            _CamSC._CameraControl();
            transform.position = _LumiaInst.GetComponent<LumiaCamSC>()._CamPos1;
        }
    }
    public void _ChairStart()
    {
        _LumiaInst.transform.position = _Chair.transform.position;
        _LumiaInst.GetComponent<Animator>().SetBool("_Sitting", true);
        _LumiaInst.GetComponent<LumiaSC>()._CanControl = false;
        _Chair.GetComponent<ChairSC>()._CanStand = true;
        StageManagerSC._LSC._TemporaryFlag.Clear();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
