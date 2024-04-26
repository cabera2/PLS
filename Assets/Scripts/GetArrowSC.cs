using System.Collections;
using System.Collections.Generic;
using Lumia;
using UnityEngine;

using UnityEngine.UI;

public class GetArrowSC : MonoBehaviour
{
    public Vector2 _ArrowOffset;
    public string[] _ArrowText;
    [HideInInspector] public bool _Enabled;
    private GameObject _Arrow;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartC());
    }
    IEnumerator StartC()
    {
        while (StageManagerSC._WorkingCam == null)
        {
            yield return null;
        }
        if (StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._InteractArrowInst != null)
        {
            _Arrow = StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._InteractArrowInst;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StageManagerSC._LumiaInst != null)
        {
            if (GetComponent<BoxCollider2D>().IsTouching(StageManagerSC._LumiaInst.GetComponent<BoxCollider2D>()) && StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._IsGrounded == true && StageManagerSC._LumiaInst.GetComponent<LumiaSC>()._CanControl == true)
            {
                _Enabled = true;
                if (_Arrow == null)
                {
                    _Arrow = Instantiate(StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._InteractArrowPrefab);
                    _Arrow.transform.SetParent(StageManagerSC._WorkingCam.GetComponent<StageManagerSC>()._LocalCanvas.transform, false);
                }
                _Arrow.GetComponent<Text>().text = _ArrowText[SysSaveSC._Language];
                _Arrow.GetComponent<WorldTextSC>()._TargetObj = gameObject;
                _Arrow.GetComponent<WorldTextSC>()._Offset = _ArrowOffset;
                _Arrow.SetActive(true);
            }
            else if (_Arrow != null && _Enabled == true)
            {
                _Enabled = false;
                _Arrow.GetComponent<UIFaderSC>()._FadeOut();
            }
        }
    }
}
