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

    // plants //

    [SerializeField]
    public Object currentPlant;
    [SerializeField]
    public Object strawberry;
    [SerializeField]
    public Object garlic;

    // tools //

    bool hoeActive;

    private void Start()
    {
        hoeActive = false;
        currentPlant = null;
    }

    void Update()
    {
        HoeTile();
        Plant();
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

            foregroundTilemap.SetTile(gridPosition, patch); // puts patch on tile
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

                if (foregroundTilemap.GetTile(gridPosition) is RuleTile) //checks if tile is patch (patch is the only RuleTile in scene)
                {
                    Instantiate(currentPlant, objectSpawnPosition, Quaternion.identity); // instantiates plant in given place
                    Debug.Log(objectSpawnPosition.x+" "+objectSpawnPosition.y);
                    Debug.Log(gridPosition.x+" "+gridPosition.y);
                }              
            }
        }
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
}

// TO DO //
//
// 1) It is possible to plant multiple plants on the same tile. It shouldnt be possible XD