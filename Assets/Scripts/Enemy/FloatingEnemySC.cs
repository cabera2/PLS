using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEnemySC : EnemyParentSC
{
    private GameObject _Lumia;
    public GameObject _Bullet;

    public float _ChaseDistance;
    public float _ChaseSpeed;
    private bool _IsChasing;
    private Vector2 _FloatPosBase;
    private Vector3 _FloatPosNew;
    private Vector2 _Target;
    public float _FloatRange;
    public float _FloatSpeed;
    private float _CurrentSpeed;
    public float _Accel;
    private bool _TooClose;
    public AudioClip _SFX;
    private AudioSource _AS;
    [HideInInspector] public List<GameObject> _BulletPool = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        base._Caching();
        _FloatPosBase = transform.position;
        StartCoroutine(_FloatPosSet());
        StartCoroutine(_StartC());
        if (_Bullet != null)
        {
            _Ani.SetBool("_SType", true);
            StartCoroutine(_Shooting());
        }
    }
    public override void _Appear()
    {
        StartCoroutine(_AppearC());
    }
    IEnumerator _AppearC()
    {
        base._Caching();
        CircleCollider2D _CC = GetComponent<CircleCollider2D>();
        _CC.enabled =false;
        _SR.color = new Color(1f,1f,1f,0f);
        float _Alpha = 0;
        while (_SR.color.a < 1)
        {
            _Alpha += 0.05f;
            _SR.color = new Color(1f, 1f, 1f, _Alpha);
            transform.position += (Vector3.down * 0.01f);
            yield return null;
        }
        _CC.enabled = true;
    }
    IEnumerator _StartC()
    {
        while (StageManagerSC._LumiaInst == null)
        {
            yield return null;
        }
        _Lumia = StageManagerSC._LumiaInst;
        if (_Bullet != null)
        {
            _AS = _Lumia.GetComponent<AudioSource>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_Common._HP > 0)
        {
            if (_Lumia != null)
            {
                float _Distance;
                _Distance = Vector2.Distance(_Lumia.transform.position, transform.position);
                if (_Distance < 25)
                {
                    if (_Distance > _ChaseDistance)
                    {
                        _Ani.SetBool("_Chasing", false);
                        Hovering();
                    }
                    else
                    {
                        _Ani.SetBool("_Chasing", true);
                        if (_Bullet != null)
                        {
                            if (_Distance > 6)
                            {
                                _TooClose = false;
                            }
                            else if (_Distance < 4)
                            {
                                _TooClose = true;
                            }

                            if (_TooClose == false)
                            {
                                Chase(1);
                            }
                            else
                            {
                                Chase(-0.5f);
                            }

                        }
                        else
                        {
                            Chase(1);
                        }
                    }
                }
            }
        }
        else
        {
            _RB.constraints = RigidbodyConstraints2D.None;
        }
    }
    void Chase(float dir)
    {
        if (transform.position.x >= _Lumia.transform.position.x)
        {
            _SR.flipX = false;
        }
        if (transform.position.x < _Lumia.transform.position.x)
        {
            _SR.flipX = true;
        }
        _IsChasing = true;
        _CurrentSpeed = Mathf.Lerp(_CurrentSpeed, _ChaseSpeed * dir, _Accel * Time.unscaledDeltaTime);
        transform.position = Vector3.MoveTowards(transform.position, _Lumia.transform.position, _CurrentSpeed * Time.deltaTime);
    }
    void Hovering()
    {
        if (transform.position.x >= _Target.x)
        {
            _SR.flipX = false;
        }
        if (transform.position.x < _Target.x)
        {
            _SR.flipX = true;
        }
        if (_IsChasing == true)
        {
            _FloatPosBase = transform.position;
            _IsChasing = false;
        }
        if (_IsChasing == false)
        {
            _CurrentSpeed = Mathf.Lerp(_CurrentSpeed, _FloatSpeed, _Accel * Time.unscaledDeltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _Target, _CurrentSpeed * Time.deltaTime);
        }
    }
    IEnumerator _FloatPosSet()
    {
        Vector2 Dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        _Target = Dir.normalized * _FloatRange + _FloatPosBase;
        yield return new WaitForSeconds(1f);
        StartCoroutine(_FloatPosSet());
    }
    IEnumerator _Shooting()
    {
        while (_Common._HP > 0)
        {
            if (_IsChasing == true)
            {
                GameObject _NewBullet = null;
                if (_BulletPool.Count == 0)
                {
                    _NewBullet = Instantiate(_Bullet);
                    _NewBullet.GetComponent<BulletSC>()._Pool = _BulletPool;
                }
                else
                {
                    _NewBullet = _BulletPool[0];
                    _NewBullet.SetActive(true);
                }
                _AS.PlayOneShot(_SFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
                _NewBullet.transform.position = transform.position;

                Vector3 dir = _Lumia.transform.position + Vector3.up * 0.7f - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
                _NewBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                _NewBullet.GetComponent<Rigidbody2D>().velocity = -_NewBullet.transform.up * 10.0f;
                yield return new WaitForSeconds(3.0f);
            }
            yield return null;
        }
    }
}
