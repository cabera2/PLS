using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lumia
{
    public partial class LumiaSC
    {
        public IEnumerator _LoadScene(string _TargetScene, bool _KeepLumia)
        {
            while (_Canvas.GetComponent<PauseSC>()._FadeObj.GetComponent<CanvasGroup>().alpha < 1f)
            {
                yield return null;
            }
            AsyncOperation op = SceneManager.LoadSceneAsync(_TargetScene);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            DontDestroyOnLoad(gameObject);
            op.allowSceneActivation = true;
            _CurrentScene = _TargetScene;
            if (_KeepLumia == true)
            {
                StartCoroutine(_Reload(false));
            }
            else
            {
                Destroy(_Canvas);
                Destroy(_TargetMark);
                Destroy(gameObject);
            }

        }
        public void _WhenSceneLoad()
        {
            _stageManagerSc = _MyCamera.GetComponent<StageManagerSC>();
            if (SceneManager.GetActiveScene().name == _WarpScene)
            {
                _SetPortal();
            }
            if (_ChairRespawn == true)
            {
                _ChairRespawn = false;
                _stageManagerSc._ChairStart();
                _RB.bodyType = RigidbodyType2D.Dynamic;
            }
            if (_PassingPortal == true)
            {
                _PassingPortal = false;
                transform.position = _WarpPos;
                _CanControl = true;
            }
            if (_PassingGate == true)
            {
                _PassingGate = false;
                transform.position = _stageManagerSc._Gates[_GateNumber].position;

                if (_JumpGate == false)
                {
                    if (_AutoWalk != 0)
                    {
                        StartCoroutine(_StopAutoMove());
                    }
                    else
                    {
                        StartCoroutine(_FallGateEvent());
                    }
                }

                else if (_JumpGate == true)
                {
                    transform.position = transform.position + Vector3.up * 2f;
                    if (_JumpGateDir == 0)
                    {
                        _AutoWalk = _mainSpriteRenderer.flipX == false ? 0.5f : _mainSpriteRenderer.flipX == true ? -0.5f : 0;
                    }
                    else if (_JumpGateDir != 0)
                    {
                        _AutoWalk = _JumpGateDir * 0.5f;
                        _mainSpriteRenderer.flipX = _JumpGateDir > 0 ? false : _JumpGateDir < 0 ? true : false;
                    }
                    StartCoroutine(_JumpGateEvent());
                }
            }
            _MusicCheck();
            _Canvas.GetComponent<Canvas>().worldCamera = _MyCamera.GetComponent<Camera>();
            _Canvas.GetComponent<Canvas>().sortingLayerName = "UI";
            _Canvas.GetComponent<PauseSC>()._FadeObj.GetComponent<UIFaderSC>()._FadeOut();
            _TargetMark.transform.position = transform.position;
        }
    }
}
