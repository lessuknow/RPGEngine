using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class doorTile : Tile {

    public GameObject door;

    public void ToggleDoor()
    {
        door.GetComponent<DoorScript>().Toggle();
    }

    public bool GetWalkable()
    {
        return door.GetComponent<DoorScript>().getOpen();
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomTiles/Doortile")]
    public static void CreateDoorTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Door", "New Door", "asset", "Save Door", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<doorTile>(), path);
    }
#endif
}
