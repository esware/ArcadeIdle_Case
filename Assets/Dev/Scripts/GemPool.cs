using System.Collections.Generic;
using Dev.Scripts;
using UnityEngine;

public class GemPool
{
    private static GemPool _instance;
    public static GemPool Instance => _instance ??= new GemPool();

    private Queue<GameObject> _gemPool = new Queue<GameObject>();
    private List<GemType> _gemTypes = new List<GemType>();

    private GameObject _gemPrefab;
    private int _initialSize;

    private GemPool() { }

    public void Initialize( List<GemType> gemTypes, int initialSize)
    {
        _gemTypes = gemTypes;
        _initialSize = initialSize*2;
        
        for (int i = 0; i < _initialSize; i++)
        {
            GameObject gem = CreateRandomGem();
            _gemPool.Enqueue(gem);
        }
    }

    private GameObject CreateRandomGem()
    {
        var randomGemType = _gemTypes[Random.Range(0, _gemTypes.Count)];

        var gem = GameObject.Instantiate(randomGemType.model, GridManager.Instance.transform);
        gem.SetActive(false);
        gem.AddComponent<Gem>().gemType = randomGemType;
        return gem;
    }

    public GameObject GetGem()
    {
        if (_gemPool.Count == 0)
        {
            var gem = CreateRandomGem();
            gem.SetActive(true);
            return gem;
        }
        
        var pooledGem = _gemPool.Dequeue();
        pooledGem.SetActive(true);

        return pooledGem;
    }

    public void ReturnGem(GameObject gem)
    {
        gem.SetActive(false);
        _gemPool.Enqueue(gem);
    }
}