using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiSC : MonoBehaviour
{
    private const float WaitTime = 1f;

    private KeyCode[] keys = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };

    public bool success;

    public GameObject _DeBug;
    // Start is called before the first frame update
    public void ResetSuccess()
    {
        success = false;
    }
    void OnEnable()
    {
        StartCoroutine(StartC());
    }
    IEnumerator StartC()
    {
        float timer = 0f;
        int index = 0;

        while (gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(keys[index]))
            {
                index++;

                if (index == keys.Length)
                {
                    timer = 0f;
                    index = 0;
                    StartCoroutine(_Success());
                }
                else
                {
                    timer = WaitTime;
                }
            }
            else if (Input.anyKeyDown)
            {
                // print("Wrong key in sequence.");
                timer = 0;
                index = 0;
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = 0;
                    index = 0;
                }
            }
            yield return null;
        }
    }
    IEnumerator _Success()
    {
        if (success == false)
        {
            GetComponent<UIFaderSC>()._FadeOut();
            _DeBug.SetActive(true);
            success = true;
        }
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
