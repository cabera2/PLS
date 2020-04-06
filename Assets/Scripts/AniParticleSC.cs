using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniParticleSC : MonoBehaviour
{
    public ParticleSystem[] _PS;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void _Play(int _Num){
        _PS[_Num].Play();
    }
}
