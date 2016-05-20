namespace SpecterOps.Utilities
{
    using UnityEngine;
    using System.Collections.Generic;

    public static class ListExtension 
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var temp = list[i];
                int randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}
