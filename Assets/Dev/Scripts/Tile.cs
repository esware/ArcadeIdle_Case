using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    private GameObject _gemInstance;
    private bool _isCollectible = false;
    
    private void Awake()
    {
        SpawnGem();
    }
    
    private void SpawnGem()
    {
        if (_gemInstance != null)
        {
            Destroy(_gemInstance);
        }

        _gemInstance = GemPool.Instance.GetGem();
        _gemInstance.transform.SetParent(transform);
        _gemInstance.transform.localScale = Vector3.zero;
        _gemInstance.transform.localPosition = Vector3.zero;

        _isCollectible = false;

        StartCoroutine(ScaleGem());
    }

    private void CollectGem()
    {
        if (_isCollectible)
        { 
            GameEvents.GemCollectedEvent?.Invoke(_gemInstance.GetComponent<Gem>());
            _gemInstance = null;
            
            SpawnGem();
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
