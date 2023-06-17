using System;
using UnityEngine;

namespace Dev.Scripts
{
    public struct GameEvents
    {
        public static Action<Gem> GemCollectedEvent;
        public static Action<int,Gem> GemSoldEvent;

    }
    public class GameManager:MonoBehaviour
    {
        [SerializeField] private GameObject player = null;


        private void Awake()
        {
            player.transform.position = Vector3.zero;
        }
    }
}