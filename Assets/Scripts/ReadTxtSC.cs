using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadTxtSC : MonoBehaviour
{
    public string[] _TestTexts;
    // Start is called before the first frame update
    void Start()
    {
        _TestTexts = System.IO.File.ReadAllLines(Application.dataPath + @"/Texts/TestText.txt");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
