using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts;
using UnityEngine;

public class TotalGemCanvas : MonoBehaviour
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform itemTransform;
    private void OnEnable()
    {
        CreateItem();
    }

    private void OnDisable()
    {
        foreach (Transform child in itemTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateItem()
    {
        List<Gem> gemStack = Character.Instance.gemStack;
        Dictionary<GemType, int> gemTypeCounts = new Dictionary<GemType, int>();
        
        foreach (var gem in gemStack)
        {
            if (!gemTypeCounts.ContainsKey(gem.gemType))
            {
                gemTypeCounts[gem.gemType] = 1;
            }
            else
            {
                gemTypeCounts[gem.gemType]++;
            }
        }
        
        foreach (var kvp in gemTypeCounts)
        {
            var item = Instantiate(itemPrefab.gameObject, itemTransform);
            item.GetComponent<Item>().itemImage.sprite = kvp.Key.icon;
            item.GetComponent<Item>().itemCountText.text = "Collected Count : "+kvp.Value;
            item.GetComponent<Item>().itemTypeText.text = "Gem Type : "+kvp.Key.gemName;
        }

    }
}
