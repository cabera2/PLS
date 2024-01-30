using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LumiaSC
{
    private void ReloadingEffect(bool isStart)
    {
        _ANI.SetBool("_IsReloadingAni", isStart);
        _reloadEffectPlaying = isStart;
        if (isStart)
        {
            _ReloadParticle.Play();
        }
        else
        {
            _ReloadParticle.Stop();
            _SwordHanger.GetComponent<AudioSource>().Stop();
        }
    }
}
