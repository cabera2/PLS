using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTextSC : MonoBehaviour
{
    public GameObject _TargetObj;
    public Vector3 _Offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_TargetObj != null)
        {
            transform.position = _TargetObj.transform.position + _Offset;
        }
    }
}
