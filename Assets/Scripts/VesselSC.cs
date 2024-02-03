using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselSC : MonoBehaviour
{
    public GameObject _GetVesselAni;
    private bool _Used;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_Used == false && col.gameObject.name == "LumiaHitbox")
        {
            _Used = true;
            StartCoroutine(_VesselGet());
        }
    }
    IEnumerator _VesselGet()
    {
        Time.timeScale = 0f;
        GameObject _FadeObj = StageManagerSC._CanvasInst.GetComponent<PauseSC>()._FadeObj;
        UIFaderSC _UIFader = _FadeObj.GetComponent<UIFaderSC>();

        _UIFader._FadeSpeed = 1;
        _UIFader._TargetAlpha = 0.5f;
        _FadeObj.SetActive(true);
        while (_FadeObj.GetComponent<CanvasGroup>().alpha < 0.5f)
        {
            yield return null;
        }
        GameObject _GetVesselAniInst = Instantiate(_GetVesselAni);
        _GetVesselAniInst.transform.SetParent(_FadeObj.transform.parent);
        _GetVesselAniInst.transform.position = _FadeObj.transform.position;
        _GetVesselAniInst.transform.localScale = new Vector3(1, 1, 1);
        Animator _GetAni = _GetVesselAniInst.GetComponent<Animator>();
        if (StageManagerSC._lumiaSc._HaveVessel == false)
        {
            StageManagerSC._lumiaSc._HaveVessel = true;
            _GetAni.SetTrigger("_Half");
            yield return new WaitForSecondsRealtime(3f);
        }
        else
        {
            StageManagerSC._lumiaSc._HaveVessel = false;
            _GetAni.SetTrigger("_Whole");
            LumiaHitboxSC _HBSC = StageManagerSC._lumiaSc._Hitbox.GetComponent<LumiaHitboxSC>();
            _HBSC._HP_Max += 1;
            yield return new WaitForSecondsRealtime(3f);
            StageManagerSC._CanvasInst.GetComponent<PauseSC>()._UpdateMaxHp();
            _HBSC._HP_Current = _HBSC._HP_Max;
            StageManagerSC._CanvasInst.GetComponent<PauseSC>()._UpdateCurrentHp();
        }
        if (GetComponent<MapSaverSC>() != null)
        {
            GetComponent<MapSaverSC>()._SaveStatus();
        }

        _UIFader._TargetAlpha = 1f;
        _UIFader._FadeOut();
        Transform _Child = transform.GetChild(0);
        _Child.GetComponent<SpriteRenderer>().enabled = false;
        _Child.GetComponent<ParticleSystem>().Stop();
        _Child.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
