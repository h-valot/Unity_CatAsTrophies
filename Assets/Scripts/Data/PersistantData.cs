using Unity.VisualScripting;

namespace Data
{
    [System.Serializable]
    public class PersistantData
    {
        public bool isSaved;

        // current loaded and displayed map
        public Map map;

        public float MusicVolumeValue = 0.5f;
    }
}