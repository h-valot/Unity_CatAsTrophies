using System;
using System.Collections.Generic;

namespace Player
{
    [System.Serializable]
    public class Item
    {
        public int entityIndex;

        public List<CatData> data = new List<CatData>();
        public int amount;
        public Action onChanged;

        public Item(int newEntityIndex = 0)
        {
            entityIndex = newEntityIndex;
        }

        public void Add(float health, int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                data.Add(new CatData(entityIndex, health));
            }
        }

        public void Remove(int amount = 1)
        {
            for (int i = 0; i < amount; i++)
            {
                data.Remove(data[^1]);
            }
        }
    }
}