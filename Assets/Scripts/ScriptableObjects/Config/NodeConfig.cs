using UnityEngine;

[CreateAssetMenu(fileName = "NodeConfig", menuName = "Config/Map/Node", order = 2)]
public class NodeConfig : ScriptableObject
{
    public NodeType nodeType;
    public Sprite sprite;
}