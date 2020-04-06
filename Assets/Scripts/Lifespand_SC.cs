using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespand_SC : MonoBehaviour
{
    public float _Lifespand;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _Lifespand -= Time.deltaTime;
        if (_Lifespand > 0)
        {
            _Lifespand -= Time.deltaTime;
        }
        else if (_Lifespand <= 0)
        {
            Destroy(gameObject);
        }
    }
}
