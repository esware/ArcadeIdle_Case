using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Character Instance
    {
        get;
        private set;
    }

    public int totalGemCount = 0;
    public List<Gem> gemList = new List<Gem>();
    public List<Gem> soldGems = new List<Gem>();

    private readonly int gemOffset = 1;
    private int _gemOffsetIndex = 1;
    private bool _isInSaleArea = false;

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
    private void SellGemsAndSave(Gem gem)
    {
        gemList.Remove(gem);
        _gemOffsetIndex--;

    }

    private void LoadSoldGems()
    {
        
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
            
            totalGemCount += gemValue;
            PlayerPrefs.SetInt("TotalGem",totalGemCount);
            
            SellGemsAndSave(gem);
            GameEvents.GemSoldEvent?.Invoke(gemValue,gem);
            yield return new WaitForSeconds(.1f);
        }
    }
    
}
