using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class Test : MonoBehaviour
{
    public int Red = 120;
    public int Green = 104;
    public int Blue = 33;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = new Color32((byte)Red, (byte)Green, (byte)Blue, 255);
    }
}
