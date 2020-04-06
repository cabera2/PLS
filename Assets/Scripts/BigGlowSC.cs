using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGlowSC : MonoBehaviour
{
    private GameObject _Camera;
    public float _FadeDistance;
    private SpriteRenderer _SR;
    private float _Alpha;
    private float _FadeSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        _SR = GetComponent<SpriteRenderer>();
        if (_Camera == null)
        {
            _Camera = StageManagerSC._WorkingCam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Camera != null)
        {
            if (Vector2.Distance(transform.position, _Camera.transform.position) < _FadeDistance && _Alpha > 0)
            {
                _Alpha -= _FadeSpeed * Time.deltaTime;
                _SR.color = new Color(_SR.color.r, _SR.color.g, _SR.color.b, _Alpha);
            }
            else if (Vector2.Distance(transform.position, _Camera.transform.position) >= _FadeDistance && _Alpha < 0.8f)
            {
                _Alpha += _FadeSpeed * Time.deltaTime;
                _SR.color = new Color(_SR.color.r, _SR.color.g, _SR.color.b, _Alpha);
            }
        }
    }
}
