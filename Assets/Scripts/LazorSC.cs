using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazorSC : MonoBehaviour
{
    public LayerMask _GroundLayer;
    public GameObject[] _Lazors;
    public AudioClip[] _SFX;
    private AudioSource _AS;
    private BoxCollider2D _BC;
    public bool _Loop;
    public float _Timer;
    public float[] _PhaseTime;
    private float _XScale;
    // Start is called before the first frame update
    void Start()
    {
        _BC = _Lazors[0].GetComponent<BoxCollider2D>();
        _AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_Timer < _PhaseTime[2])
        {

            RaycastHit2D _LazorRay = Physics2D.Raycast(transform.position, transform.rotation * Vector3.down, Mathf.Infinity, _GroundLayer);
            float _RayLength = _LazorRay.distance;
            _Lazors[0].transform.localScale = new Vector2(_XScale, _RayLength / 3);
            if (_Loop == true)
            {

                _Timer += Time.deltaTime;
            }

            if (_Timer < _PhaseTime[0])
            {
                if (_AS.isPlaying == true)
                {
                    _AS.Stop();
                }
                _BC.enabled = false;
                _Lazors[1].SetActive(false);
                _Lazors[2].SetActive(false);
                _XScale = Mathf.MoveTowards(_XScale, 0, 5 * Time.deltaTime);
            }
            else
            {
                if (_AS.isPlaying == false)
                {
                    _AS.Play();
                }

                if (_Timer < _PhaseTime[1])
                {
                    _AS.clip = _SFX[0];
                    _Lazors[2].SetActive(true);
                    _Lazors[2].transform.localScale = new Vector2(0.35f, 0.35f);
                    _XScale = Mathf.MoveTowards(_XScale, 0.1f, 5 * Time.deltaTime);
                    
                }
                else if (_Timer < _PhaseTime[2])
                {
                    _AS.clip = _SFX[1];
                    _BC.enabled = true;
                    _Lazors[1].SetActive(true);
                    _Lazors[2].transform.localScale = new Vector2(0.7f, 0.7f);
                    _XScale = Mathf.MoveTowards(_XScale, 0.5f, 5 * Time.deltaTime);
                    _Lazors[1].transform.localPosition = new Vector3(0, -_RayLength, -0.1f);
                }
            }
        }
        else
        {
            _Timer = 0;
        }

    }
}
