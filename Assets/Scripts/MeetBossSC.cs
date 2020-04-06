using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetBossSC : MonoBehaviour
{
    public GameObject _Boss;
    public ShutterSC[] _ShutterSC;
    private bool _Touched;
    private StageManagerSC _CurrentSM;
    private Vector2 _PrevMin;
    private Vector2 _PrevMax;
    public Vector2 _NewMin;
    public Vector2 _NewMax;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (_Touched == false && col.gameObject.layer == 9)
        {
            _Touched = true;
            for (int i = 0; i < _ShutterSC.Length; i++)
            {
                _ShutterSC[i]._Close();
            }

            StageManagerSC._LSC._CanControl = false;
            float _BosDir = _Boss.transform.position.x - StageManagerSC._LumiaInst.transform.position.x;
            StageManagerSC._LumiaInst.GetComponent<SpriteRenderer>().flipX = _BosDir > 0 ? false : _BosDir < 0 ? true : false;
            _CurrentSM = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>();
            _PrevMin = _CurrentSM._CamMinPos;
            _PrevMax = _CurrentSM._CamMaxPos;
            _CurrentSM._CamMinPos = _NewMin;
            _CurrentSM._CamMaxPos = _NewMax;
            _Boss.SetActive(true);
        }
    }
    public void _RevertCam()
    {
        if (_CurrentSM == null)
        {
            Debug.Log("카메라가 지정되지 않았습니다.");
        }
        else
        {
            _CurrentSM._CamMinPos = _PrevMin;
            _CurrentSM._CamMaxPos = _PrevMax;
        }
    }
}
