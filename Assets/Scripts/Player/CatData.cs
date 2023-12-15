[System.Serializable]
public class CatData
{
    public int entityIndex;
    
    public float health;
    public bool isDead;

    public CatData(int entityIndex, float health)
    {
        this.entityIndex = entityIndex;
        this.health = health;
    }
    
    public void Reset()
    {
        health = Registry.entitiesConfig.cats[entityIndex].health;
        isDead = false;
    }
}