using System;
using System.Collections.Generic;

namespace Player
{
    [System.Serializable]
    public class Item
    {
        public int entityIndex;
        public Action onDataChanged;
        public List<CatData> cats = new List<CatData>();

        public Item(int newEntityIndex = 0)
        {
            entityIndex = newEntityIndex;
        }

        public void Add(CatData data, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
                cats.Add(data);

#if UNITY_EDITOR || UNITY_ANDROID
            onDataChanged?.Invoke();
#endif
        }

        public void Remove(int amount = 1)
        {
            // exit, if data is empty
            if (cats.Count <= 0) return;
            
            for (int i = 0; i < amount; i++)
                cats.Remove(cats[^1]);
            
#if UNITY_EDITOR || UNITY_ANDROID
            onDataChanged?.Invoke();
#endif
        }
    }
}