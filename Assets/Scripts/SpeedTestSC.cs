using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTestSC : MonoBehaviour
{
    private Rigidbody2D _RB;
    public Vector2 _RB_Velocity;
    public float _RB_Magnitude;
    public float _TF_Velocity;

    private Vector3 previous;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody2D>() == true)
        {
            _RB = GetComponent<Rigidbody2D>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        _RB_Velocity = _RB.velocity;
        _RB_Magnitude = _RB.velocity.magnitude;

        _TF_Velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
    }
}
