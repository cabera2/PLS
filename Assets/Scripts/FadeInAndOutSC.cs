using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAndOutSC : MonoBehaviour
{
    private CanvasGroup _UIGroup;
    public float _FadeSpeed;
    public float _WaitTime;
    // Start is called before the first frame update
    void Awake()
    {
        _UIGroup = GetComponent<CanvasGroup>();
    }
    void OnEnable()
    {
        StartCoroutine(_StartFade());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator _StartFade()
    {
        while (_UIGroup.alpha < 1)
        {
            _UIGroup.alpha += _FadeSpeed * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(_WaitTime);
        while (_UIGroup.alpha > 0)
        {
            _UIGroup.alpha -= _FadeSpeed * Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
