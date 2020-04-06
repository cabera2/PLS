using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LangChangeSC : MonoBehaviour
{
    // Start is called before the first frame update
    public Text _TextObj;
    public string[] _LangText;
    public void OnEnable()
    {
        if(_LangText[SysSaveSC._Language] != null){
            _TextObj.text = _LangText[SysSaveSC._Language].Replace("\\n", "\n");
        }
        else if(_LangText[SysSaveSC._Language] == null){
            _TextObj.text = _LangText[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
