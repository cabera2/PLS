using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using Lumia;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadButtonSC : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] _NewGameLang;
    public int _FileNumber;
    public Text _Button;
    public GameObject _Fade;
    public GameObject _StatHolderPrefab;
    public float FadeSpeed;
    private bool _CheckExist;


    public string _Scene;
    public int _HP_Max;
    public float _PlayTime;
    public int _SwordMax;
    public int _Money;
    public bool _HaveVessel;
    public int _SlashAtkLv;
    public int _ShotAtkLv;
    public int _AtkSpeedLv;
    public int _WarpLv;
    public int _SwordSizeLv;
    public List<int> _PermanentFlag;


    void Start()
    {
        _CheckExist = File.Exists(Application.persistentDataPath + "/CharSave" + _FileNumber + ".dat");
        Debug.Log("Directory:" + Application.persistentDataPath);
        if (_CheckExist == false)
        {
            Debug.Log(_FileNumber + "번 파일이 없습니다");
            _Button.text = _NewGameLang[SysSaveSC._Language];
        }
        else
        {
            Debug.Log(_FileNumber + "번 파일이 발견되었습니다.");
            SysSaveSC._CharLoad(_FileNumber, GetComponent<LoadButtonSC>());
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void _Pressed()
    {
        Debug.Log(_FileNumber + "번 파일을 불러옵니다.");
        Cursor.visible = false;
        if (_CheckExist == false)
        {
            if (SceneManager.GetActiveScene().name == "OpeningStage")
            {
                GameObject Slime = GameObject.Find("Slime");
                Slime.GetComponent<LumiaSlimeSC>()._OpEvent();
                StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._FileNumber = _FileNumber;
            }
            else
            {
                StartCoroutine(_LoadScene("OpeningStage"));
            }
        }
        else
        {
            SysSaveSC._Load_Required = true;
            SysSaveSC._Loaded_FileNumber = _FileNumber;
            SysSaveSC._Loaded_SavedScene = _Scene;
            SysSaveSC._Loaded_HP_Max = _HP_Max;
            SysSaveSC._Loaded_PlayTime = _PlayTime;
            SysSaveSC._Loaded_SwordMax = _SwordMax;
            SysSaveSC._Loaded_Money = _Money;
            SysSaveSC._Loaded_HaveVessel = _HaveVessel;
            SysSaveSC._Loaded_SlashAtkLv = _SlashAtkLv;
            SysSaveSC._Loaded_ShotAtkLv = _ShotAtkLv;
            SysSaveSC._Loaded_AtkSpeedLv = _AtkSpeedLv;
            SysSaveSC._Loaded_WarpLv = _WarpLv;
            SysSaveSC._Loaded_SwordSizeLv = _SwordSizeLv;
            SysSaveSC._Loaded_PermanentFlag = _PermanentFlag;
            StartCoroutine(_LoadScene(_Scene));
        }
    }
    IEnumerator _LoadScene(string _TargetScene)
    {
        Debug.Log(_FileNumber + "번 파일의 씬" + _TargetScene + "를 불러옵니다.");
        _Fade.SetActive(true);
        while (_Fade.GetComponent<CanvasGroup>().alpha < 1f)
        {
            yield return null;
        }
        AsyncOperation op = SceneManager.LoadSceneAsync(_TargetScene);
        op.allowSceneActivation = false;
        while (op.progress < 0.5f)
        {
            yield return null;
        }
        op.allowSceneActivation = true;
        Destroy(transform.parent.parent.gameObject);
        Debug.Log(_FileNumber + "번 파일의 씬" + _TargetScene + "를 불러왔습니다.");
    }
    public void _ChangeText()
    {
        _Button.text = "Scene: " + _Scene + " / HP: " + _HP_Max + " / Sword: " + _SwordMax + " / Money: " + _Money;
    }
    public void _DeleteFile()
    {
        File.Delete(Application.persistentDataPath + "/CharSave" + _FileNumber + ".dat");
        _Button.text = _NewGameLang[SysSaveSC._Language];
        _CheckExist = false;
    }
}
