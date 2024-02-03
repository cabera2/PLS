using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaverSC : MonoBehaviour
{
    public bool _Destroy;
    public bool _ReGenWhenSave;
    public int _ID;
    [HideInInspector] public bool _Used;
    // Start is called before the first frame update
    void Awake()
    {
        if (StageManagerSC._LumiaInst != null)
        {
            if (StageManagerSC._lumiaSc._TemporaryFlag.Contains(_ID) == true || StageManagerSC._lumiaSc._PermanentFlag.Contains(_ID) == true)
            {
                if (_Destroy == true)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _Used = true;
                }
            }
        }
        else
        {
            if (SysSaveSC._Loaded_PermanentFlag != null && SysSaveSC._Loaded_PermanentFlag.Contains(_ID) == true)
            {
                if (_Destroy == true)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _Used = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _SaveStatus()
    {
        if (_ReGenWhenSave == true)
        {
            StageManagerSC._lumiaSc._TemporaryFlag.Add(_ID);
        }
        else
        {
            StageManagerSC._lumiaSc._PermanentFlag.Add(_ID);
        }
    }
}
