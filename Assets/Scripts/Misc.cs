using System.Collections.Generic;
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
    /// <param name="_cats">List of cats to browse the cat</param>
    /// <param name="_id">Id of the cat</param>
    /// <returns>Cat that own the given id</returns>
    public static Cat GetCatById(List<Cat> _cats, string _id)
    {
        Cat output = new Cat();
        
        for (int i = 0; i < _cats.Count; i++)
        {
            if (_cats[i].id == _id)
            {
                output = _cats[i];
                break;
            }
        }
        
        return output;
    }
}