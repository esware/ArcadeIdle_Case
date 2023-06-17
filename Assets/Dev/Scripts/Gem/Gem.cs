using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dev.Scripts
{
    [System.Serializable]
    public class Gem:MonoBehaviour
    {
        [SerializeField]
        public GemType gemType;
    }
}