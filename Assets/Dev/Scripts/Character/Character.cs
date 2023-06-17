using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Dev.Scripts;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public static Character Instance
    {
        get;
        private set;
    }
    
    public List<Sprite> loadedSprites = new List<Sprite>();
    public Dictionary<GemType, int> soldGems = new Dictionary<GemType, int>();
    public List<Gem> gemList = new List<Gem>();
    public List<GemType> soldGem = new List<GemType>();

    private readonly int gemOffset = 1;
    private int _gemOffsetIndex = 1;
    private bool _isInSaleArea = false;
    private string spriteKeyPrefix = "GemSprite_";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SignUpEvents();

        LoadSoldGems();
    }

    private void OnDisable()
    {
        SaveSoldGems();
    }

    private Dictionary<string, Sprite> iconDictionary = new Dictionary<string, Sprite>();

    private void SaveSoldGems()
    {
        PlayerPrefs.SetInt("SoldGemsCount", soldGems.Count);
        var index = 0;
    
        foreach (var pair in soldGems)
        {
            var keyPrefix = $"SoldGem_{index}_";
            PlayerPrefs.SetString(keyPrefix + "GemName", pair.Key.gemName);
            PlayerPrefs.SetInt(keyPrefix + "Quantity", pair.Value);
            index++;
        }

        PlayerPrefs.Save();
    }

    private void LoadSoldGems()
    {
        foreach (var gem in GridManager.Instance.gemTypeList)
        {
            loadedSprites.Add(gem.icon);
        }
        int count = PlayerPrefs.GetInt("SoldGemsCount");
        for (int index = 0; index < count; index++)
        {
            var keyPrefix = $"SoldGem_{index}_";
            var gemName = PlayerPrefs.GetString(keyPrefix + "GemName");
            var quantity = PlayerPrefs.GetInt(keyPrefix + "Quantity");
    
            GemType gemType = GetGemTypeByName(gemName);
            if (gemType != null)
            {
                soldGem.Add(gemType);
                soldGems[gemType] = quantity;
                MatchSpritesToGemTypes(loadedSprites, gemType);
            }
        }
    }
    
    private void MatchSpritesToGemTypes(List<Sprite> sprites,GemType gemType)
    {
        foreach (Sprite sprite in sprites)
        {
            if (gemType.gemName == sprite.name)
            {
                gemType.icon = sprite;
                break;
            }
        }
    }
    
    private GemType GetGemTypeByName(string gemName)
    {
        foreach (GemType gemType in GridManager.Instance.gemTypeList)
        {
            if (gemType.gemName.Equals(gemName))
            {
                return gemType;
            }
        }

        return null; 
    }
    
    private void SignUpEvents()
    {
        GameEvents.GemCollectedEvent += CollectGem;
    }
    
    private void CollectGem(Gem gem)
    {
        gem.transform.SetParent(transform);

        var gemStackOffset = _gemOffsetIndex * gemOffset;
        var targetPosition = new Vector3(0f, gemStackOffset, -1.5f);

        gem.transform.DOLocalMove(targetPosition, .5f)
            .SetEase(Ease.OutExpo);

        gemList.Add(gem);
        _gemOffsetIndex++;
        
    }
    
    private void SellGems(Gem gem)
    {
        gemList.Remove(gem);
        _gemOffsetIndex--;
        
        if (!soldGems.ContainsKey(gem.gemType))
        {
            soldGems[gem.gemType] = 1;
        }
        else
        {
            soldGems[gem.gemType]++;
        }

        /*gem.transform.DOMove(saleAreaTransform.position, 1f).SetEase(Ease.OutBounce).OnUpdate(() =>
        {
            gem.transform.DOScale(0.2f, 1f).SetDelay(.5f);
        }).OnComplete(() => GemPool.Instance.ReturnGem(gem.gameObject));*/
        
        GemPool.Instance.ReturnGem(gem.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SaleArea"))
        {
            _isInSaleArea = true;
            StartCoroutine(SellGemsCoroutine());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("SaleArea"))
        {
            _isInSaleArea = false;
        }
    }
    private IEnumerator SellGemsCoroutine()
    {
        while (gemList.Count > 0)
        {
            if (!_isInSaleArea)
                break;
            
            var gem = gemList[gemList.Count - 1];
            var gemType = gem.GetComponent<Gem>().gemType;
            int gemValue = gemType.startingPrice + (int)(gem.transform.localScale.x * 100);
            GameEvents.GemSoldEvent?.Invoke(gemValue,gem);
            SellGems(gem);
            yield return new WaitForSeconds(.1f);
        }
    }
    
}
