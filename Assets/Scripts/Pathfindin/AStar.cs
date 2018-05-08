using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public TileManager tm;
    public Transform start;
    public Transform end;

    private int getDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }

    private void Start()
    {
    }

    
    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = tm.GetNodeFromPos(startPos);
        Node endNode = tm.GetNodeFromPos(endPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node curNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < curNode.fCost ||
                    ((openSet[i].fCost == curNode.fCost) && openSet[i].hCost < curNode.hCost))
                {
                    curNode = openSet[i];
                }
            }

            openSet.Remove(curNode);
            closedSet.Add(curNode);

            if (curNode == endNode)
            {
                return retracePath(startNode, endNode);

            }
            foreach (Node n in tm.GetNeighbors(curNode))
            {
                if (n.walkable == false || closedSet.Contains(n))
                {
                    continue;
                }

                int newMovementcostToNeighbor = curNode.gCost + getDistance(curNode, n);

                if (newMovementcostToNeighbor < n.gCost || openSet.Contains(n) == false)
                {
                    n.gCost = newMovementcostToNeighbor;
                    n.hCost = getDistance(n, endNode);
                    n.parent = curNode;

                    if (openSet.Contains(n) == false)
                    {
                        openSet.Add(n);
                    }

                }

            }

        }

        return new List<Vector3>();

    }

    List<Vector3> retracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();

        Node curNode = endNode;
        while (curNode != startNode)
        {
            path.Add(tm.NodeToWorld(curNode));
            curNode = curNode.parent;
            curNode.resetCosts();
        }

        curNode.resetCosts();
        path.Reverse();
        

        return path;
    }

}


