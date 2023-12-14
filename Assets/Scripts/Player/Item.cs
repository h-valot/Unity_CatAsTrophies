using System;

namespace Player
{
    [System.Serializable]
    public class Item
    {
        public int entityIndex;
        public int inGameHealth;
        
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                onCountChanged?.Invoke();
            }
        }
        public Action onCountChanged;
        private int _count;

        public Item(int newEntityIndex, int newCount)
        {
            entityIndex = newEntityIndex;
            _count = newCount;
        }
    }
}