using UnityEngine;

namespace Dev.Scripts
{
    [System.Serializable]
    public class Gem:MonoBehaviour
    {
        [HideInInspector] public GemType gemType;
    }
}