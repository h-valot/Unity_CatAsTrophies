using System.Collections.Generic;

public class MapManager
{
    public static MapManager Instance;
    
    public List<InterestPoint> interestPoints;

    public int stageAmount = 7;
    public int maxInterestPointPerStage = 5;
    public int maxInterestPointFirstStage = 3;
    public float spawnBattleChance, spawnShopChance, spawnEventChance, spawnMergeChance, spawnGraveyardChance, spawnCampfireChance;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        interestPoints = new List<InterestPoint>();
        GenerateMap();
    }
    
    public void GenerateMap()
    {
        for (int stageIndex = 0; stageIndex < stageAmount; stageIndex++)
        {
            int interestPointPerStage;
            if (stageIndex == 0) interestPointPerStage = maxInterestPointFirstStage;
            else interestPointPerStage = maxInterestPointPerStage;
            
            for (int innerStageInterestPointIndex = 0; innerStageInterestPointIndex < interestPointPerStage; innerStageInterestPointIndex++)
            {
                interestPoints.Add(new InterestPoint(stageIndex));
                
                // based on a random gaussian number based itself on a seed, choose the interest point type 
            }
        }
    }

    public void SaveMapToAsset()
    {
        
    }

    public void DeleteMap()
    {
        
    }
}