using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class ShieldEnmSC : EnemyParentSC
    {
        private SpriteRenderer _SR_C;
        private GameObject _Lumia;
        private GameObject _Child;
        private bool _InRange;
        private float _HalfWidth;
        private float _Timer;
        private int _Dir;
        public LayerMask _GroundLayer;
        public int _Pattern;
        public float _WalkSpd;
        public bool _FacingR;
        public float _Reach;
        public float _ChainSawSpd;
        // Start is called before the first frame update
        void Start()
        {
            base._Caching();
            StartCoroutine(_StartC());
            _HalfWidth = GetComponent<BoxCollider2D>().size.x / 2;
            _Child = transform.GetChild(0).gameObject;
            _SR_C = _Child.GetComponent<SpriteRenderer>();
        }
        IEnumerator _StartC()
        {
            while (StageManagerSC._LumiaInst == null)
            {
                yield return null;
            }
            _Lumia = StageManagerSC._LumiaInst;
        }
        // Update is called once per frame
        void Update()
        {
            if (_Common._HP > 0)
            {
                if (_Lumia != null)
                {
                    _Dir = _SR.flipX == true ? 1 : _SR.flipX == false ? -1 : 0;
                    if (Vector2.Distance(transform.position, _Lumia.transform.position) < 5 && Mathf.Abs(transform.position.y - _Lumia.transform.position.y) < 2 && ((_Lumia.transform.position.x - transform.position.x) * _Dir) > 0)
                    {
                        _InRange = true;
                    }
                    else
                    {
                        _InRange = false;
                    }
                    if (_Pattern == 0)
                    {
                        Vector3 WallDetectPos;
                        Vector3 EdgeDetectPos;
                        WallDetectPos = transform.position + new Vector3(_Dir * _HalfWidth, 0.3f, 0f);
                        EdgeDetectPos = transform.position + new Vector3(_Dir * _HalfWidth, 0f, 0f);
                        bool _HitWall = Physics2D.OverlapCircle(WallDetectPos, 0.1f, _GroundLayer);
                        bool _ReachEdge = Physics2D.OverlapCircle(EdgeDetectPos, 0.1f, _GroundLayer);
                        if (_HitWall == true || _ReachEdge == false)
                        {
                            _SR.flipX = !_SR.flipX;
                        }
                        _RB.velocity = new Vector2(_WalkSpd * _Dir, 0f);
                        if (_InRange == true)
                        {
                            _Pattern = 1;
                        }
                    }
                    else if (_Pattern == 1)
                    {
                        _Common._GuardFront = true;
                        _Ani.SetInteger("_Pattern", 1);
                        _Timer += Time.deltaTime;
                        if (_Timer >= 1)
                        {
                            _Timer = 0;
                            _Pattern = 2;
                            _Common._GuardFront = false;
                        }
                    }
                    else if (_Pattern == 2)
                    {
                        _Ani.SetInteger("_Pattern", 2);
                    }
                }
            }
            else
            {
                _Child.SetActive(false);
                enabled = false;
            }
        }
        void _Attack()
        {
            StartCoroutine(_Stretch());
            _Child.SetActive(true);
        }
        IEnumerator _Stretch()
        {
            while (_Child.transform.localPosition.x * _Dir < _Reach)
            {
                yield return null;
                _Child.transform.position += new Vector3((_Reach - Mathf.Abs(_Child.transform.localPosition.x) + 0.01f) * _ChainSawSpd * _Dir, 0, 0);
                _SR_C.size = new Vector2(-_Child.transform.localPosition.x, _SR_C.size.y);
            }
            yield return new WaitForSeconds(1);
            while (_Child.transform.localPosition.x * _Dir > 0)
            {
                yield return null;
                _Child.transform.position += new Vector3((_Reach - Mathf.Abs(_Child.transform.localPosition.x) + 0.01f) * _ChainSawSpd * -_Dir, 0, 0);
                _SR_C.size = new Vector2(-_Child.transform.localPosition.x, _SR_C.size.y);
            }
            _Child.SetActive(false);
            if (_InRange == true)
            {
                _Pattern = 1;
                _Ani.SetInteger("_Pattern", 1);
            }
            else
            {
                _Pattern = 0;
                _Ani.SetInteger("_Pattern", 0);
            }
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (((col.gameObject.name == "PlayerDetector" && col.GetComponent<PlayerDetectorSC>()._Grounded == false) || col.gameObject.name == "Slash(Clone)") && (_Pattern == 0 || _Pattern == 1))
            {
                float LPos = _Lumia.transform.position.x - transform.position.x;
                _SR.flipX = LPos > 0 ? true : LPos < 0 ? false : false;
            }
        }
    }
}
