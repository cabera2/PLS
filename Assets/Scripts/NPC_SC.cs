using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_SC : MonoBehaviour
{
    public float[] _TalkPos;

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
            if (_TalkPos.Length == 1)
            {
                _TargetPos = _TalkPos[0];
            }
            else if (_TalkPos.Length == 2)
            {
                if (StageManagerSC._LumiaInst.transform.position.x <= transform.position.x)
                {
                    _TargetPos = _TalkPos[0];
                }
                else
                {
                    _TargetPos = _TalkPos[1];
                }
            }
            StartCoroutine(_MoveToPos(_TargetPos));
        }
    }
    IEnumerator _MoveToPos(float _TargetPos2)
    {
        float _Dir1 = (transform.position.x + _TargetPos2) - StageManagerSC._LumiaInst.transform.position.x;
        int _Dir2 = _Dir1 > 0 ? 1 : _Dir1 < 0 ? -1 : 0;
        StageManagerSC._LumiaInst.GetComponent<Lumia_SC>()._AutoWalk = _Dir2;
        while (0 < ((transform.position.x + _TargetPos2) - StageManagerSC._LumiaInst.transform.position.x) * _Dir2)
        {
            yield return null;
        }
        StageManagerSC._LumiaInst.GetComponent<Lumia_SC>()._AutoWalk = 0;
        if (transform.position.x > StageManagerSC._LumiaInst.transform.position.x)
        {
            StageManagerSC._LumiaInst.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            StageManagerSC._LumiaInst.GetComponent<SpriteRenderer>().flipX = true;
        }
        GetComponent<DialogueSC>()._StartTalk();
    }
}
