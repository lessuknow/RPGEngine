using System.Collections;
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
    
    private List<Vector3> verts;
    private List<Vector2> uvs;
    private List<int> triangleVerts;

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
        

        Vector3Int orgn = tlmp.origin;
        Vector3Int sz = tlmp.size;

        verts = new List<Vector3>();
        triangleVerts = new List<int>();
        uvs = new List<Vector2>();
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
                    //AddVerts(x, y);
                }

            }
        }

        AddVerts(0, 0);
        AddVerts(1, 0);
        AddVerts(2, 0);
        AddVerts(3, 0);
        AddVerts(4, 0);
        AddVerts(5, 0);
        AddVerts(1, 1);
        AddVerts(1, 2);
        AddVerts(1, 3);
        AddVerts(1, 4);
        AddVerts(1, 5);
        //AddVerts(1, 1);
        //AddVerts(0, 1);

        //mesh.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        mesh.GetComponent<MeshRenderer>().material = wallMat;
        //After we spawn the objects/add the verts, work on the rest of the sutff.
        mesh.GetComponent<MeshFilter>().mesh.vertices = verts.ToArray();
        mesh.GetComponent<MeshFilter>().mesh.uv = uvs.ToArray();   
        MakeTri();
        mesh.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        mesh.GetComponent<MeshCollider>();

        

        for(int i=0;i<verts.Count;i++)
        {
            Vector3 tmp = verts[i];
            for(int j=0;j< verts.Count; j++)
            {
                if (tmp == verts[j] && i != j)
                { 
                    print("Dupe! "+i+" "+j+" "+verts[j] + " " + tmp);

                }
            }
        }

    }

    private void MakeTri()
    {
        //int[] tri = new int[vertNum];
        List<int> triangles = new List<int>();
        //Front Face

        //TODO: Math, so we aren't unness. rendering things.
        for(int i=0;i< triangleVerts.Count; i+=8)
        {
            
            //Render the left side
            triangles.Add(triangleVerts[i]);
            triangles.Add(triangleVerts[i + 7]);
            triangles.Add(triangleVerts[i + 3]);
            triangles.Add(triangleVerts[i]);
            triangles.Add(triangleVerts[i + 4]);
            triangles.Add(triangleVerts[i + 7]);
           
            //Render the right side
            triangles.Add(triangleVerts[i + 2]);
            triangles.Add(triangleVerts[i + 6]);
            triangles.Add(triangleVerts[i + 1]);
            triangles.Add(triangleVerts[i + 6]);
            triangles.Add(triangleVerts[i + 5]);
            triangles.Add(triangleVerts[i + 1]);
           
            //Render the front side
            triangles.Add(triangleVerts[i + 3]);
            triangles.Add(triangleVerts[i + 2]);
            triangles.Add(triangleVerts[i + 0]);
            triangles.Add(triangleVerts[i + 2]);
            triangles.Add(triangleVerts[i + 1]);
            triangles.Add(triangleVerts[i + 0]);
            
            //Render the back side
            triangles.Add(triangleVerts[i + 6]);
            triangles.Add(triangleVerts[i + 7]);
            triangles.Add(triangleVerts[i + 4]);
            triangles.Add(triangleVerts[i + 5]);
            triangles.Add(triangleVerts[i + 6]);
            triangles.Add(triangleVerts[i + 4]);
            
            //Render the top side
            triangles.Add(triangleVerts[i + 3]);
            triangles.Add(triangleVerts[i + 7]);
            triangles.Add(triangleVerts[i + 6]);
            triangles.Add(triangleVerts[i + 3]);
            triangles.Add(triangleVerts[i + 6]);
            triangles.Add(triangleVerts[i + 2]);
            

        }
        print(triangleVerts);
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
        bool flipX = false, flipY = false;
        //These if statements can and will need to be refactored...

        Vector3 tmp = new Vector3(0 + xMod, 0, 0 + zMod);
        if(VertsContains(tmp) != -1)
        {
            if(uvs[VertsContains(tmp)] == new Vector2(0,0))
            {
                flipX = true;
            }

           //print("Des "+uvs[VertsContains(tmp)]+" "+ VertsContains(tmp));
        }

        tmp = new Vector3(2 * x + xMod, 0, 0 + zMod);
        print(tmp);
        if (VertsContains(tmp) != -1)
        {
            if (uvs[VertsContains(tmp)] == new Vector2(0, 0))
            {
                flipY = true;
            }

            print("Dees " + uvs[VertsContains(tmp)] + " " + VertsContains(tmp));
        }

        tmp = new Vector3(0 + xMod, 0, 0 + zMod);
        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(tmp);
            uvs.Add(new Vector2(1, 0));
        }
        else
        {
            print("A");
            triangleVerts.Add(VertsContains(tmp));
        }
        tmp = new Vector3(2 * x + xMod, 0, 0 + zMod);
        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(tmp);
            if (!flipX)
                uvs.Add(new Vector2(0, 0));
            else
                uvs.Add(new Vector2(1, 0));
        }
        else
        {
            print("B");
            triangleVerts.Add(VertsContains(tmp));
        }
        tmp = new Vector3(2 * x + xMod, 2 * y, 0 + zMod);
        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(new Vector3(2 * x + xMod, 2 * y, 0 + zMod));
            if (!flipX)
            { 
            uvs.Add(new Vector2(0, 1));
            }
            else
            {
                print("OWO");
                uvs.Add(new Vector2(1,1));
            }
        }
        else
        {
            print("C");
            triangleVerts.Add(VertsContains(tmp));
        }
        tmp = new Vector3(0 + xMod, 2 * y, 0 + zMod);
        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(new Vector3(0 + xMod, 2 * y, 0 + zMod));
            if (!flipX)
                uvs.Add(new Vector2(1, 1));
            else
                uvs.Add(new Vector2(0, 0));
        }
        else
        {
            print("D");
            triangleVerts.Add(VertsContains(tmp));
        }

        tmp = new Vector3(0 + xMod, 0, 2 * z + zMod);
        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(new Vector3(0 + xMod, 0, 2 * z + zMod));
            if (!flipY)
                uvs.Add(new Vector2(0, 0));
            else
                uvs.Add(new Vector2(1, 0));
        }
        else
        {
            print('E');
            triangleVerts.Add(VertsContains(tmp));
        }
        tmp = new Vector3(2 * x + xMod, 0, 2 * z + zMod);
        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(new Vector3(2 * x + xMod, 0, 2 * z + zMod));
            if (flipY)
                uvs.Add(new Vector2(1, 0));
            else
                uvs.Add(new Vector2(0, 0));
        }
        else
        {
            print('F');
            triangleVerts.Add(VertsContains(tmp));
        }
        tmp = new Vector3(2 * x + xMod, 2 * y, 2 * z + zMod);

        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(new Vector3(2 * x + xMod, 2 * y, 2 * z + zMod));
            if (flipY)
                uvs.Add(new Vector2(1, 1));
            else
                uvs.Add(new Vector2(0, 1));
        }
        else
        {
            print("G");
           triangleVerts.Add(VertsContains(tmp));
        }
        tmp = new Vector3(0 + xMod, 2 * y, 2 * z + zMod);

        if (VertsContains(tmp) == -1)
        {
            triangleVerts.Add(verts.Count);
            verts.Add(new Vector3(0 + xMod, 2 * y, 2 * z + zMod));
            if (!flipY)
                uvs.Add(new Vector2(0, 1));
            else
                uvs.Add(new Vector2(1, 1));
        }
        else
        {
            print("H");
            triangleVerts.Add(VertsContains(tmp));
        }
        //Wait, how?? Sure. I'll learn the math later...

    }

    private int VertsContains(Vector3 tmp)
    {
        for (int i = 0; i < verts.Count; i++)
            if (verts[i] == tmp)
            {
                return i;
            }
        return -1;
        
    }

}
