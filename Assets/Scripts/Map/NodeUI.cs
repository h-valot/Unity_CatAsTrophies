using System;
using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [Header("REFERENCES")]
    public Image nodeImage;
    public Image animationCircleImage;
    public float animationCircleDuration = 0.3f;
    public float durationBeforeLoadingScene = 0.5f;
    
    [Header("DEBUGGING")]
    public NodeStates state;
    public Node node;

    public void Initialize(Node node)
    {
        this.node = node;
        nodeImage.sprite = node.sprite;
        nodeImage.transform.localScale *= Registry.mapConfig.nodeScaleModifier;
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
                nodeImage.color = Registry.mapConfig.nodeLockedColor;
                break;
            case NodeStates.VISITED:
                nodeImage.color = Registry.mapConfig.nodeVisitedColor;
                break;
            case NodeStates.ATTAIGNABLE:
                nodeImage.color = Registry.mapConfig.nodeAttaignableColor;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public void PressNodeUI()
    {
        // exit, if the node state isn't attaignable
        if (state is NodeStates.LOCKED or NodeStates.VISITED) return;
        
        SetState(NodeStates.VISITED);
        MapPlayerTracker.Instance.EnterNode(this);
    }
    
    public async Task ShowSelectionAnimation()
    {
        // exit, if the circle image is null
        if (animationCircleImage == null) return;
        
        animationCircleImage.fillAmount = 0f;
        DOTween.To(() => animationCircleImage.fillAmount, x => animationCircleImage.fillAmount = x, 1f, animationCircleDuration);
        await Task.Delay((int)(1000 * durationBeforeLoadingScene));
    }
}

public enum NodeStates
{
    LOCKED = 0,
    VISITED,
    ATTAIGNABLE
}