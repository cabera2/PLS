using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventTriggerSC : MonoBehaviour
{
    public bool _BySword;
    public GameObject _Target;
    private bool _Used;
    public bool _UseOnce;
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
        if (_Used == false)
        {
            if (_BySword == false && col.gameObject.tag == "Lumia")
            {
                _Used = true;
                _Target.SetActive(true);

            }
            else if (_BySword == true && col.gameObject.tag == "Sword")
            {
                _Used = true;
                _Target.SetActive(true);
            }
            if (GetComponent<MapSaverSC>() != null && _UseOnce == true)
            {
                GetComponent<MapSaverSC>()._SaveStatus();
            }
        }

    }
}
