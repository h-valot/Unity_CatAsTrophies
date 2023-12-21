using Player;

namespace Data
{
    [System.Serializable]
    public class PersistantData
    {
        // current loaded and displayed map
        public Map map;

        // sound musics
        public float musicVolume;
        
        // level loading data
        public CompositionConfig compoToLoad;
        public EndBattleStatus endBattleStatus;
        
        // player collection
        public PlayerStorage playerStorage;
        public int tuna, treat;
    }
}

public enum EndBattleStatus
{
    NONE = 0,
    VICTORY,
    DEFEATED,
    LEFT
}