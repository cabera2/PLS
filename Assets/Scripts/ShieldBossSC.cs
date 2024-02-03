using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBossSC : MonoBehaviour
{
    public LayerMask _GroundLayer;
    public MeetBossSC _Meet;
    public ShieldEnmSC _SESC;
    public AudioClip[] _SFX;
    public EnemyCommonSC _ECSC;
    private bool _ClearEventPlayed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartC());
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        _SESC.enabled = false;
    }
    IEnumerator StartC()
    {
        float _StartPos = transform.position.y;
        while (transform.position.y > _StartPos - 5)
        {
            yield return null;
        }
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        while (Physics2D.OverlapCircle(transform.position, 0.1f, _GroundLayer) == false)
        {
            yield return null;
        }
        StageManagerSC._LumiaInst.GetComponent<AudioSource>().PlayOneShot(_SFX[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        StageManagerSC._CamSC._Shake(0.5f);
        yield return new WaitForSeconds(1f);
        _SESC.enabled = true;
        StageManagerSC._lumiaSc._CanControl = true;
        StartCoroutine(StageManagerSC._lumiaSc._ChangeMusic(_SFX[1]));
    }
    // Update is called once per frame
    void Update()
    {
        if (_ECSC._HP <= 0 && _ClearEventPlayed == false)
        {
            _ClearEventPlayed = true;
            if (GetComponent<MapSaverSC>() != null)
            {
                GetComponent<MapSaverSC>()._SaveStatus();
            }
            GetComponent<ShutterControllerSC>()._Run(true);
            StartCoroutine(StageManagerSC._lumiaSc._ChangeMusic(null));
            _Meet._RevertCam();
        }
    }
}
