using System;

namespace Player
{
    [System.Serializable]
    public class Item
    {
        public int entityIndex;

        public float health;
        public bool isDead;
        public Action onChanged;

        public Item(int newEntityIndex = 0, float newHealth = 0)
        {
            entityIndex = newEntityIndex;
            health = newHealth;
        }
    }
}