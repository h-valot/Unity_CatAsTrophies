using UnityEngine;

public class DebugDrawButton : MonoBehaviour
{
    public void Press()
    {
        HandManager.Instance.DrawCat(DeckManager.Instance.RemoveCat());
    }
}