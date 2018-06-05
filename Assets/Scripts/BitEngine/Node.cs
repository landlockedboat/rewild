using System;
using UnityEngine;

[Serializable]
public class Node
{
    public Vector2Int Position;
    public bool Walkable;

    public int GCost;
    public int HCost;

    public int FCost => GCost + HCost;

    public Node Parent;
}