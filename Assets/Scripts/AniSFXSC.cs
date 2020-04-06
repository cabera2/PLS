using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniSFXSC : MonoBehaviour
{
    public AudioClip _SFX;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _PlaySFX()
    {
        AudioSource _AS = StageManagerSC._LumiaInst.GetComponent<AudioSource>();
        _AS.PlayOneShot(_SFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
    }
}
