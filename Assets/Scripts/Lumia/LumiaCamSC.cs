using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumiaCamSC : MonoBehaviour
{
    public GameObject _MyCamera;
    private GameObject _CamArea;
    private Rigidbody2D _RB;
    public Vector2 _CameraOffset;
    [HideInInspector] public Vector3 _CamPos1;
    private bool _InCamArea;
    [HideInInspector] public float _LookUpDown = 0;
    private Vector3 _PrevPos;
    public float _Speed;


    private bool _Shaking;
    private float _ShakeAmount;
    public bool _CoolDownLock;
    public float _CoolDownSpeed;
    private Vector3 _ShakeOffset;
    // Start is called before the first frame update
    void Awake()
    {
        _RB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.timeScale != 0)
        {
            _CameraControl();
        }
    }

    public void _CameraControl()
    {
        if (_MyCamera != null)
        {
            int _Direction = GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            _CamPos1 = new Vector3(transform.position.x + _CameraOffset.x * _Direction, transform.position.y + _CameraOffset.y + _LookUpDown, -10);
            Vector2 _CamMinPos;
            Vector2 _CamMaxPos;
            if (_InCamArea == true)
            {
                _CamMinPos = _CamArea.GetComponent<CameraAreaSC>()._CamMinPos;
                _CamMaxPos = _CamArea.GetComponent<CameraAreaSC>()._CamMaxPos;
            }
            else
            {
                _CamMinPos = _MyCamera.GetComponent<StageManagerSC>()._CamMinPos;
                _CamMaxPos = _MyCamera.GetComponent<StageManagerSC>()._CamMaxPos;
            }

            _CamPos1.x = Mathf.Clamp(_CamPos1.x, _CamMinPos.x, _CamMaxPos.x);
            _CamPos1.y = Mathf.Clamp(_CamPos1.y, _CamMinPos.y, _CamMaxPos.y);
            _MyCamera.transform.position = Vector3.MoveTowards(_MyCamera.transform.position, _CamPos1, (Vector2.Distance(_CamPos1, _MyCamera.transform.position) * _Speed));
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "CameraArea")
        {
            _InCamArea = true;
            _CamArea = col.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "CameraArea")
        {
            _InCamArea = false;
        }
    }
    public void _Shake(float _ShakeRequest)
    {
        _ShakeAmount = _ShakeRequest;
        if (_Shaking == false)
        {
            StartCoroutine(_ShakeC());
        }
    }
    IEnumerator _ShakeC()
    {
        _Shaking = true;
        Vector3 _PrevPos = _MyCamera.transform.position;
        while (_ShakeAmount != 0)
        {
            if (_CoolDownLock == false)
            {
                _ShakeAmount = Mathf.MoveTowards(_ShakeAmount, 0, _CoolDownSpeed * Time.unscaledDeltaTime);
            }
            _ShakeOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * _ShakeAmount;
            _MyCamera.transform.position = _PrevPos + _ShakeOffset;
            yield return null;
        }
        _Shaking = false;
    }
    
}
