using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterControllerSC : MonoBehaviour
{
    public ShutterSC[] _Shutter;
    public GameObject _CameraArea;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _Run(bool _Open)
    {
        if (_Open == true)
        {
            for (int i = 0; i < _Shutter.Length; i++)
            {
                _Shutter[i]._Open();

            }
            if (_CameraArea != null)
            {
                _CameraArea.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _Shutter.Length; i++)
            {
                _Shutter[i]._Close();

            }
            if (_CameraArea != null)
            {
                _CameraArea.SetActive(true);
            }
        }
    }
}
