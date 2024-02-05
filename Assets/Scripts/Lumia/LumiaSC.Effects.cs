using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LumiaSC
{
    private void ReloadingEffect(bool isStart)
    {
        _mainAnimator.SetBool(AniIsReloading, isStart);
        _reloadEffectPlaying = isStart;
        if (isStart)
        {
            _ReloadParticle.Play();
            _reloadAudioSource.volume = SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f;
            _reloadAudioSource.Play();
        }
        else
        {
            _ReloadParticle.Stop();
            _reloadAudioSource.Stop();
        }
    }
}
