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
        }
        
        // current loaded and displayed map
        public Map map;
    }
}