using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSC : MonoBehaviour
{
    [HideInInspector] public List<GameObject> _BBPool;
    private SpriteRenderer _SR;
    private ParticleSystem _PS;
    private CircleCollider2D _SC;
    private AudioSource _AS;
    public AudioClip _Pop;
    // Start is called before the first frame update
    void Awake()
    {
        _SR = GetComponent<SpriteRenderer>();
        _PS = GetComponent<ParticleSystem>();
        _SC = GetComponent<CircleCollider2D>();
        _AS = StageManagerSC._LumiaInst.GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        _SR.enabled = true;
        _SC.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8 || col.gameObject.tag == "Lumia" || col.gameObject.name == "PlayerDetector" || col.gameObject.name == "Slash(Clone)")
        {
            _AS.PlayOneShot(_Pop, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
            _SR.enabled = false;
            _SC.enabled = false;
            _PS.Play();
        }
    }
    public void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        _BBPool.Add(gameObject);
    }
}