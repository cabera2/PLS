using System.Collections.Generic;
using UnityEngine;

public class ActiveRemembererSc : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private List<GameObject> isActive;
    [SerializeField] private List<GameObject> isInactive;
    [ContextMenu("Remember")]
    private void Remember()
    {
        Remember(targetObject);
    }
    private void Remember(GameObject target)
    {
        if (target.activeSelf)
        {
            isActive.Add(target);
        }
        else
        {
            isInactive.Add(target);
        }
        foreach (Transform child in target.transform)
        {
            Remember(child.gameObject);
        }
    }
    [ContextMenu("Restore")]
    void Restore()
    {
        for (int i = 0; i < isActive.Count; i++)
        {
            isActive[i].SetActive(true);
        }
        for (int i = 0; i < isInactive.Count; i++)
        {
            isInactive[i].SetActive(false);
        }
    }
}
