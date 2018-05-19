using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class EditTilemap : MonoBehaviour {

    public TileManager tm;
    public Tile t;
    public CreateObjects co;

    private float MaxDistance = 100000;
    int layermask;

	// Use this for initialization
	void Start () {
        layermask = 1 << LayerMask.NameToLayer("Tilemap");
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tmp = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, MaxDistance, layermask))
            {
                print(hit.point);
                print(tm.tlmp.WorldToCell(hit.point));
                tm.tlmp.SetTile(tm.tlmp.WorldToCell(hit.point), t);
                tm.RecalculateNodemap();
                co.CreateWalls();
                print("WOW!");
                
            }

            //tm.tlmp.SetTile(tm.tlmp.WorldToCell(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), t);
            //tm.RecalculateNodemap();
            print("wow");
        }
	}
}
