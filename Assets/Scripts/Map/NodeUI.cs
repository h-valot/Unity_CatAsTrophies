using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public Image image;

    public void Initialize(Sprite sprite)
    {
        image.sprite = sprite;
    }
}