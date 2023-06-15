using UnityEngine;

namespace Dev.Scripts
{
    public static class GemDataConverter
    {
        public static string ConvertToJson(Gem gemData)
        {
            return JsonUtility.ToJson(gemData);
        }

        public static Gem ConvertFromJson(string json)
        {
            return JsonUtility.FromJson<Gem>(json);
        }
    }
}