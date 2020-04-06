using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSC : MonoBehaviour
{
    public bool _On;
    public SpriteRenderer _GemSR;
    public Sprite _OnGem;
    public Vector2[] TargetPos = null;
    private int _next = 0;
    private float _eTime = 0.0f;
    public float _loopTime = 1.0f;
    public float _waitTime = 0.5f;

    void Start()
    {
        if (GetComponent<MapSaverSC>() != null && GetComponent<MapSaverSC>()._Used == true)
        {
            _On = true;
        }
        if (_On == true)
        {
            _GemSR.sprite = _OnGem;
            StartCoroutine(Move());
        }
    }
    public void _TurnOn()
    {
        _On = true;
        _GemSR.sprite = _OnGem;
        StartCoroutine(_TurnOnC());
    }
    IEnumerator _TurnOnC()
    {
        Transform _GemObj = _GemSR.transform;
        ParticleSystem _PS = GetComponent<ParticleSystem>();
        _PS.Play();
        for (int i = 0; i < 60; i++)
        {
            _GemObj.localPosition = (new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))).normalized * 0.05f;
            yield return null;
        }
        _PS.Stop();
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        _loopTime -= TargetPos.Length * _waitTime;

        Vector3 sPos = Vector3.zero;
        Vector3 ePos = Vector3.zero;

        sPos = this.gameObject.transform.position;
        ePos = TargetPos[_next];

        while (true)
        {
            _eTime = 0.0f;

            float delayRate = _loopTime / TargetPos.Length;   //핵심코드

            while (_eTime < 1)
            {
                _eTime += Time.fixedDeltaTime * Time.timeScale / delayRate;

                this.gameObject.transform.position = Vector3.Lerp(sPos, ePos, _eTime);

                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(_waitTime);
            _next++;
            if (_next >= TargetPos.Length)
            {
                _next = 0;
            }

            sPos = this.gameObject.transform.position;
            ePos = TargetPos[_next];
        }
    }
}