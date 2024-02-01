using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRemembererSc : MonoBehaviour
{
    [Serializable]
    private struct Item{
        public GameObject GameObject;
        public bool WasActive;
        public CanvasGroup CanvasGroup;
        public float Alpha;
    }
    [SerializeField] private GameObject targetObject;
    [SerializeField] private List<Item> items;
    [ContextMenu("Remember")]
    private void Remember()
    {
        Remember(targetObject);
    }
    private void Remember(GameObject target)
    {
        Item newItem = new Item()
        {
            GameObject = target,
            WasActive = target.activeSelf,
            CanvasGroup = target.GetComponent<CanvasGroup>(),
        };
        if (newItem.CanvasGroup != null)
        {
            newItem.Alpha = newItem.CanvasGroup.alpha;
        }
        items.Add(newItem);
        
        foreach (Transform child in target.transform)
        {
            Remember(child.gameObject);
        }
    }
    [ContextMenu("Restore")]
    void Restore()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].GameObject.SetActive(items[i].WasActive);
            if (items[i].CanvasGroup != null)
            {
                items[i].CanvasGroup.alpha = items[i].Alpha;
            }
        }
    }
}
