using System;
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
        UpdateSpriteColor();
    }

    private void UpdateSpriteColor()
    {
        switch (state)
        {
            case NodeStates.LOCKED:
                image.color = Registry.mapConfig.nodeLockedColor;
                break;
            case NodeStates.VISITED:
                image.color = Registry.mapConfig.nodeVisitedColor;
                break;
            case NodeStates.ATTAIGNABLE:
                image.color = Registry.mapConfig.nodeAttaignableColor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public void PressNodeUI()
    {
        // exit, if the node state isn't attaignable
        if (state == NodeStates.LOCKED || state == NodeStates.VISITED) return;
        
        Debug.Log("NODE UI: button pressed");
        
        switch (node.nodeType)
        {
            case NodeType.BossBattle:
                break;
            case NodeType.EliteBattle:
                break;
            case NodeType.SimpleBattle:
                // load battle scene with a random composition of enemy
                break;
            case NodeType.Shop:
                break;
            case NodeType.Merge:
                break;
            case NodeType.Graveyard:
                break;
            case NodeType.Event:
                break;
            case NodeType.Campfire:
                break;
        }
        
        SetState(NodeStates.VISITED);
    }
}

public enum NodeStates
{
    LOCKED = 0,
    VISITED,
    ATTAIGNABLE
}