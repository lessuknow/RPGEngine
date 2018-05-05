using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour {

    //The floor's tilemap.
    public Tilemap tlmp;
    public Tile openedDoor;
    [SerializeField] Tile[] unwalkable;

    //This converts a world position into it's tile position, and back into a world position.
    //Basically, this "centers" the given world position onto a tile.
    public Vector3 GetTilePos(Vector3 pos)
    {
        return tlmp.CellToWorld(tlmp.WorldToCell(pos));
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
        return true;
    }

    public void DisableDoor(Vector3 pos)
    {
        tlmp.SetTile(tlmp.WorldToCell(pos),
            openedDoor);
    }
}
