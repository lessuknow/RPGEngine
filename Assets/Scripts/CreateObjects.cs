﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateObjects : MonoBehaviour {

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Tilemap tlmp;
    [SerializeField]
    private TileManager tm;
    
    //playertile, walltile, doortile
    [SerializeField]
    private Tile playerTile, wallTile, doorTile, chestTile, enemyTile, shopTile;

    [SerializeField]
    private AStar astar;

    [SerializeField]
    private GameObject door, chest, enemy, shop;

    [SerializeField] private UnitManager um;
    [SerializeField] private UIHandler uih;
    [SerializeField] private Material wallMat;
    [SerializeField] private ShopUI sui;

    private int vertNum = 0;
    private List<Vector3> verts;

    GameObject mesh;

    //There has to be a better way to do this...
    //There is a better way; doing it rn.

    // Use this for initialization
    void Start () {

        CreateWalls();

        Vector3Int orgn = tlmp.origin;
        Vector3Int sz = tlmp.size;

        //Spawn the player real fast
        GameObject plr = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        Camera cam = plr.GetComponentInChildren<Camera>();
        PlayerControls pc = plr.GetComponent<PlayerControls>();

        bool PlacedPlayer = false;
        for (int x = orgn.x; x < orgn.x + sz.x; x++)
        {
            for (int y = orgn.y; y < orgn.y + sz.y; y++)
            {
                Vector3Int a = new Vector3Int(x, y, orgn.z);

                if (tlmp.GetTile(a) == playerTile)
                    {
                        Vector3 pos = tlmp.CellToWorld(a);

                        pos.x += tlmp.cellSize.x / 2;
                        pos.z += tlmp.cellSize.y / 2;
                        pos.y += tlmp.cellSize.y / 2;

                        print("Spawned player");

                        plr.transform.position = pos;
                        plr.transform.rotation = Quaternion.Euler(0, 180, 0);

                        //Should probably change this stuff to be done inside the prefab instead lol
                        plr.GetComponent<PlayerControls>().curDir = PlayerControls.Direction.North;
                        plr.GetComponent<PlayerControls>().tm = tm;
                        plr.GetComponent<PlayerControls>().uih = uih;
                        plr.GetComponent<PlayerControls>().um = um;
                        um.pC = plr.GetComponent<PlayerControls>();
                        PlacedPlayer = true;
                    }
                    if (tlmp.GetTile(a) == doorTile)
                    {
                        Vector3 pos = tlmp.CellToWorld(a);
                        pos.x += tlmp.cellSize.x / 2;
                        pos.z += tlmp.cellSize.y / 2;
                        pos.y += tlmp.cellSize.y / 2;

                        print("Spawned door");

                        GameObject obj = Instantiate(door, pos, Quaternion.identity);

                        ((doorTile)tlmp.GetTile(a)).door = obj;
                        (obj.GetComponent<DoorScript>()).tm = tm;

                    }
                    if (tlmp.GetTile(a) == chestTile)
                    {
                        Vector3 pos = tlmp.CellToWorld(a);
                        pos.x += tlmp.cellSize.x / 2;
                        pos.z += tlmp.cellSize.y / 2;
                        pos.y += tlmp.cellSize.y / 2;

                        print("Spawned chest");

                        GameObject obj = Instantiate(chest, pos, Quaternion.identity);

                        obj.GetComponent<ChestScript>().um = um;
                        obj.GetComponent<ChestScript>().itm = new Skill_Base("Revive", true);
                        //((Chesttile)tlmp.GetTile(a)).door = obj;

                    }
                    if (tlmp.GetTile(a) == enemyTile)
                    {
                        Vector3 pos = tlmp.CellToWorld(a);
                        pos.x += tlmp.cellSize.x / 2;
                        pos.z += tlmp.cellSize.y / 2;
                        pos.y += tlmp.cellSize.y / 2;

                        print("Spawned enemy");

                        GameObject obj = Instantiate(enemy, pos, Quaternion.identity);

                        obj.GetComponent<Mob_Movement>().target = plr;
                        obj.GetComponent<Mob_Movement>().astar = astar;
                        obj.GetComponent<Mob_Movement>().tm = tm;
                        obj.GetComponentInChildren<FaceCameraUI>().m_Camera = cam;
                        print(obj.GetComponent<Mob_Base>());
                        um.AddEnemy(obj.GetComponent<Mob_Base>());
                        print(um);

                    }
                    if (tlmp.GetTile(a) == shopTile)
                    {
                        Vector3 pos = tlmp.CellToWorld(a);
                        pos.x += tlmp.cellSize.x / 2;
                        pos.z += tlmp.cellSize.y / 2;
                        pos.y += tlmp.cellSize.y / 2;

                        print("Spawned shopkeeper");

                        GameObject obj = Instantiate(shop, pos, Quaternion.identity);
                        obj.GetComponent<Shop_NPC>().um = um;
                        obj.GetComponent<Shop_NPC>().sui = sui;
                        obj.GetComponent<Shop_NPC>().pc = pc;
                        obj.GetComponentInChildren<FaceCameraUI>().m_Camera = cam;


                }
            }
        }
        if (!PlacedPlayer)
        {
            Destroy(plr);
        }

    }

    public void CreateWalls()
    {

        vertNum = 0;

        Vector3Int orgn = tlmp.origin;
        Vector3Int sz = tlmp.size;

        verts = new List<Vector3>();
        Destroy(mesh);
        mesh = new GameObject("Plane");


        mesh.AddComponent<MeshFilter>();
        mesh.AddComponent<MeshRenderer>();



        for (int x = orgn.x; x < orgn.x + sz.x; x++)
        {
            for (int y = orgn.y; y < orgn.y + sz.y; y++)
            {
                Vector3Int a = new Vector3Int(x, y, orgn.z);

                //This elif statement 
                if (tlmp.GetTile(a) == wallTile)
                {
                    AddVerts(x, y);
                }

            }
        }

        mesh.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        mesh.GetComponent<MeshRenderer>().material = wallMat;
        //After we spawn the objects/add the verts, work on the rest of the sutff.
        mesh.GetComponent<MeshFilter>().mesh.vertices = verts.ToArray();
        MakeTri();
        mesh.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        mesh.GetComponent<MeshCollider>();
    }

    private void MakeTri()
    {
        //int[] tri = new int[vertNum];
        List<int> triangles = new List<int>();
        //Front Face

        //TODO: Math, so we aren't unness. rendering things.
        for(int i=0;i<vertNum;i+=8)
        {
            
            //Render the left side
            triangles.Add(i);
            triangles.Add(i + 7);
            triangles.Add(i + 3);
            triangles.Add(i);
            triangles.Add(i + 4);
            triangles.Add(i + 7);

            //Render the right side
            triangles.Add(i + 2);
            triangles.Add(i + 6);
            triangles.Add(i + 1);
            triangles.Add(i + 6);
            triangles.Add(i + 5);
            triangles.Add(i + 1);

            //Render the front side
            triangles.Add(i + 3);
            triangles.Add(i + 2);
            triangles.Add(i + 0);
            triangles.Add(i + 2);
            triangles.Add(i + 1);
            triangles.Add(i + 0);
            
            //Render the back side
            triangles.Add(i + 6);
            triangles.Add(i + 7);
            triangles.Add(i + 4);
            triangles.Add(i + 5);
            triangles.Add(i + 6);
            triangles.Add(i + 4);
            
            //Render the top side
            triangles.Add(i + 3);
            triangles.Add(i + 7);
            triangles.Add(i + 6);
            triangles.Add(i + 3);
            triangles.Add(i + 6);
            triangles.Add(i + 2);
            
        }

        mesh.GetComponent<MeshFilter>().mesh.triangles = triangles.ToArray();
    }

    private void AddVerts(int xCoord, int zCoord)
    {
        float x = tlmp.cellSize.x / 2;
        float y = tlmp.cellSize.y / 2;
        float z = tlmp.cellSize.y / 2;

        y *= 2;

        float xMod = xCoord * tlmp.cellSize.x;
        float zMod = zCoord * tlmp.cellSize.y;

        verts.Add(new Vector3(0 + xMod, 0 , 0 + zMod));
        verts.Add(new Vector3(2 * x + xMod, 0, 0 + zMod));
        verts.Add(new Vector3(2 * x + xMod, 2 * y, 0 + zMod));
        verts.Add(new Vector3(0 + xMod, 2 * y, 0 + zMod));

        verts.Add(new Vector3(0 + xMod, 0, 2 * z + zMod));
        verts.Add(new Vector3(2 * x + xMod, 0, 2 * z + zMod));
        verts.Add(new Vector3(2 * x + xMod, 2 * y, 2 * z + zMod));
        verts.Add(new Vector3(0 + xMod, 2 * y, 2 * z + zMod));

        vertNum += 8;
    }

}
