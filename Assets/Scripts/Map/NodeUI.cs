using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public Image image;
    public NodeStates state;
    public Node node;

    public void Initialize(Node node)
    {
        this.node = node;
        image.sprite = node.GetSprite();
    }
    public void SetState(NodeStates state)
    {
        this.state = state;
    }
}

public enum NodeStates
{
    LOCKED = 0,
    VISITED,
    ATTAIGNABLE
}