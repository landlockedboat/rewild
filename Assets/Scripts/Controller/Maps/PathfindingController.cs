using System;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(NodeMap))]
public class PathfindingController : BitController<PathfindingController> {

	private NodeMap _nodeMap;

	private void Awake() {
		_nodeMap = GetComponent<NodeMap>();
	}

	public IEnumerable<Node> FindPath(Vector2Int startPos, Vector2Int targetPos) {

		var startNode = _nodeMap.GetNode(startPos);
		var targetNode = _nodeMap.GetNode(targetPos);

		var openSet = new List<Node>();
		var closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			var node = openSet[0];
			for (var i = 1; i < openSet.Count; i ++)
			{
				if (openSet[i].FCost >= node.FCost && openSet[i].FCost != node.FCost) continue;

				if (openSet[i].HCost < node.HCost)
				{
					node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode) {
				return RetracePath(startNode,targetNode);
			}

			foreach (var neighbour in _nodeMap.GetNeighbours(node)) {
				if (!neighbour.Walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				var newCostToNeighbour = node.GCost + GetDistance(node, neighbour);
				if (newCostToNeighbour >= neighbour.GCost && openSet.Contains(neighbour)) continue;
				
				neighbour.GCost = newCostToNeighbour;
				neighbour.HCost = GetDistance(neighbour, targetNode);
				neighbour.Parent = node;

				if (!openSet.Contains(neighbour))
					openSet.Add(neighbour);
			}
		}
		
		throw new Exception("Path not found");
	}

	private IEnumerable<Node> RetracePath(Node startNode, Node endNode) {
		var path = new List<Node>();
		var currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		path.Reverse();
		return path;
	}

	private static int GetDistance(Node nodeA, Node nodeB)
	{
		var dstX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
		var dstY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}


}