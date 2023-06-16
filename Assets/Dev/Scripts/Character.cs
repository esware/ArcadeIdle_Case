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

public class Character : MonoBehaviour
{
    public static Character Instance
    {
        get;
        private set;
    }

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

    private void OnDestroy()
    {
        SaveSoldGems();
    }

    private void SaveSoldGems()
    {
        PlayerPrefs.SetInt("SoldGemsCount", soldGems.Count);
        int index = 0;
        foreach (var pair in soldGems)
        {
            string keyPrefix = $"SoldGem_{index}_";
            PlayerPrefs.SetString(keyPrefix + "GemName", pair.Key.gemName);
            PlayerPrefs.SetInt(keyPrefix + "Quantity", pair.Value);
            
            string spriteKey = spriteKeyPrefix + index.ToString();
            SaveSprite(pair.Key.icon, spriteKey);

            index++;
        }

        PlayerPrefs.Save();
        

    }

    private void LoadSoldGems()
    {
        int count = PlayerPrefs.GetInt("SoldGemsCount");
        for (int index = 0; index < count; index++)
        {
            string keyPrefix = $"SoldGem_{index}_";
            string gemName = PlayerPrefs.GetString(keyPrefix + "GemName");
            int quantity = PlayerPrefs.GetInt(keyPrefix + "Quantity");
            
            GemType gemType = GetGemTypeByName(gemName);
            if (gemType != null)
            {
                Debug.Log("notnull");
                soldGem.Add(gemType);
                soldGems[gemType] = quantity;

                // Sprite'ı yükle
                string spriteKey = spriteKeyPrefix + index.ToString();
                gemType.icon = LoadSprite(spriteKey);
            }
            
        }
        
        foreach (var g in soldGem)
        {
            Debug.Log(g);
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

    private void SaveSprite(Sprite sprite, string key)
    {
        Texture2D texture = sprite.texture;
        Rect rect = sprite.textureRect;
        Vector2 pivot = sprite.pivot;

        byte[] textureData = texture.EncodeToPNG();

        PlayerPrefs.SetString(key + "_TextureData", System.Convert.ToBase64String(textureData));
        PlayerPrefs.SetFloat(key + "_RectX", rect.x);
        PlayerPrefs.SetFloat(key + "_RectY", rect.y);
        PlayerPrefs.SetFloat(key + "_RectWidth", rect.width);
        PlayerPrefs.SetFloat(key + "_RectHeight", rect.height);
        PlayerPrefs.SetFloat(key + "_PivotX", pivot.x);
        PlayerPrefs.SetFloat(key + "_PivotY", pivot.y);
    }

    private Sprite LoadSprite(string key)
    {
        byte[] textureData = System.Convert.FromBase64String(PlayerPrefs.GetString(key + "_TextureData", ""));
        float rectX = PlayerPrefs.GetFloat(key + "_RectX", 0f);
        float rectY = PlayerPrefs.GetFloat(key + "_RectY", 0f);
        float rectWidth = PlayerPrefs.GetFloat(key + "_RectWidth", 0f);
        float rectHeight = PlayerPrefs.GetFloat(key + "_RectHeight", 0f);
        float pivotX = PlayerPrefs.GetFloat(key + "_PivotX", 0.5f);
        float pivotY = PlayerPrefs.GetFloat(key + "_PivotY", 0.5f);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(textureData);

        Sprite sprite = Sprite.Create(texture, new Rect(rectX, rectY, rectWidth, rectHeight), new Vector2(pivotX, pivotY));

        return sprite;
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
