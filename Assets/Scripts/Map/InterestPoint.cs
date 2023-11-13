using System.Collections.Generic;

public class InterestPoint
{
    public string id;
    
    public int stage;
    public InterestPointType type;
    public List<InterestPoint> nextInterestPoints;

    public InterestPoint(int _stage)
    {
        id = Misc.GetRandomId();
        nextInterestPoints = new List<InterestPoint>();
        stage = _stage;
    }

    /// <summary>
    /// Start game scene corresponding to the type of the interest point
    /// </summary>
    public void Trigger()
    {
        switch (type)
        {
            case InterestPointType.Battle:
                // start game battle scene with a given enemy composition
                break;
            case InterestPointType.Shop:
                break;
            case InterestPointType.Merge:
                break;
            case InterestPointType.Graveyard:
                break;
            case InterestPointType.Event:
                break;
            case InterestPointType.Campfire:
                break;
        }
    }
}

public enum InterestPointType
{
    Battle = 0,
    Shop,
    Merge,
    Graveyard,
    Event,
    Campfire
}