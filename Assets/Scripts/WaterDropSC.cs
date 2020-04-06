using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropSC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D()
    {
        GetComponent<ParticleSystem>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(DropEvent());
    }
    IEnumerator DropEvent()
    {
        yield return new WaitForSeconds(3f);
        transform.position = new Vector3(-20, 4, 0);
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
