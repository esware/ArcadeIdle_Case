using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Dev.Scripts;
using TMPro;
using UnityEngine;

public class CollectedGemPopup : MonoBehaviour
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform itemTransform;
    [SerializeField] private TextMeshProUGUI totalSoldGoldText= null;
    
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
        foreach (var kvp in Character.Instance.soldGems)
        {
            var item = Instantiate(itemPrefab.gameObject, itemTransform);
            item.GetComponent<Item>().itemImage.sprite = kvp.Key.icon;
            item.GetComponent<Item>().itemCountText.text = "Collected Count : " + kvp.Value;
            item.GetComponent<Item>().itemTypeText.text = "Gem Type : " + kvp.Key.gemName;
        }

        totalSoldGoldText.text = "Total Gold :"+PlayerPrefs.GetInt("TotalSoldGold").ToString();
    }

}