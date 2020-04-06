using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSC : MonoBehaviour
{
    public int _Amount;
    public GameObject _CoinFxPF;

    private List<GameObject> _Pool;
    private List<GameObject> _FXPool;
    private AudioSource _AS;
    public AudioClip[] _SFX;
    void Awake()
    {
        if (StageManagerSC._WorkingCam != null)
        {
            _Pool = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._CoinPool;
            _FXPool = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._CoinFXPool;
            _AS = StageManagerSC._LumiaInst.GetComponent<AudioSource>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(_StartChase());
    }
    void OnEnable()
    {
        GetComponent<Animator>().SetInteger("_Amount", _Amount);
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator _StartChase()
    {
        while (StageManagerSC._LumiaInst == null)
        {
            yield return null;
        }
        transform.parent.GetComponent<SpringJoint2D>().connectedAnchor = StageManagerSC._LumiaInst.transform.position;
        transform.parent.GetComponent<SpringJoint2D>().enabled = true;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            _AS.PlayOneShot(_SFX[Random.Range(0, 1)], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        }
        if (col.gameObject.name == "LumiaHitbox")
        {
            _AS.PlayOneShot(_SFX[Random.Range(2, 3)], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            if (StageManagerSC._LSC._Money < 999999999)
            {
                if (StageManagerSC._LSC._Money + _Amount > 999999999)
                {
                    StageManagerSC._LSC._Money = 999999999;
                }
                else {
                    StageManagerSC._LSC._Money += _Amount;
                }
            }
            gameObject.SetActive(false);
            _Pool.Add(gameObject);

            GameObject _CoinFX = null;
            if (_FXPool.Count == 0)
            {
                _CoinFX = Instantiate(_CoinFxPF);
                _CoinFX.transform.GetChild(0).GetComponent<ParentToPoolSC>()._Pool = _FXPool;
            }
            else if (_FXPool.Count >= 1)
            {
                _CoinFX = _FXPool[0];
                _FXPool.RemoveAt(0);
            }
            _CoinFX.transform.position = transform.position;
            _CoinFX.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            _CoinFX.SetActive(true);
        }
    }
}
