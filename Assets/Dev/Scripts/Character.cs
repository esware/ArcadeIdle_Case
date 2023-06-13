using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float gemMoveDuration = 0.5f;
    
    private readonly int gemOffset = 1;
    private int _gemOffsetIndex = 1;
    public List<GameObject> gemStack = new List<GameObject>();

    private void Awake()
    {
        SignUpEvents();
    }

    private void SignUpEvents()
    {
        GameEvents.GemCollectedEvent += CollectGem;
    }
    
    private void CollectGem(GameObject gem)
    {
        gem.transform.SetParent(transform);

        var gemStackOffset = _gemOffsetIndex * gemOffset;
        var targetPosition = new Vector3(0f, gemStackOffset, -1.5f);

        gem.transform.DOLocalMove(targetPosition, gemMoveDuration)
            .SetEase(Ease.OutElastic);
        
        gemStack.Add(gem);
        _gemOffsetIndex++;
        
    }

    private IEnumerator SellGemsCoroutine()
    {
        int totalGold = 0;
        while (gemStack.Count > 0)
        {
            GameObject topGem = gemStack[gemStack.Count - 1];
            var gemType = topGem.GetComponent<Gem>().gemType;
            int gemValue = gemType.startingPrice + (int)(topGem.transform.localScale.x * 100);
            totalGold += gemValue;

            gemStack.Remove(topGem);
            Destroy(topGem);
            _gemOffsetIndex--;
            Debug.Log("Sold Gem: " + topGem.name + " Gold Earned: " + gemValue);

            yield return new WaitForSeconds(0.4f);
        }

        Debug.Log("Total Gold: " + totalGold);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("SaleArea"))
        {
            StartCoroutine(SellGemsCoroutine());
        }
    }
}
