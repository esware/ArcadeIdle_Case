using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    private GameObject _gemInstance;
    private bool _isCollectible = false;
    
    private void Awake()
    {
        SpawnGem(GridManager.Instance.gemTypeList);
    }
    
    private void SpawnGem(List<GemType> gemTypes)
    {
        if (_gemInstance != null)
        {
            Destroy(_gemInstance);
        }

        var randomGemType = gemTypes[Random.Range(0, gemTypes.Count)];
        
        _gemInstance = Instantiate(randomGemType.model, transform);
        _gemInstance.AddComponent<Gem>();
        _gemInstance.GetComponent<Gem>().gemType = randomGemType;
        _gemInstance.transform.localScale = Vector3.zero;
        gameObject.name = randomGemType.gemName;
        
        _isCollectible = false;

        StartCoroutine(ScaleGem());
    }

    private void CollectGem()
    {
        if (_isCollectible)
        { 
            GameEvents.GemCollectedEvent?.Invoke(_gemInstance.GetComponent<Gem>());
            _gemInstance = null;
            
            SpawnGem(GridManager.Instance.gemTypeList);
        }
    }
    
    private IEnumerator ScaleGem()
    {
        float duration = 5f;
        float targetScale = 1f;

        Tweener tweener = _gemInstance.transform.DOScale(targetScale, duration).SetEase(Ease.OutCubic);

        tweener.OnUpdate(() =>
        {
            if (!_isCollectible && _gemInstance.transform.localScale.x >= 0.25f)
            {
                _isCollectible = true;
            }
        });

        yield return tweener.WaitForCompletion();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollectGem();
        }
    }
}
