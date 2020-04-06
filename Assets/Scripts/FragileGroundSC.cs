using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileGroundSC : MonoBehaviour
{
    private bool _Stepped;
    public AudioClip[] _SFX;
    private AudioSource _AS;
    public Sprite[] _FragSpr;
    public GameObject _FrageObj;
    public Transform _SurfaceHolder;
    public int _FragCount;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MapSaverSC>() != null && GetComponent<MapSaverSC>()._Used == true)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Lumia" && _Stepped == false)
        {
            _AS = col.gameObject.GetComponent<AudioSource>();
            _Stepped = true;
            for (int i = 0; i < _SurfaceHolder.childCount; i++)
            {
                if (i % 2 == 0)
                {
                    _SurfaceHolder.GetChild(i).eulerAngles = new Vector3(0, 0, 5);
                }
                else
                {
                    _SurfaceHolder.GetChild(i).eulerAngles = new Vector3(0, 0, -5);
                }
            }
            Invoke("_Break", 0.5f);
            GetComponent<ParticleSystem>().Play();
            _AS.PlayOneShot(_SFX[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        }
    }
    void _Break()
    {
        if (GetComponent<MapSaverSC>() != null)
        {
            GetComponent<MapSaverSC>()._SaveStatus();
        }
        _AS.PlayOneShot(_SFX[1], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
        BoxCollider2D _BC2D = transform.parent.GetComponent<BoxCollider2D>();
        float _Width = _BC2D.size.x / 2;
        _BC2D.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < _FragCount; i++)
        {
            GameObject FragInst;
            FragInst = Instantiate(_FrageObj);
            FragInst.GetComponent<SpriteRenderer>().sprite = _FragSpr[Random.Range(0, _FragSpr.Length)];
            FragInst.AddComponent(typeof(PolygonCollider2D));
            FragInst.transform.position = transform.position + new Vector3(Random.Range(-_Width, _Width), Random.Range(-0.5f, 0.5f), 0);
        }
    }
}
