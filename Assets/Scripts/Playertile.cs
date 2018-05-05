using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class Playertile : Tile
{  

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/Playertile")]
    public static void dCreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Playertile", "New Playertile", "asset", "Save Playertile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<Playertile>(), path);
    }
#endif
}

