using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaderSC : MonoBehaviour
{
    private CanvasGroup _UIGroup;
    public float _FadeSpeed;
    public float _TargetAlpha = 1;
    [HideInInspector] public bool _Working;
    // Start is called before the first frame update
    void Awake()
    {
        _UIGroup = GetComponent<CanvasGroup>();
    }
    public void OnEnable()
    {
        StartCoroutine(_FadeInC());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _FadeOut()
    {
        if (gameObject.activeInHierarchy == true)
        {
            StartCoroutine(_FadeOutC());
        }
    }
    IEnumerator _FadeInC()
    {
        while (_Working == true)
        {
            yield return null;
        }
        _Working = true;
        while (_UIGroup.alpha < _TargetAlpha)
        {
            _UIGroup.alpha += _FadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        _Working = false;
    }
    IEnumerator _FadeOutC()
    {
        while (_Working == true)
        {
            yield return null;
        }
        _Working = true;
        while (_UIGroup.alpha > 0)
        {
            _UIGroup.alpha -= _FadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        _Working = false;
    }
}
