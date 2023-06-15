using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Dev.Scripts;
using UnityEngine;
using Path = DG.Tweening.Plugins.Core.PathCore.Path;

public class CollectedGemPopup : MonoBehaviour
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
        if (Character.Instance.soldGems.Count <1)
            return;
        
        Dictionary<GemType, int> gemTypeDic = new Dictionary<GemType, int>();

        foreach (var gem in Character.Instance.soldGems)
        {
            if (!gemTypeDic.ContainsKey(gem.gemType))
            {
                gemTypeDic[gem.gemType] = 1;
            }
            else
            {
                gemTypeDic[gem.gemType]++;
            }
        }

        foreach (var kvp in gemTypeDic)
        {
            var item = Instantiate(itemPrefab.gameObject, itemTransform);
            item.GetComponent<Item>().itemImage.sprite = kvp.Key.icon;
            item.GetComponent<Item>().itemCountText.text = "Collected Count : " + kvp.Value;
            item.GetComponent<Item>().itemTypeText.text = "Gem Type : " + kvp.Key.gemName;
        }
    }

}