using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Lumia;

public class SysSaveSC : MonoBehaviour
{
    [Serializable]
    class SysSaveCon
    {
        public int _Language;
        public int _LastFile;
        public int _Vol_Master = 10;
        public int _Vol_BGM = 10;
        public int _Vol_SFX = 10;
        public KeyCode[] _Keys;
    }
    [Serializable]
    class CharSaveCon
    {
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
    }
    public static int _Language = 0;
    public static int _LastFile = 0;
    public static int _Vol_Master = 10;
    public static int _Vol_BGM = 10;
    public static int _Vol_SFX = 10;
    public static KeyCode[] _Keys = new KeyCode[]
{
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.Tab,//Map
        KeyCode.Z,//Jump
        KeyCode.X,//Slash
        KeyCode.C,//Shoot
        KeyCode.F,//Teleport
        KeyCode.Z,//Submit
        KeyCode.X,//Cancel
        KeyCode.Escape,//Pause
        KeyCode.I,//Status
        KeyCode.D,//Warp
        KeyCode.S//Shield
};
    public static bool _Load_Required;
    public static int _Loaded_FileNumber;
    public static string _Loaded_SavedScene;
    public static int _Loaded_HP_Max;
    public static float _Loaded_PlayTime;
    public static int _Loaded_SwordMax;
    public static int _Loaded_Money;
    public static bool _Loaded_HaveVessel;
    public static int _Loaded_SlashAtkLv;
    public static int _Loaded_ShotAtkLv;
    public static int _Loaded_AtkSpeedLv;
    public static int _Loaded_WarpLv;
    public static int _Loaded_SwordSizeLv;
    public static List<int> _Loaded_PermanentFlag;
    public static bool _Loading = false;
    
    // private nn.fs.FileHandle fileHandle = new nn.fs.FileHandle();
    // private nn.hid.NpadState npadState;
    // private nn.hid.NpadId[] npadIds = { nn.hid.NpadId.Handheld, nn.hid.NpadId.No1 };
    
    public static void _SysSave()
    {
        BinaryFormatter _BF = new BinaryFormatter();
        SysSaveCon data = new SysSaveCon();
        data._Language = _Language;
        data._LastFile = _LastFile;
        data._Vol_Master = _Vol_Master;
        data._Vol_BGM = _Vol_BGM;
        data._Vol_SFX = _Vol_SFX;
        data._Keys = _Keys;
#if UNITY_STANDALONE_WIN
        FileStream file = File.Create(Application.persistentDataPath + "/SysSave.dat");
        _BF.Serialize(file, data);
        file.Close();
        Debug.Log("시스템 설정이 저장되었습니다. 언어:" + _Language + "마지막 파일" + _LastFile);
#endif
#if UNITY_SWITCH
        // nn.account.Account.Initialize();
        // nn.account.UserHandle userHandle = new nn.account.UserHandle();
#endif
    }
    public static void _SysLoad()
    {
        BinaryFormatter _BF = new BinaryFormatter();
        bool _CheckExist = false;
#if UNITY_STANDALONE_WIN
        _CheckExist = File.Exists(Application.persistentDataPath + "/SysSave.dat");
        if (_CheckExist)
        {
            FileStream file = File.Open(Application.persistentDataPath + "/SysSave.dat", FileMode.Open);
            SysSaveCon data = (SysSaveCon)_BF.Deserialize(file);
            _Language = data._Language;
            _LastFile = data._LastFile;
            _Vol_Master = data._Vol_Master;
            _Vol_BGM = data._Vol_BGM;
            _Vol_SFX = data._Vol_SFX;
            if (data._Keys != null && _Keys.Length == data._Keys.Length)
            {
                _Keys = data._Keys;
            }
            file.Close();
            Debug.Log("파일 발견. 언어:" + _Language + "마지막 파일" + _LastFile);
        }
        else
        {
            if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                _Language = 1;
            }
            Debug.Log("파일이 없습니다.");
        }
        _Loading = false;
        Debug.Log("시스템 설정이 로드 완료. 음량: " + _Vol_Master + "/" + _Vol_BGM + "/" + _Vol_SFX);
#endif
    }
    public static void _CharSave()
    {
        LumiaSC _LSC = StageManagerSC._LumiaInst.GetComponent<LumiaSC>();
        BinaryFormatter _BF = new BinaryFormatter();
        
        CharSaveCon data = new CharSaveCon();
        data._Scene = _LSC._SavedScene;
        data._HP_Max = _LSC._Hitbox.GetComponent<LumiaHitboxSC>()._HP_Max;
        data._PlayTime = _LSC._PlayTime;
        data._SwordMax = _LSC._SwordMax;
        data._Money = _LSC._Money;
        data._HaveVessel = _LSC._HaveVessel;
        data._SlashAtkLv = _LSC.levelData.attackLv;
        data._ShotAtkLv = _LSC.levelData.shotLv;
        data._AtkSpeedLv = _LSC.levelData.atkSpeedLv;
        data._WarpLv = _LSC.levelData.warpLv;
        data._SwordSizeLv = _LSC.levelData.swordSizeLv;
        data._PermanentFlag = _LSC._PermanentFlag;
#if UNITY_STANDALONE_WIN
        FileStream file = File.Create(Application.persistentDataPath + "/CharSave" + _LSC._FileNumber + ".dat");
        _BF.Serialize(file, data);
        file.Close();
        Debug.Log("캐릭터 데이터가 " + _LSC._FileNumber + "번 파일에 저장되었습니다." + data._HP_Max);
#endif
    }
    public static void _CharLoad(int _FileNumber, LoadButtonSC LBSC)
    {
#if UNITY_STANDALONE_WIN
        BinaryFormatter _BF = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/CharSave" + _FileNumber + ".dat", FileMode.Open);
        CharSaveCon data = (CharSaveCon)_BF.Deserialize(file);
        LBSC._Scene = data._Scene;
        LBSC._HP_Max = data._HP_Max;
        LBSC._PlayTime = data._PlayTime;
        LBSC._SwordMax = data._SwordMax;
        LBSC._Money = data._Money;
        LBSC._HaveVessel = data._HaveVessel;
        LBSC._SlashAtkLv = data._SlashAtkLv;
        LBSC._ShotAtkLv = data._ShotAtkLv;
        LBSC._AtkSpeedLv = data._AtkSpeedLv;
        LBSC._WarpLv = data._WarpLv;
        LBSC._SwordSizeLv = data._SwordSizeLv;
        LBSC._PermanentFlag = data._PermanentFlag;
        file.Close();
        LBSC._ChangeText();
#endif
    }
}
