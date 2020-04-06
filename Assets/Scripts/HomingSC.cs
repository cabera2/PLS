using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingSC : MonoBehaviour
{
    public bool _Rotate = true;
    public Transform target;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    public Vector3 _Angle;
    private Rigidbody2D _RB;
    // Start is called before the first frame update
    void Start()
    {
        _RB = GetComponent<Rigidbody2D>();
        StartCoroutine(_StartC());
    }
    IEnumerator _StartC()
    {
        while (StageManagerSC._LumiaInst == null)
        {
            yield return null;
        }
        target = StageManagerSC._LumiaInst.transform;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - _RB.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            if (_Rotate == true)
            {
                _RB.angularVelocity = -rotateAmount * rotateSpeed;
                _RB.velocity = transform.up * speed;
            }
            else
            {
                _Angle = -rotateAmount * rotateSpeed * Vector3.up;
                _RB.velocity = _Angle.normalized * speed;
            }
        }
    }
}
