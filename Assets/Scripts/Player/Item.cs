namespace Player
{
    [System.Serializable]
    public class Item
    {
        public int entityIndex;
        public int inGameHealth;
        public int count;

        public Item(int newEntityIndex, int newCount)
        {
            entityIndex = newEntityIndex;
            count = newCount;
        }
    }
}