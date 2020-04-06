﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealSwordSC : MonoBehaviour
{
    public GameObject _NewSkillGuideObj;
    public Transform _Canvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (StageManagerSC._LSC != null && Mathf.Abs(StageManagerSC._LSC._UpDownInput) > 0.5f && GetComponent<GetArrowSC>()._Enabled == true)
        {
            StageManagerSC._LumiaInst.GetComponent<Lumia_SC>()._CanControl = false;
            float _TargetPos = 0;
            StartCoroutine(_MoveToPos(_TargetPos));
        }
    }
    IEnumerator _MoveToPos(float _TargetPos2)
    {
        if (GetComponent<MapSaverSC>() != null)
        {
            Debug.Log("검 획득," + GetComponent<MapSaverSC>()._ID + "플래그 획득");
            GetComponent<MapSaverSC>()._SaveStatus();
        }
        float _Dir1 = (transform.position.x + _TargetPos2) - StageManagerSC._LumiaInst.transform.position.x;
        int _Dir2 = _Dir1 > 0 ? 1 : _Dir1 < 0 ? -1 : 0;
        StageManagerSC._LumiaInst.GetComponent<Lumia_SC>()._AutoWalk = _Dir2;
        while (0 < ((transform.position.x + _TargetPos2) - StageManagerSC._LumiaInst.transform.position.x) * _Dir2)
        {
            yield return null;
        }
        StageManagerSC._LumiaInst.GetComponent<Lumia_SC>()._AutoWalk = 0;

        GetComponent<Animator>().SetTrigger("_GoFast");
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().SetTrigger("_End");
        yield return new WaitForSeconds(2f);

    }
    public void _NewSkillGuide()
    {

        if (StageManagerSC._LSC != null)
        {
            StageManagerSC._LSC._SwordMax += 1;
            StageManagerSC._LSC._SwordStock += 1;
            StageManagerSC._LSC._Canvas.GetComponent<PauseSC>()._UpdateSwordMax();
            StageManagerSC._LSC._Canvas.GetComponent<PauseSC>()._UpdateSwordCurrent();
            StageManagerSC._LSC._UpdateBackSwords();
        }
        if (_Canvas != null && _NewSkillGuideObj != null && StageManagerSC._LSC._SwordMax <= 5)
        {
            GameObject _Guide = Instantiate(_NewSkillGuideObj);
            _Guide.transform.parent = _Canvas;
            _Guide.transform.position = _Canvas.position;
            _Guide.transform.localScale = new Vector3(1f, 1f, 1f);
            _Guide.GetComponent<NewSkillGuideSC>()._Run(StageManagerSC._LSC._SwordMax);
        }
        else
        {
            StageManagerSC._LSC._CanControl = true;

        }
        gameObject.SetActive(false);
    }
}
