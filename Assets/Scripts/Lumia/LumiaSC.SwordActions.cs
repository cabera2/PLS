using System.Linq;
using UnityEngine;

namespace Lumia
{
    public partial class LumiaSC
    {
        private void Slash()
        {
            _AtkTimer = levelData.atkSpeedValues[levelData.atkSpeedLv];
            _mainAudioSource.PlayOneShot(sfx[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            GameObject _SlashInst = null;
            if (_stageManagerSc._SlashPool.Count == 0)
            {
                _SlashInst = Instantiate(_SlashObj);
                _SlashInst.GetComponent<ParticlePoolingSC>()._TargetPool = _stageManagerSc._SlashPool;
                _SlashInst.transform.parent = transform;
            }
            else if (_stageManagerSc._SlashPool.Count >= 1)
            {
                _SlashInst = _stageManagerSc._SlashPool[0];
                _SlashInst.SetActive(true);
                _stageManagerSc._SlashPool.RemoveAt(0);
            }

            _SlashInst.transform.localScale = new Vector2(levelData.swordSizeValues[levelData.swordSizeLv],
                levelData.swordSizeValues[levelData.swordSizeLv]);

            _mainAnimator.SetTrigger(AniDoSlash);
            if (_mainSpriteRenderer.flipX) _SlashInst.GetComponent<SpriteRenderer>().flipX = true;
            if (_UpDownInput > 0.5f)
            {
                _mainAnimator.SetFloat(AniAtkDirection, 1);
                _SlashInst.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                _SlashInst.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (_UpDownInput < -0.5f && _IsGrounded == false)
            {
                _mainAnimator.SetFloat(AniAtkDirection, -1);
                _SlashInst.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                _SlashInst.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _mainAnimator.SetFloat(AniAtkDirection, 0);
                _SlashInst.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                if (_mainSpriteRenderer.flipX == false)
                    _SlashInst.transform.rotation = Quaternion.Euler(0, 0, 90);
                else if (_mainSpriteRenderer.flipX) _SlashInst.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }

        private void Shot()
        {
            _AtkTimer = levelData.atkSpeedValues[levelData.atkSpeedLv];
            if (_SwordStock > 0 && _ReloadTimer <= _ShootTime)
            {
                float _ShootAngle = 0;
                var _ShootRC = new RaycastHit2D();
                if (_UpDownInput > 0.5f)
                {
                    _mainAnimator.SetFloat(AniAtkDirection, 1);
                    _ShootAngle = 180;
                    _ShootRC = Physics2D.Raycast(
                        new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z), Vector2.up,
                        Mathf.Infinity, _GroundLayer);
                }
                else if (_UpDownInput < -0.5f && _IsGrounded == false)
                {
                    _mainAnimator.SetFloat(AniAtkDirection, -1);
                    _ShootAngle = 0;
                    _ShootRC = Physics2D.Raycast(
                        new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z), Vector2.down,
                        Mathf.Infinity, _GroundLayer);
                    _RB.velocity = Vector2.up * _ShotRebound;
                }
                else
                {
                    _mainAnimator.SetFloat(AniAtkDirection, 0);
                    if (_mainSpriteRenderer.flipX == false)
                    {
                        _ShootAngle = 90;
                        _ShootRC = Physics2D.Raycast(
                            new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z),
                            Vector2.right, Mathf.Infinity, _GroundLayer);
                    }
                    else
                    {
                        _ShootAngle = -90;
                        _ShootRC = Physics2D.Raycast(
                            new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z),
                            Vector2.left, Mathf.Infinity, _GroundLayer);
                    }
                }


                if (_IsGrounded == false && _ShootAngle != 0) _RB.velocity = Vector2.zero;
                var _FromWall = _ShootRC.distance;
                if (_ShootRC.transform == null ||
                    (_ShootRC.transform == true &&
                     _ShootRC.transform.gameObject.GetComponent<PlatformEffector2D>() != null) || _FromWall >= 0.9f)
                {
                    _mainAudioSource.PlayOneShot(sfx[0], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                    _mainAnimator.SetTrigger(AniDoShoot);
                    GameObject _SwordInst = null;
                    if (_stageManagerSc._SwordPool.Count == 0)
                    {
                        _SwordInst = Instantiate(_SwordObj);
                    }
                    else if (_stageManagerSc._SwordPool.Count >= 1)
                    {
                        _SwordInst = _stageManagerSc._SwordPool[0];
                        _stageManagerSc._SwordPool.RemoveAt(0);
                    }

                    _SwordInst.transform.position = new Vector3(transform.position.x, transform.position.y + 0.7f,
                        transform.position.z);
                    _SwordInst.transform.rotation = Quaternion.Euler(0, 0, _ShootAngle);
                    _SwordInst.SetActive(true);
                    _SwordInst.GetComponent<Rigidbody2D>().velocity = -_SwordInst.transform.up * _SwordSpeed;
                    _SwordInst.GetComponent<SwordSC>()._Lumia = gameObject;
                    _SwordStock -= 1;
                    _Canvas.GetComponent<PauseSC>()._UpdateSwordCurrent();
                }
            }

            if (_reloadEffectPlaying)
            {
                ReloadingEffect(false);
                _reloadEffectPlaying = false;
            }

            _IsReloading = false;
            _ReloadTimer = 0;
        }

        private void FindNearestSword()
        {
            //가까운 검 찾기
            if (_SwordList.Count > 0)
            {
                for (int i = 0; i < _SwordList.Count; i++)
                {
                    _SwordDistanceList[i] = Vector2.Distance(_SwordList[i].transform.position, transform.position + new Vector3(0f, 0.7f, 0f));
                }
                _NearestSword = _SwordDistanceList.IndexOf(_SwordDistanceList.Min());
                if (_SwordDistanceList[_NearestSword] <= _WarpDistance)
                {

                    //_TargetMark.transform.position = Vector2.MoveTowards(_TargetMark.transform.position, _SwordList[_NearestSword].transform.position, _TargetMarkSpeed * Time.deltaTime);
                    if (_NearestSword != _NearestSword_Old || _InRange == false)
                    {
                        _TargetOnOff(true);
                        _TarAni.SetTrigger("_Trigger");
                        _InRange = true;
                        _mainAudioSource.PlayOneShot(sfx[4], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                        _NearestSword_Old = _NearestSword;
                    }
                    _TargetMark.transform.position = _SwordList[_NearestSword].transform.position;
                }
                else if (_SwordDistanceList[_NearestSword] > _WarpDistance)
                {
                    TargetPosReset();
                }
            }
            else if (_SwordList.Count == 0)
            {
                TargetPosReset();
            }
        }

        private void Teleport()
        {
            if (transform.parent != null) transform.parent = null;
            _mainAudioSource.PlayOneShot(sfx[1], SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            Vector2 _ParticlePos;
            var _ParticleOffset = new Vector3(0, -0.7f, 0f);
            for (var i = 0; i <= _SwordDistanceList[_NearestSword] / _ParticleGap; i++)
            {
                _ParticlePos =
                    (_SwordList[_NearestSword].transform.position - transform.position + _ParticleOffset).normalized * (_ParticleGap * i) + transform.position;
                GameObject _WarpParticle;
                if (_stageManagerSc._WarpParticlePool.Count == 0)
                {
                    _WarpParticle = Instantiate(_Particle[0], _ParticlePos, Quaternion.identity);
                    _WarpParticle.GetComponent<ParticlePoolingSC>()._TargetPool = _stageManagerSc._WarpParticlePool;
                    _WarpParticle.SetActive(true);
                }
                else if (_stageManagerSc._WarpParticlePool.Count >= 1)
                {
                    _WarpParticle = _stageManagerSc._WarpParticlePool[0];
                    _WarpParticle.transform.position = _ParticlePos;
                    _WarpParticle.SetActive(true);
                    _stageManagerSc._WarpParticlePool.RemoveAt(0);
                }
            }

            _IsWarp = true;
            ToSword();
        }
    }
}