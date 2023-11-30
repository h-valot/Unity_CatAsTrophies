using System.Collections.Generic;

namespace List
{
    public static class ListExtensions 
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> _list) 
        {
            int count = _list.Count;
            int last = count - 1;
            
            for (var i = 0; i < last; ++i) 
            {
                int rnd = UnityEngine.Random.Range(i, count);
                (_list[i], _list[rnd]) = (_list[rnd], _list[i]);
            }
        }
    }
}