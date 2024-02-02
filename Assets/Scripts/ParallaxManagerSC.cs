using System;
using UnityEngine;

public class ParallaxManagerSC : MonoBehaviour
{
    [Serializable]
    private class BgItem
    {
        public Transform bgTransform;
        public float multiply;
    }
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundTransform;
    [SerializeField] private bool useZPos = false;
    [Header("Fill Below if you use zoom")]
    [SerializeField] private bool enableZoom;
    [SerializeField] private Camera cameraCompnent;
    [SerializeField] private float defaultCamSize;
    [SerializeField] private BgItem[] bgItems;

    void Update()
    {
        Move();
    }
    public void Move()
    {
        float zoomScale = 1;
        if (enableZoom)
        {
            zoomScale = cameraCompnent.orthographicSize / defaultCamSize;
        }
        for (int i = 0; i < bgItems.Length; i++)
        {
            var multiply = useZPos ? bgItems[i].bgTransform.position.z - groundTransform.position.z : bgItems[i].multiply;
            float layerScale = 1;
            if (enableZoom)
            {
                layerScale =  Mathf.Pow(zoomScale, multiply);
                Vector3 zoomVector = new Vector3(layerScale, layerScale, 1);
                bgItems[i].bgTransform.localScale = zoomVector;
            }
            var camPos = cameraTransform.position;
            var posDiff = camPos - groundTransform.position;
            var targetPos = posDiff * multiply;
            targetPos = (targetPos - camPos) * layerScale + camPos; 
            targetPos.z = bgItems[i].bgTransform.position.z;
            bgItems[i].bgTransform.position = targetPos;

        }
    }
}