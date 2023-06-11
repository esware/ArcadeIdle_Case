namespace Dev.Scripts
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Gem Type", menuName = "Gem Type")]
    public class GemType : ScriptableObject
    {
        public string gemName;
        public int startingPrice;
        public Sprite icon;
        public GameObject model;
    }

}