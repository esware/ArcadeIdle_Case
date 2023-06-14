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
    public List<Gem> gemStack = new List<Gem>();

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
            .SetEase(Ease.InSine);

        gemStack.Add(gem);
        _gemOffsetIndex++;
        
    }

    private IEnumerator SellGemsCoroutine()
    {
        while (gemStack.Count > 0)
        {
            if (!_isInSaleArea)
                break;
            
            var topGem = gemStack[gemStack.Count - 1];
            var gemType = topGem.GetComponent<Gem>().gemType;
            int gemValue = gemType.startingPrice + (int)(topGem.transform.localScale.x * 100);

            GameEvents.GemSelledEvent?.Invoke(gemValue,topGem);
            
            gemStack.Remove(topGem);
            Destroy(topGem.gameObject);
            _gemOffsetIndex--;
            yield return new WaitForSeconds(.1f);
        }
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
    
}
