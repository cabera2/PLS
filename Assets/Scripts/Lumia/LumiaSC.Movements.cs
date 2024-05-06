using UnityEngine;

namespace Lumia
{
    public partial class LumiaSC
    {
        private void Move()
        {
            Vector2Int leftStickInput = _myInput.GetAxis(KeyType.LeftStick);
            if (_KnockbackCounter > 0)
            {
                _KnockbackCounter -= Time.deltaTime;
            }
            else
            {
                if (_ReloadTimer < _StartReloadTime && _WarpTimer == 0 && _mainAnimator.GetBool(AniIsShielding) == false)
                {
                    leftStickX = leftStickInput.x;
                }
                else
                {
                    leftStickX = 0;
                }
                leftStickY = leftStickInput.y;
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
                        _RB.velocity = new Vector2(leftStickX * _CurrentWalkSpeed, _RB.velocity.y);
                    }
                    //Jump
                    if (_myInput.GetButton(KeyType.Jump) && _IsJumping)
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
                    if (_myInput.GetButtonDown(KeyType.Jump) && _JumpCountCounter > 0)
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
