using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxDebugger : MonoBehaviour
{
    [SerializeField] private ParallaxManagerSC controller;
    void Update()
    {
        if (controller != null)
        {
            controller.Move();
        }
    }
}
