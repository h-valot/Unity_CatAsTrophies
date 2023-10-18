using UnityEngine;

public class DebugDrawButton : MonoBehaviour
{
    public void Press()
    {
        HandManager.Instance.DrawCat(0);
    }
}