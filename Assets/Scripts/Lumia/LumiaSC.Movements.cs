using UnityEngine;

namespace Lumia
{
    public partial class LumiaSC
    {
        private void Move()
        {
            if (_KnockbackCounter > 0)
            {
                _KnockbackCounter -= Time.deltaTime;
            }
            else
            {
                if (_ReloadTimer < _StartReloadTime && _WarpTimer == 0 && _mainAnimator.GetBool(AniIsShielding) == false)
                {
                    if (_SmoothMove == true)
                    {
                        _MoveInput = Input.GetAxisRaw("LeftStickX");
                    }
                    else if (_SmoothMove == false)
                    {
                        int _LeftPressed = Input.GetKey(SysSaveSC._Keys[2]) == true ? -1 : Input.GetKey(SysSaveSC._Keys[2]) == false ? 0 : 0;
                        int _RightPressed = Input.GetKey(SysSaveSC._Keys[3]) == true ? 1 : Input.GetKey(SysSaveSC._Keys[3]) == false ? 0 : 0;
                        _MoveInput = Input.GetAxisRaw("LeftStickX") + _LeftPressed + _RightPressed;
                        if (_MoveInput != 0)
                        {
                            if (_MoveInput < 0)
                            {
                                _MoveInput = -1f;
                            }

                            if (_MoveInput > 0)
                            {
                                _MoveInput = 1f;
                            }
                        }
                    }
                }
                else
                {
                    _MoveInput = 0;
                }

                int _UpPressed = Input.GetKey(SysSaveSC._Keys[0]) == true ? 1 : Input.GetKey(SysSaveSC._Keys[0]) == false ? 0 : 0;
                int _DownPressed = Input.GetKey(SysSaveSC._Keys[1]) == true ? -1 : Input.GetKey(SysSaveSC._Keys[1]) == false ? 0 : 0;
                _UpDownInput = Input.GetAxisRaw("LeftStickY") + _UpPressed + _DownPressed;
                if (_UpDownInput != 0)
                {
                    if (_UpDownInput < -0.5f)
                    {
                        _UpDownInput = -1f;
                    }

                    if (_UpDownInput > 0.5f)
                    {
                        _UpDownInput = 1f;
                    }
                }

                if (_CanControl == true)
                {
                    //Walk
                    if (_CanControl == false)
                    {
                        _RB.velocity = new Vector2(_AutoWalk * _CurrentWalkSpeed, _RB.velocity.y);
                        if (_AutoJumping == true)
                        {
                            _RB.velocity = new Vector2(_AutoWalk * 2 * _CurrentWalkSpeed, _JumpForce);
                        }
                    }
                    if (_RB.bodyType == RigidbodyType2D.Dynamic)
                    {
                        _RB.velocity = new Vector2(_MoveInput * _CurrentWalkSpeed, _RB.velocity.y);
                    }
                    //Jump
                    if ((Input.GetButton("ButtonA") || Input.GetKey(SysSaveSC._Keys[5])) && _IsJumping == true)
                    {
                        if (_JumpTimeCounter > 0 && _JumpCountCounter > 0)
                        {
                            _CoyoteTimer = _CoyoteTime;
                            _RB.velocity = new Vector2(_RB.velocity.x, _JumpForce);
                            _JumpTimeCounter -= Time.deltaTime;
                        }
                        else
                        {
                            _IsJumping = false;
                        }
                    }
                    //StopJump
                    if ((Input.GetButtonUp("ButtonA") || Input.GetKeyUp(SysSaveSC._Keys[5])) && _JumpCountCounter > 0)
                    {
                        _JumpCountCounter -= 1;
                    }
                }
                if (_CanControl == false)
                {
                    _RB.velocity = new Vector2(_AutoWalk * _CurrentWalkSpeed, _RB.velocity.y);
                    if (_AutoJumping == true)
                    {
                        _RB.velocity = new Vector2(_AutoWalk * 2 * _CurrentWalkSpeed, _JumpForce);
                    }
                }
                if (_RB.bodyType == RigidbodyType2D.Dynamic)
                {
                    _RB.velocity = Vector3.ClampMagnitude(_RB.velocity, _MaxSpeed);
                }
            }
        }
    }
}
