using UnityEngine;

[System.Serializable]
public class FloatMinMax
{
    public float min, max;
    
    /// <summary>
    /// Returns a random float in range between min and max
    /// </summary>
    public float GetValue() => Random.Range(min, max);
}

[System.Serializable]
public class IntMinMax
{
    public int min, max;
    
    /// <summary>
    /// Returns a random int in range between min and max
    /// </summary>
    public int GetValue() => Random.Range(min, max + 1);
}