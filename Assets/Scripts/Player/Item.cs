using System;
using System.Collections.Generic;

namespace Player
{
    [System.Serializable]
    public class Item
    {
        public int entityIndex;
        public Action onDataChanged;
        public List<CatData> data = new List<CatData>();

        public Item(int newEntityIndex = 0)
        {
            entityIndex = newEntityIndex;
        }

        public void Add(CatData data, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
                this.data.Add(data);

            onDataChanged?.Invoke();
        }

        public void Remove(int amount = 1)
        {
            // exit, if data is empty
            if (data.Count <= 0) return;
            
            for (int i = 0; i < amount; i++)
                data.Remove(data[^1]);
            
            onDataChanged?.Invoke();
        }
    }
}