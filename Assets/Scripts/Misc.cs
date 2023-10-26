using UnityEngine;

public static class Misc
{
    private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    /// <summary>
    /// Get a random string id with down and upper letter and numbers
    /// </summary>
    /// <returns>Unique string id</returns>
    public static string GetRandomId()
    {
        string output = "";
        
        for(int i = 0; i < 19; i++)
        {
            output += chars[Random.Range(0, chars.Length)];
        }

        return output;
    }
    
    /// <summary>
    /// Get a reference to a cat thanks to its id.
    /// </summary>
    /// <param name="_id">Id of the cat</param>
    /// <returns>Cat that own the given id</returns>
    public static Cat GetCatById(string _id)
    {
        Cat? output = null;
        
        for (int i = 0; i < CatGenerator.Instance.cats.Count; i++)
        {
            if (CatGenerator.Instance.cats[i].id == _id)
            {
                output = CatGenerator.Instance.cats[i];
                break;
            }
        }
        
        return output;
    }

    /// <summary>
    /// Get a reference to an entity thanks to its id.
    /// </summary>
    /// <param name="_id">Id of the entity</param>
    /// <returns>Entity that own the given id</returns>
    public static Entity GetEntityById(string _id)
    {
        Entity output = new Entity();
        
        for (int i = 0; i < EntityManager.Instance.entities.Count; i++)
        {
            if (EntityManager.Instance.entities[i].id == _id)
            {
                output = EntityManager.Instance.entities[i];
                break;
            }
        }
        
        return output;
    }
}