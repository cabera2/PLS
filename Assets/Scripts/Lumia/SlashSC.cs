using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSC : MonoBehaviour
{
    private Rigidbody2D _RB;
    private AudioSource _AS;
    public AudioClip _RejectSFX;
    // Use this for initialization
    void Start()
    {
        _RB = transform.parent.GetComponent<Rigidbody2D>();
        _AS = transform.parent.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 12 && (col.gameObject.GetComponent<EnemyCommonSC>() == null || col.gameObject.GetComponent<EnemyCommonSC>()._HP > 0) || col.gameObject.tag == "Switch" && col.gameObject.GetComponent<SwitchSC>()._On == false || col.gameObject.tag == "EnemyWeapon")
        {
            if (col.gameObject.name != "Bubble(Clone)")
            {
                transform.parent.GetComponent<Lumia_SC>()._KnockbackCounter = 0.1f;
            }

            if (transform.rotation == Quaternion.Euler(0, 0, 90))
            {
                _RB.velocity = Vector2.left * 5;
            }
            else if (transform.rotation == Quaternion.Euler(0, 0, -90))
            {
                _RB.velocity = Vector2.right * 5;
            }
            else if (transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                _RB.velocity = Vector2.up * 15;
            }

            if (col.gameObject.tag == "EnemyWeapon")
            {
                _Reject();
            }
        }
    }
    public void _Reject()
    {
        _AS.PlayOneShot(_RejectSFX, SysSaveSC._Vol_Master * SysSaveSC._Vol_SFX * 0.01f);
    }
}
