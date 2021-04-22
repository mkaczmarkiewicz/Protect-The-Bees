using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScript : MonoBehaviour
{   
    // map //

    [SerializeField]
    public Tilemap foregroundTilemap;
    [SerializeField]
    public RuleTile patch;

    // tiles data //

    const string ROCK = "Rock";
    const string WEED = "Weed";

    int mapSizeX = 15;
    int mapSizeY = 6;
    Vector3Int ll = new Vector3Int(-8, -2, 0); //lower left corner of map
    Vector3Int ur = new Vector3Int(5, 2, 0); //upper right corner of map
    int offsetX; //offsets are needed, because our map in game has coordinates between ll & ur,
    int offsetY; //while our array has coordinates between [0][0] & [mapSizeX-1][mapSizeY-1]
    GameObject[,] tilesData;

    // obstacles //
    [SerializeField]
    public Object rock;
    [SerializeField]
    public Object weed;

    // plants //

    [SerializeField]
    public Object currentPlant;
    [SerializeField]
    public Object strawberry;
    [SerializeField]
    public Object garlic;

    // tools //

    bool hoeActive;
    bool dynamiteActive;
    bool pesticidesActive;

    // bees //
    [SerializeField] 
    public GameObject hive;

    private void Start()
    {
        // plants //
        currentPlant = null;
        // tiles data //
        tilesData = new GameObject[mapSizeX, mapSizeY];
        offsetX = ll.x * -1;
        offsetY = ll.y * -1;
        FillTilesData();

        // tools //
        hoeActive = false;
        dynamiteActive = false;
        pesticidesActive = false;

        //testing //
        SpawnSomeStonesAndWeed();
    }

    void Update()
    {
        HoeTile();
        Plant();
        DestroyRock();
        DestroyWeed();
    }

    void HoeTile() // For tilling with hoe
    {
        if (Input.GetMouseButtonDown(0) && hoeActive) //if mouse pressed and our hoe i active
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // getting mouse position
            Vector3Int gridPosition = foregroundTilemap.WorldToCell(mousePosition); // getting position of tile the mouse is pointing           
            
            /////debugging/////
            if (foregroundTilemap.GetTile(gridPosition) is RuleTile) // checks if tile is patch (patch is the only RuleTile in scene)
            {
                Debug.Log("Tu nie wolno kopaæ!");
            }
            ///////////////////

            if (TileStatus(gridPosition) == null)
            {
                foregroundTilemap.SetTile(gridPosition, patch); // puts patch on tile
            }
        }
    }

    void Plant()
    {
        if (currentPlant != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //getting mouse position               
                Vector3Int gridPosition = foregroundTilemap.WorldToCell(mousePosition); // getting position of tile the mouse is pointing 
               
                Vector2 objectSpawnPosition = new Vector2(gridPosition.x + 0.5f, gridPosition.y + 0.5f); //getting centre of tile where the plant will be spawned

                if (foregroundTilemap.GetTile(gridPosition) is RuleTile && TileStatus(gridPosition) == null) //checks if tile is patch (patch is the only RuleTile in scene)
                {
                    Object newPlant = Instantiate(currentPlant, objectSpawnPosition, Quaternion.identity); // instantiates plant in given place
                    SetTileAsOccupiedByPlant(gridPosition, newPlant);
                    Debug.Log(objectSpawnPosition.x+" "+objectSpawnPosition.y);
                    Debug.Log(gridPosition.x+" "+gridPosition.y);
                }              
            }
        }
    }

    void DestroyRock()
    {
        if (Input.GetMouseButtonDown(0) && dynamiteActive) //if mouse pressed and our hoe i active
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // getting mouse position
            Vector3Int gridPosition = foregroundTilemap.WorldToCell(mousePosition); // getting position of tile the mouse is pointing                      
            if (TileStatus(gridPosition) != null)
            {
                if (TileStatus(gridPosition).tag == ROCK)
                {
                    GameObject tmp = TileStatus(gridPosition);
                    SetTileAsAvailable(gridPosition);
                    Destroy(tmp.gameObject);
                    MakeBeesAngry();
                }
            }
        }
    }

    void DestroyWeed()
    {
        if (Input.GetMouseButtonDown(0) && pesticidesActive) //if mouse pressed and our hoe i active
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // getting mouse position
            Vector3Int gridPosition = foregroundTilemap.WorldToCell(mousePosition); // getting position of tile the mouse is pointing                      
            if (TileStatus(gridPosition) != null)
            {
                if (TileStatus(gridPosition).tag == WEED)
                {
                    GameObject tmp = TileStatus(gridPosition);
                    SetTileAsAvailable(gridPosition);
                    Destroy(tmp.gameObject);
                    MakeBeesSick();
                }
            }
        }
    }

    void FillTilesData() //sets all tiles to available
    {
        for (int x = ll.x; x <= ur.x; x++)
            for (int y = ll.y; y <= ur.y; y++)
            {
                tilesData[x + offsetX, y + offsetY] = null;
            }
    }

    void SetTileAsOccupiedByPlant(Vector3Int gridPosition, Object plant)
    {
        tilesData[gridPosition.x +offsetX, gridPosition.y + offsetY] = (GameObject) plant;
    }

    void SetTileAsOccupiedByRock(Vector3Int gridPosition, Object rock)
    {
        tilesData[gridPosition.x + offsetX, gridPosition.y + offsetY] = (GameObject) rock;
    }

    void SetTileAsOccupiedByWeed(Vector3Int gridPosition, Object weed)
    {
        tilesData[gridPosition.x + offsetX, gridPosition.y + offsetY] = (GameObject) weed;
    }

    void SetTileAsAvailable(Vector3Int gridPosition)
    {
        tilesData[gridPosition.x + offsetX, gridPosition.y + offsetY] = null;
    }

    GameObject TileStatus(Vector3Int gridPosition)
    {
        return tilesData[gridPosition.x + offsetX, gridPosition.y + offsetY];
    }

    public void HoeActivation()
    {
        hoeActive = true;
        currentPlant = null;
    }

    public void HoeDeactivation()
    {
        hoeActive = false;
    }

    public void DynamiteActivation()
    {
        dynamiteActive = true;
    }

    public void DynamiteDeactivation()
    {
        dynamiteActive = false;
    }

    public void PesticidesActivation()
    {
        pesticidesActive = true;
    }

    public void PesticidesDeactivation()
    {
        pesticidesActive = false;
    }

    public void ChooseStrawberry()
    {
        currentPlant = strawberry;
        HoeDeactivation();
    }

    public void ChooseGarlic()
    {
        currentPlant = garlic;
        HoeDeactivation();
    }

    void SpawnRock(Vector3Int gridPosition)
    {
        Object newRock = Instantiate(rock, new Vector2(gridPosition.x + 0.5f, gridPosition.y + 0.5f), Quaternion.identity);
        SetTileAsOccupiedByRock(gridPosition, newRock);
    }

    void SpawnWeed(Vector3Int gridPosition)
    {
        Object newWeed = Instantiate(weed, new Vector2(gridPosition.x + 0.5f, gridPosition.y + 0.5f), Quaternion.identity);
        SetTileAsOccupiedByWeed(gridPosition, newWeed);
    }

    // Method for testing //
    void SpawnSomeStonesAndWeed()
    {
        SpawnRock(new Vector3Int(-6, -1, 0));
        SpawnWeed(new Vector3Int(-3, 0, 0));
    }

    void MakeBeesAngry()
    {
        for (int i=0; i<hive.transform.childCount; i++)
            hive.transform.GetChild(i).GetChild(0).GetComponent<Bee>().ChangeStatusToAngry();       
    }

    void MakeBeesSick()
    {
        for (int i = 0; i < hive.transform.childCount; i++)
            hive.transform.GetChild(i).GetChild(0).GetComponent<Bee>().ChangeStatusToSick();
    }
}

// TO DO //
//
// 1) It is possible to plant multiple plants on the same tile. It shouldnt be possible XD