using Unity.VisualScripting;

namespace Data
{
    [System.Serializable]
    public class PersistantData
    {
        public PersistantData(PersistantData data)
        {
            // exit, if there is no data to load
            if (data == null) return;
            
            // sync data
            map = data.map;
            MusicVolumeValue = data.MusicVolumeValue;
        }
        
        // current loaded and displayed map
        public Map map;

        public float MusicVolumeValue;
    }
}