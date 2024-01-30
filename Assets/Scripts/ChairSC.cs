using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChairSC : MonoBehaviour
{
    [HideInInspector] public bool _CanStand;
    private LumiaSC _LSC;
    public KeyCode[] _Keys;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartC());
    }
    IEnumerator StartC()
    {
        while (StageManagerSC._LumiaInst == null)
        {
            yield return null;
        }
        _LSC = StageManagerSC._LumiaInst.GetComponent<LumiaSC>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_LSC != null)
        {
            //Debug.Log(_LSC._MoveInput + "" + _CanStand);
            if (Mathf.Abs(_LSC._UpDownInput) > 0.5f && GetComponent<GetArrowSC>()._Enabled == true)
            {
                _LSC._CanControl = false;
                _CanStand = false;
                StartCoroutine(_MoveToPos());
            }

            else if (Mathf.Abs(_LSC._MoveInput) > 0.5f && _CanStand == true && GetComponent<BoxCollider2D>().IsTouching(StageManagerSC._LumiaInst.GetComponent<BoxCollider2D>()))
            {
                StageManagerSC._LumiaInst.GetComponent<Animator>().SetBool("_Sitting", false);
                _LSC._CanControl = true;
            }
        }
    }
    IEnumerator _MoveToPos()
    {
        float _Dir1 = transform.position.x - StageManagerSC._LumiaInst.transform.position.x;
        int _Dir2 = _Dir1 > 0 ? 1 : _Dir1 < 0 ? -1 : 0;
        _LSC._AutoWalk = _Dir2;
        while (0 < (transform.position.x - StageManagerSC._LumiaInst.transform.position.x) * _Dir2)
        {
            yield return null;
        }
        _LSC._AutoWalk = 0;
        LumiaHitboxSC _LHBSC = _LSC._Hitbox.GetComponent<LumiaHitboxSC>();
        _LHBSC._HP_Current = _LHBSC._HP_Max;
        _LSC._Canvas.GetComponent<PauseSC>()._UpdateCurrentHp();
        _LSC._SavedScene = SceneManager.GetActiveScene().name;
        SysSaveSC._CharSave();
        StartCoroutine(StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._WhiteFlash());
        StageManagerSC._LumiaInst.GetComponent<SpriteRenderer>().flipX = false;
        StageManagerSC._LumiaInst.GetComponent<Animator>().SetBool("_Sitting", true);
        StageManagerSC._LSC._TemporaryFlag.Clear();

        yield return new WaitForSeconds(1f);
        _CanStand = true;
    }
}
