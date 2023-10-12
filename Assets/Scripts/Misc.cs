using UnityEngine;

public static class Misc
{
    private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    public static string GetRandomId()
    {
        string output = "";
        
        for(int i = 0; i < 19; i++)
        {
            output += chars[Random.Range(0, chars.Length)];
        }

        return output;
    }
}