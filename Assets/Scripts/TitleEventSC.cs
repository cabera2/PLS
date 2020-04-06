using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class TitleEventSC : MonoBehaviour
{
    public int _TestLang;
    public GameObject _Menu;
    public UIFaderSC _BlackStart;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(_ShowMenu());
    }

    // Update is called once per frame
    void Update()
    {
        _TestLang = SysSaveSC._Language;
        if (GetComponent<Canvas>().worldCamera == null)
        {
            GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }
    IEnumerator _ShowMenu()
    {
        Debug.Log("타이틀 코루틴 시작");
        SysSaveSC._Loading = true;
        SysSaveSC._SysLoad();
        while (SysSaveSC._Loading == true)
        {
            yield return null;
        }
        _Menu.SetActive(true);
        Debug.Log("시스템 세이브 불러오기 종료");
        AsyncOperation op = null;
        if (SysSaveSC._LastFile == 0)
        {
            op = SceneManager.LoadSceneAsync("OpeningStage");
        }
        else if (SysSaveSC._LastFile != 0)
        {
            op = SceneManager.LoadSceneAsync("123");
        }
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            yield return null;
        }
        op.allowSceneActivation = true;
        yield return new WaitForSeconds(1.0f);
        _BlackStart._FadeOut();
    }
    public void _QuitGame()
    {
        Application.Quit();
    }
}
