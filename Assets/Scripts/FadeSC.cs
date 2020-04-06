using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSC : MonoBehaviour
{
    public float _AlphaT;
    public float _RGBT;
    public float _FadeSpeed;
    private SpriteRenderer _SR;
    // Start is called before the first frame update
    void Start()
    {
        _SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float _AlphaV;
        _AlphaV = Mathf.MoveTowards(_SR.color.a, _AlphaT, _FadeSpeed * Time.unscaledDeltaTime);
        float _RGBV;
        _RGBV = Mathf.MoveTowards(_SR.color.r, _RGBT, _FadeSpeed * Time.unscaledDeltaTime);
        _SR.color = new Color(_RGBV, _RGBV, _RGBV, _AlphaV);
    }
}
