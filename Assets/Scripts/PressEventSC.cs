using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressEventSC : MonoBehaviour
{
    public bool _StartEvent;
    public int _SwordCount;
    // Start is called before the first frame update
    void Start()
    {
        if (_StartEvent == true)
        {
            StartCoroutine(_StartC());
        }
    }
    IEnumerator _StartC()
    {
        while (StageManagerSC._LumiaInst == null)
        {
            yield return null;
        }
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._SwordMax = 0;
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._SwordStock = 0;
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._UpdateBackSwords();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "LumiaHitbox")
        {
            StartCoroutine(_GetSword());
        }
    }
    IEnumerator _GetSword()
    {
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._CanControl = false;
        GetComponent<Animator>().SetTrigger("_GoFast");
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().SetTrigger("_End");
        yield return new WaitForSeconds(2f);
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._CanControl = true;
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._SwordMax = _SwordCount;
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._SwordStock = _SwordCount;
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._UpdateBackSwords();
        StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._Canvas.GetComponent<PauseSC>()._UpdateSwordMax();
        gameObject.SetActive(false);
    }
}
