using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSkillGuideSC : MonoBehaviour
{
    public CanvasGroup[] _Elements;
    public Sprite[] _Images;
    public string[] _SkillNameKO;
    public string[] _SkillNameJP;
    public string[] _SkillKeyKO;
    public string[] _SkillKeyJP;
    public string[] _SkillInfoKO;
    public string[] _SkillInfoJP;
    public bool _Running;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void _Run(int _SwordCount)
    {
        StartCoroutine(_RunC(_SwordCount));
    }
    IEnumerator _RunC(int _SwordCount)
    {

        _Running = true;
        float _FadeSpeed = 1 * Time.deltaTime;
        if (_SwordCount <= 5)
        {
            _SwordCount -= 1;
            _Elements[2].GetComponent<Image>().sprite = _Images[_SwordCount];
            if (SysSaveSC._Language == 1)
            {
                _Elements[1].GetComponent<Text>().text = _SkillNameJP[_SwordCount];
                _Elements[3].GetComponent<Text>().text = _SkillKeyJP[_SwordCount];
                _Elements[4].GetComponent<Text>().text = _SkillInfoJP[_SwordCount];
            }
            else
            {
                _Elements[1].GetComponent<Text>().text = _SkillNameKO[_SwordCount];
                _Elements[3].GetComponent<Text>().text = _SkillKeyKO[_SwordCount];
                _Elements[4].GetComponent<Text>().text = _SkillInfoKO[_SwordCount];
            }
            for (int i = 0; i < 5; i++)
            {
                while (_Elements[i].alpha < 1)
                {
                    _Elements[i].alpha += _FadeSpeed;
                    yield return null;
                }
            }
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < 6; i++)
            {
                while (_Elements[i].alpha > 0)
                {
                    _Elements[i].alpha -= _FadeSpeed;
                    yield return null;
                }
            }
        }
        _Running = false;
        StageManagerSC._LSC._CanControl = true;
    }
}
