using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        /// Clears and copies the given list into the list
        /// </summary>
        public static void FillWith<T>(this IList<T> list, IList<T> listToCopy)
        {
            list.Clear();
            foreach (T item in listToCopy)
                list.Add(item);
        }
    }
}