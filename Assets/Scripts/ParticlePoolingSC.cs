using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolingSC : MonoBehaviour
{
    public List<GameObject> _TargetPool;
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnParticleSystemStopped()
    {
        _TargetPool.Add(gameObject);
        gameObject.SetActive(false);
    }
}
