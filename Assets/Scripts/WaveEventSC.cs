using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEventSC : MonoBehaviour
{
    [System.Serializable]
    public struct _EnemyPos
    {
        public Vector2 _Pos;
        public GameObject _Prefab;
    }
    public _EnemyPos[] _WaveMember;
    public bool _FirstWave;
    public WaveEventSC _NextWave;
    public int _MaxEnemy;

    private int _NextEnemy;
    public int _KillCount;

    // Start is called before the first frame update
    void Start()
    {
        if (_FirstWave == true)
        {
            _Run();
            if (GetComponent<ShutterControllerSC>() != null)
            {
                GetComponent<ShutterControllerSC>()._Run(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void _Run()
    {
        for (int i = 0; i < _MaxEnemy; i++)
        {
            Summon(false);
        }
    }
    public void Summon(bool _Kill)
    {
        if (_Kill == true)
        {
            _KillCount += 1;
            if (_KillCount >= _WaveMember.Length)
            {
                Debug.Log("웨이브 클리어!");
                if (_NextWave == null && GetComponent<ShutterControllerSC>() != null)
                {
                    GetComponent<ShutterControllerSC>()._Run(true);
                    if (GetComponent<MapSaverSC>() != null)
                    {
                        GetComponent<MapSaverSC>()._SaveStatus();
                    }
                }
            }
        }
        if (_NextEnemy < _WaveMember.Length)
        {
            Debug.Log(_NextEnemy + "번 몬스터 소환");
            GameObject _NewEnemy = Instantiate(_WaveMember[_NextEnemy]._Prefab);
            _NewEnemy.GetComponent<EnemyParentSC>()._Appear();
            _NewEnemy.GetComponent<EnemyCommonSC>()._WaveController = this;
            _NewEnemy.transform.position = _WaveMember[_NextEnemy]._Pos;
            _NextEnemy += 1;
        }

    }
    void _End()
    {
        if (_NextWave != null)
        {
            _NextWave._Run();
        }
    }
}
