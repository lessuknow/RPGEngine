using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTileData : MonoBehaviour {
    public Dictionary<Vector2, CustomTile> custom_tiles;

    public void NewCustomTile(Vector2 pos)
    {
        custom_tiles.Add(pos, new CustomTile());
    }

}
