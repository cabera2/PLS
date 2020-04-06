using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSC : MonoBehaviour
{
    public float _WaveSpeed;
    public float _TargetAngle;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Test", 0, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(1 / Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad), 1 / Mathf.Cos(transform.localEulerAngles.x * Mathf.Deg2Rad));
        transform.rotation = Quaternion.Euler(60, Mathf.Lerp(transform.rotation.y, _TargetAngle, _WaveSpeed * Time.deltaTime), 0);
    }
    void Test(){
        _TargetAngle *= -1;
    }
}
