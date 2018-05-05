using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class Walltile : Tile {

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/Walltile")]
    public static void dCreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Walltile", "New Walltile", "asset", "Save Walltile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Walltile>(), path);
    }
#endif
}
