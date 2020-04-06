using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToPoolSC : MonoBehaviour
{
    private GameObject _Lumia;
    [HideInInspector] public List<GameObject> _Pool;
    // Start is called before the first frame update
    void OnParticleSystemStopped()
    {
        _Pool.Add(transform.parent.gameObject);
        transform.parent.gameObject.SetActive(false);
    }
}
