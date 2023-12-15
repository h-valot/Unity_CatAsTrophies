[System.Serializable]
public class EditorItem
{
    public int entityIndex;
    public int amount;
    
    public float health;

    public EditorItem(float newHealth = 0, int newEntityIndex = 0)
    {
        entityIndex = newEntityIndex;
        health = newHealth;
        amount = 1;
    }
}