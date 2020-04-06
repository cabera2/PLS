using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniAutoDestroySC : MonoBehaviour
{
    public float delay = 0f;
    public bool _DestroyParent;
    // Use this for initialization
    void Start()
    {
        if (_DestroyParent == false)
        {
            Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }
        else if (_DestroyParent == true)
        {
            Destroy(transform.parent.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
