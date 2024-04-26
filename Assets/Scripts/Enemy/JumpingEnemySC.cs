using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class JumpingEnemySC : EnemyParentSC
    {
        private float _JumpTimer;
        private float _DefaultGravity;
        private float _HalfWidth;
        public bool _OnGround;
        public LayerMask _GroundLayer;
        private RaycastHit2D _DetectF;
        private RaycastHit2D _DetectD;
        public float _MoveSpeed;
        public bool _Glide;
        private bool _InRange;
        private GameObject _Lumia;
        // Start is called before the first frame update
        void Start()
        {
            base._Caching();
            _HalfWidth = GetComponent<CircleCollider2D>().radius;
            if (_Glide == true)
            {
                _Ani.SetBool("_GlideType", true);
                _DefaultGravity = _RB.gravityScale;
                StartCoroutine(_StartC());
            }
        }
        IEnumerator _StartC()
        {
            while (StageManagerSC._LumiaInst == null)
            {
                yield return null;
            }
            _Lumia = StageManagerSC._LumiaInst;
        }
        public override void _Appear()
        {
            StartCoroutine(_AppearC());
        }
        IEnumerator _AppearC()
        {
            base._Caching();
            CircleCollider2D _CC = GetComponent<CircleCollider2D>();
            _CC.enabled = false;
            _SR.color = new Color(1f, 1f, 1f, 0f);
            float _Alpha = 0;
            while (_SR.color.a < 1)
            {
                _Alpha += 0.05f;
                _SR.color = new Color(1f, 1f, 1f, _Alpha);
                yield return null;
            }
            _CC.enabled = true;
        }
        // Update is called once per frame
        void Update()
        {
            if (_Common._HP > 0)
            {
                _OnGround = Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 0.1f), 0, _GroundLayer);
                _Ani.SetBool("_OnGround", _OnGround);
                if (_Glide == true && _Lumia != null)
                {
                    if (Vector2.Distance(transform.position, _Lumia.transform.position) < 5 && Mathf.Abs(transform.position.y - _Lumia.transform.position.y) < 2 && ((_Lumia.transform.position.x - transform.position.x) * _MoveSpeed) > 0)
                    {
                        _InRange = true;
                    }
                    else
                    {
                        _InRange = false;
                    }
                }

                if (_OnGround == true)
                {
                    if (_JumpTimer > 0)
                    {
                        _JumpTimer -= Time.deltaTime;
                    }
                    else
                    {
                        _JumpTimer = 0.7f;
                        if (_Glide == false || _InRange == false)
                        {
                            _RB.AddForce(new Vector2(_MoveSpeed, 12), ForceMode2D.Impulse);
                        }
                        else
                        {
                            StartCoroutine(_Attack());
                        }
                    }
                }
            }
            else
            {
                enabled = false;
            }
        }
        IEnumerator _Attack()
        {
            _RB.AddForce(new Vector2(_MoveSpeed, 18), ForceMode2D.Impulse);
            while (_RB.velocity.y >= 0)
            {
                yield return null;
            }
            _Ani.SetBool("_Atk", true);
            _RB.gravityScale = 0.5f;
            while (_RB.gravityScale == 0.5f)
            {
                int _Dir = _MoveSpeed > 0 ? 1 : _MoveSpeed < 0 ? -1 : 0;
                _RB.velocity = new Vector2(3 * _Dir, _RB.velocity.y);
                yield return null;
            }
        }
        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer == 8)
            {
                if (_Glide == true && _RB.gravityScale == 0.5f && (col.gameObject.layer == 8 || col.gameObject.layer == 9 || col.gameObject.layer == 10 || col.gameObject.name == "Slash(Clone)"))
                {
                    _Ani.SetBool("_Atk", false);
                    _RB.gravityScale = _DefaultGravity;
                }
                float _Pos = _MoveSpeed > 0 ? _HalfWidth : _MoveSpeed < 0 ? -_HalfWidth : 0;
                if (Physics2D.OverlapCircle(transform.position + new Vector3(_Pos, _HalfWidth, 0), 0.1f, _GroundLayer) == true)
                {
                    _MoveSpeed *= -1;
                    _SR.flipX = _MoveSpeed > 0 ? true : _MoveSpeed < 0 ? false : false;
                    _RB.AddForce(new Vector2(_MoveSpeed, _RB.velocity.y));
                }
            }
        }
    }
}
