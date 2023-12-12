namespace Player
{
    [System.Serializable]
    public class Item
    {
        public EntityConfig entity;
        public int count;

        public Item(EntityConfig newEntity, int newCount)
        {
            entity = newEntity;
            count = newCount;
        }
    }
}