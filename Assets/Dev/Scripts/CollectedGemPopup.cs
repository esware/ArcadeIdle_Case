using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts;
using UnityEngine;

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
        
        if (!PlayerPrefs.HasKey("SoldGems"))
            return;
        
        string savedGemData = PlayerPrefs.GetString("SoldGems");
        List<Gem> loadedGems = JsonUtility.FromJson<GemListWrapper>(savedGemData).Gems;
        Dictionary<GemType, int> gemTypeCounts = new Dictionary<GemType, int>();
        
        foreach (var gem in Character.Instance.soldGems)
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
            
            PlayerPrefs.SetInt(kvp.Key.gemName,kvp.Value);
        }

    }
}