using UnityEngine;

namespace Enemy
{
    public class ShootingEnemy1SC : EnemyParentSC
    {
        public Vector2 _Speed;
        public float _DetectRange;
        public LayerMask _GroundLayer;
        private Vector3 DetectPosX;
        private Vector3 DetectPosY;
        // Start is called before the first frame update
        void Start()
        {
            base._Caching();
        }

        // Update is called once per frame
        void Update()
        {
            if (_Common._HP > 0)
            {
                int _DirX = _Speed.x > 0 ? 1 : _Speed.x < 0 ? -1 : 0;
                int _DirY = _Speed.y > 0 ? 1 : _Speed.y < 0 ? -1 : 0;
                DetectPosX = transform.position + new Vector3(_DirX * 0.5f, 0f, 0f);
                DetectPosY = transform.position + new Vector3(0f, _DirY * 0.5f + 0.5f, 0f);
                bool _Left;
                _Left = _Speed.x >= 0 ? true : _Speed.x < 0 ? false : false;
                _SR.flipX = _Left;

                bool _HitX = Physics2D.OverlapCircle(DetectPosX, _DetectRange, _GroundLayer);
                bool _HitY = Physics2D.OverlapCircle(DetectPosY, _DetectRange, _GroundLayer);
                if (_HitX == true)
                {
                    _Speed.x *= -1;
                }
                if (_HitY == true)
                {
                    _Speed.y *= -1;
                }
                _RB.velocity = _Speed;
            }
            else
            {
                enabled = false;
            }
        }
    }
}
