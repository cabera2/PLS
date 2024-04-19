using System;
using UnityEngine;

public class ParallexManagerClassic : MonoBehaviour
{
    [Serializable]
    private class BgItem
    {
        public Transform bgTransform;
        public float multiply;
    }
    [SerializeField] private BgItem[] bgItems;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundTransform;
    [ContextMenu("Convert")]
    private void Convert()
    {
        BG_ControlSC[] founded = FindObjectsOfType<BG_ControlSC>();
        bgItems = new BgItem[founded.Length];
        if (bgItems.Length == 0)
        {
            return;
        }
        cameraTransform = founded[0]._Camera;
        groundTransform = founded[0]._BaseBG;
        for(int i=0; i< founded.Length; i++)
        {
            BgItem newItem = new();
            newItem.bgTransform = founded[i].transform;
            newItem.multiply = founded[i]._PosMultiply;
            bgItems[i] = newItem;
            DestroyImmediate(founded[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i< bgItems.Length; i++)
        {
            Vector3 TargetPos = (groundTransform.position - cameraTransform.position) * bgItems[i].multiply;
            bgItems[i].bgTransform.position = new Vector3(TargetPos.x, TargetPos.y, bgItems[i].bgTransform.position.z);
        }

    }
}
