using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LumiaSC
{
    //Animation Parameters
    public static readonly int AniXInputAbs = Animator.StringToHash("XInputAbs");
    public static readonly int AniYVelocity = Animator.StringToHash("YVelocity");
    public static readonly int AniIsGrounded = Animator.StringToHash("IsGrounded");
    public static readonly int AniAtkDirection = Animator.StringToHash("AtkDirection");
    public static readonly int AniDoHang = Animator.StringToHash("DoHang");
    public static readonly int AniIsHanging = Animator.StringToHash("IsHanging");
    public static readonly int AniIsSitting = Animator.StringToHash("IsSitting");
    public static readonly int AniIsSpiked = Animator.StringToHash("IsSpiked");
    public static readonly int AniIsReloading = Animator.StringToHash("IsReloading");
    public static readonly int AniIsGliding = Animator.StringToHash("IsGliding");
    public static readonly int AniIsShielding = Animator.StringToHash("IsShielding");
    public static readonly int AniIsWarpSetting = Animator.StringToHash("IsWarpSetting");
    public static readonly int AniIsWarpMoving = Animator.StringToHash("IsWarpMoving");
    public static readonly int AniDoSlash = Animator.StringToHash("DoSlash");
    public static readonly int AniDoShoot = Animator.StringToHash("DoShoot");
    public static readonly int AniDoSit = Animator.StringToHash("DoSit");
    public static readonly int AniDoDamage = Animator.StringToHash("DoDamage");
    
    
    private void ReloadingEffect(bool isStart)
    {
        _ANI.SetBool(AniIsReloading, isStart);
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
