using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemySC : EnemyParentSC
{
    public float _Speed;
    public float _DetectRange;
    public LayerMask _GroundLayer;
    private Vector3 WallDetectPos;
    private Vector3 EdgeDetectPos;
    private float _HalfWidth;

    // Use this for initialization
    void Start()
    {
        base._Caching();
        if (GetComponent<BoxCollider2D>() != null)
        {
            _HalfWidth = GetComponent<BoxCollider2D>().size.x / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Common._HP > 0)
        {
            if (_Speed < 0)
            {
                WallDetectPos = transform.position + new Vector3(-_HalfWidth, 0.3f, 0f);
                EdgeDetectPos = transform.position + new Vector3(-_HalfWidth, 0f, 0f);
                _SR.flipX = false;
            }
            else if (_Speed > 0)
            {
                WallDetectPos = transform.position + new Vector3(_HalfWidth, 0.3f, 0f);
                EdgeDetectPos = transform.position + new Vector3(_HalfWidth, 0f, 0f);
                _SR.flipX = true;
            }
            bool _OnGround = Physics2D.OverlapCircle(transform.position, _DetectRange, _GroundLayer);
            if (_OnGround == true)
            {
                bool _HitWall = Physics2D.OverlapCircle(WallDetectPos, _DetectRange, _GroundLayer);
                bool _ReachEdge = Physics2D.OverlapCircle(EdgeDetectPos, _DetectRange, _GroundLayer);
                if (_HitWall == true || _ReachEdge == false)
                {
                    _Speed *= -1;
                }
                _RB.velocity = new Vector2(_Speed, 0f);
            }
        }
        else
        {
            enabled = false;
        }
    }
}
