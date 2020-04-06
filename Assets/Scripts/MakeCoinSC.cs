using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MakeCoinSC : MonoBehaviour
{
    public int _Money;
    public GameObject _CoinPrefab;
    // Start is called before the first frame update
    void Start()
    {
 
    }
 
    // Update is called once per frame
    void Update()
    {
 
    }
    public void _Make()
    {
        if (_Money > 0)
        {
            List<GameObject> _Pool = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._CoinPool;
            while (_Money > 0)
            {
                int _CoinAmount;
                if (_Money >= 10)
                {
                    _CoinAmount = 10;
                }
                else if (_Money >= 5)
                {
                    _CoinAmount = 5;
                }
                else
                {
                    _CoinAmount = 1;
                }
                GameObject _CoinInst = null;
                if (_Pool.Count == 0)
                {
                    _CoinInst = Instantiate(_CoinPrefab);
                }
                else
                {
                    _CoinInst = _Pool[0];
                    _Pool.RemoveAt(0);
                }
                _CoinInst.GetComponent<CoinSC>()._Amount = _CoinAmount;
                _CoinInst.transform.position = transform.position;
                _CoinInst.SetActive(true);
                _CoinInst.GetComponent<Rigidbody2D>().velocity = (new Vector2(UnityEngine.Random.Range(-1f, 1f), 1)).normalized * 10f;
                _Money -= _CoinAmount;
            }
        }
    }
}