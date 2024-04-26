using System.Collections;
using System.Collections.Generic;
using Lumia;
using UnityEngine;

public class DebugEventSC : MonoBehaviour
{
    private LumiaSC _LSC;
    // Start is called before the first frame update
    void Start()
    {
        _LSC = StageManagerSC._LumiaInst.GetComponent<LumiaSC>();
        _LSC._SwordMax = 5;
        _LSC._SwordStock = 5;
        _LSC._Canvas.GetComponent<PauseSC>()._UpdateSwordMax();
        _LSC._Canvas.GetComponent<PauseSC>()._UpdateSwordCurrent();
        transform.parent.GetComponent<DialogueSC>()._StartLine = 16;
        transform.parent.GetComponent<DialogueSC>()._EndLine = 16;
    }
}
