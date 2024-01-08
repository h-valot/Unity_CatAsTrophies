using System.Collections.Generic;

namespace List
{
    public static class ListExtensions 
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list) 
        {
            int count = list.Count;
            int last = count - 1;
            
            for (var i = 0; i < last; ++i) 
            {
                int rnd = UnityEngine.Random.Range(i, count);
                (list[i], list[rnd]) = (list[rnd], list[i]);
            }
        }

        /// <summary>
        /// Clears and fills the list with the given list  
        /// </summary>
        public static List<T> Copy<T>(this IList<T> list)
        {
            return new List<T>(list);
        }
    }
}