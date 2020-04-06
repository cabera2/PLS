using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFXSC : MonoBehaviour
{
    private StageManagerSC _SMSC;
    public GameObject[] _HitFx;
    public bool _IsSlash;
    // Start is called before the first frame update
    void Start()
    {
        _SMSC = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _GenFX()
    {
        GameObject _FXInst = null;
        if (_IsSlash == true)
        {
            List<GameObject> _Fx1Pool = _SMSC._HitFX1Pool;
            if (_Fx1Pool.Count == 0)
            {
                _FXInst = Instantiate(_HitFx[0]);
                _FXInst.transform.GetChild(0).GetComponent<ParentToPoolSC>()._Pool = _Fx1Pool;
            }
            else if (_Fx1Pool.Count >= 1)
            {
                _FXInst = _Fx1Pool[0];
                _Fx1Pool.RemoveAt(0);
            }
            _FXInst.transform.position = transform.position + (transform.up * -2f);
            _FXInst.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            _FXInst.SetActive(true);
        }
        GameObject _FXInst2 = null;
        List<GameObject> _Fx2Pool = _SMSC._HitFX2Pool;
        if (_Fx2Pool.Count == 0)
        {
            _FXInst2 = Instantiate(_HitFx[1]);
            _FXInst2.transform.GetChild(0).GetComponent<ParentToPoolSC>()._Pool = _Fx2Pool;
        }
        else if (_Fx2Pool.Count >= 1)
        {
            _FXInst2 = _Fx2Pool[0];
            _Fx2Pool.RemoveAt(0);
        }
        _FXInst2.transform.position = transform.position + (transform.up * -2f);
        _FXInst2.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        _FXInst2.SetActive(true);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyWeapon")
        {
            _GenFX();
        }
    }
}
