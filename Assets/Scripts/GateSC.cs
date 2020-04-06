using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GateSC : MonoBehaviour
{
    public string _TargetScene;
    public int _TargetGateNumber;
    public bool _WallGate;
    public int _ForceDir;
    public LayerMask _GroundLayer;
    // Start is called before the first frame update
    void Start()
    {
        _WallGate = Physics2D.OverlapCircle(transform.position, 0.3f, _GroundLayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
