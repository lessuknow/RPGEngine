using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour {

    //The floor's tilemap.
    public Tilemap tlmp;
    public Tile openedDoor, defaultTile, enemyTile, enemyWalkTile;
    [SerializeField] Tile[] unwalkable;

    private Node[] nodeMap;
    int minX = int.MaxValue, maxX = int.MinValue;
    int minY = int.MaxValue, maxY = int.MinValue;
    int totalNodeNum = 0;

    //This converts a world position into it's tile position, and back into a world position.
    //Basically, this "centers" the given world position onto a tile.
    public void Awake()
    {
        RecalculateNodemap();
    }

    private void RecalculateNodemap()
    {
        tlmp.CompressBounds();
        totalNodeNum = tlmp.cellBounds.size.x * tlmp.cellBounds.size.y * tlmp.cellBounds.size.z;
       // TileBase
        nodeMap = new Node[totalNodeNum];

        int i = 0;
        foreach (var position in tlmp.cellBounds.allPositionsWithin)
        {
            Node n = new Node();
            n.gridX = position.x;
            n.gridY = position.y;

            foreach(Tile x in unwalkable)
            {
                if(tlmp.GetTile(position) == x)
                {
                    n.walkable = false;
                }
            }

            if (n.gridX < minX)
                minX = n.gridX;
            if (n.gridX > maxX)
                maxX = n.gridX;

            if (n.gridY < minY)
                minY = n.gridY;
            if (n.gridY > maxY)
                maxY = n.gridY;

            nodeMap[i] = n;
            i++;
        }

    }

    public Node[] GetNeighbors(Node n)
    {
        List<Node> lst = new List<Node>();

        int xValue = Mathf.Abs(minX - n.gridX);
        int yValue = Mathf.Abs(minY - n.gridY);

        if (xValue + 1 + yValue * tlmp.cellBounds.size.x < totalNodeNum)
            lst.Add(nodeMap[xValue + 1 + yValue * tlmp.cellBounds.size.x]);
        if (xValue - 1 + yValue * tlmp.cellBounds.size.x > 0)
            lst.Add(nodeMap[xValue - 1 + yValue * tlmp.cellBounds.size.x]);
        if (xValue + (yValue + 1) * tlmp.cellBounds.size.x < totalNodeNum)
            lst.Add(nodeMap[xValue + (yValue + 1) * tlmp.cellBounds.size.x]);
        if (xValue+ (yValue - 1) * tlmp.cellBounds.size.x > 0)
            lst.Add(nodeMap[xValue + (yValue - 1) * tlmp.cellBounds.size.x]);
        return lst.ToArray();
    }


    //Takes in a world position, and returns it's respective Node.
    public Node GetNodeFromPos(Vector3 pos)
    {
        Vector3Int ps = tlmp.WorldToCell(pos);
        //Vector3Int ps = Vector3Int.FloorToInt(pos);
        int xValue = Mathf.Abs(minX - ps.x);
        int yValue = Mathf.Abs(minY - ps.y);
        
        return nodeMap[xValue + yValue * tlmp.cellBounds.size.x];
    }

    //Returns the position of a tile in the world, given another world position.
    public Vector3 GetTilePos(Vector3 pos)
    {
        return tlmp.CellToWorld(tlmp.WorldToCell(pos));
    }

    //Convert a Node to a world position.
    public Vector3 NodeToWorld(Node n)
    {
        Vector3 tmp =  tlmp.CellToWorld(new Vector3Int(n.gridX, n.gridY, 0));
        tmp.x += (tlmp.cellSize.x / 2);
        tmp.y = (tlmp.cellSize.y / 2);
        tmp.z += (tlmp.cellSize.y / 2);
        return tmp;
    }

    public bool IsWalkable(Vector3 pos)
    {
        TileBase x = tlmp.GetTile(tlmp.WorldToCell(pos));
      
        foreach (Tile y in unwalkable)
        {
            if(y==x)
            {
                return false;
            }
        }
        //Only enemies can walk on these tiles, not friends.
        if (x == enemyWalkTile)
            return false;
        return true;
    }

    //Disables a door at a positon. When it does this, we need to recalculate the Nodempa.
    public void DisableDoor(Vector3 pos)
    {
        tlmp.SetTile(tlmp.WorldToCell(pos),
            openedDoor);
        RecalculateNodemap();
    }

    //This will prob. be changed to "Move uninteractable", and will take in a tile. But for now, it just moves an enemy.
    public void MoveEnemy(Vector3 prevPos, Vector3 newPos)
    {
        tlmp.SetTile(tlmp.WorldToCell(prevPos), defaultTile);
        tlmp.SetTile(tlmp.WorldToCell(newPos), enemyTile);
        RecalculateNodemap();
    }

    //Set the given tile to the "Walk Enemy" Tile. If prevPos == newPos, it just sets the given position to the default tile..
    public void WalkEnemy(Vector3 prevPos, Vector3 newPos)
    {
        tlmp.SetTile(tlmp.WorldToCell(newPos), enemyWalkTile);
        tlmp.SetTile(tlmp.WorldToCell(prevPos), defaultTile);
        RecalculateNodemap();
    }
}
