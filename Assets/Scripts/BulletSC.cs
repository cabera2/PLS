using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSC : MonoBehaviour
{
    [HideInInspector] public List<GameObject> _Pool;
    public AudioClip _SFX;
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
        if (col.gameObject.name == "LumiaHitbox")
        {
            _Destroy();
        }
        if (col.gameObject.name == "Shield")
        {
            _Destroy();
            if(StageManagerSC._LumiaInst != null){
                StageManagerSC._LumiaInst.GetComponent<AudioSource>().PlayOneShot(_SFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            _Destroy();
        }
    }
    void _Destroy()
    {
        _Pool.Add(gameObject);
        gameObject.SetActive(false);
    }
}
