using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_ControlSC : MonoBehaviour
{
    public Transform _Camera;
    public Transform _BaseBG;
    public float _PosMultiply;
    private float _Z_Pos;
    // Use this for initialization
    void Start()
    {
        _Z_Pos = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 TargetPos = (_BaseBG.position - _Camera.position) * _PosMultiply;
        transform.position = new Vector3(TargetPos.x, TargetPos.y, _Z_Pos);
    }
}
