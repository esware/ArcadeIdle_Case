using System;
using UnityEngine;

namespace Dev.Scripts
{

    public struct GameEvents
    {
        public static Action<Gem> GemCollectedEvent;
        public static Action<int,Gem> GemSelledEvent;

    }
    public class GameManager
    {
        
    }
}