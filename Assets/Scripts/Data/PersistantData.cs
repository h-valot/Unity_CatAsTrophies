using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class PersistantData
    {
        // current loaded and displayed map
        [SerializeField] public Map map;

        // sound musics
        public float musicVolume;
        
        // level loading data
        [SerializeField] public CompositionConfig compoToLoad;
    }
    
}