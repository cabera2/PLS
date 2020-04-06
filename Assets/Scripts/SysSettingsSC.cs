using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SysSettingsSC : MonoBehaviour
{
    // Start is called before the first frame update

    private int _LanguageOld;
    private int _Vol_MasterOld;
    private int _Vol_BGMOld;
    private int _Vol_SFXOld;

    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _ChangeLangSet()
    {
        if (SysSaveSC._Language == 0)
        {
            SysSaveSC._Language = 1;
        }
        else if (SysSaveSC._Language == 1)
        {
            SysSaveSC._Language = 0;
        }
    }
    public void ConfirmSet()
    {
        SysSaveSC._SysSave();
    }
}
