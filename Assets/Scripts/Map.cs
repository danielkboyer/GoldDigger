using Assets.Scripts;
using System;
using UnityEngine;

public class Map : MonoBehaviour,IMap
{
    //private DirtBlock[][] _grid;
    /// <summary>
    /// 0,1,2,3,4 Y
    /// b,b,b,b,b
    /// b,b,b,b,b
    /// b,b,b,b,b
    /// </summary>
    
    private Grid _grid;

    public int GridSizeX;

    public int GridSizeY;

    /// <summary>
    /// How much air above the dirt
    /// the x matches the GridSizeX
    /// </summary>
    public int GridAirY;

    public GameObject DirtBlockGameObject;

    public GameObject MainCamera;

    /// <summary>
    /// the prefab used to instantiate bases
    /// </summary>
    public GameObject BasePrefab;

    /// <summary>
    /// the prefab used to instantate a digger
    /// </summary>
    public GameObject DiggerPrefab;

    /// <summary>
    /// an array of spawn coordinates, will create a new base per spawn coordinate. (set in inspector)
    /// </summary>
    public BaseSetting[] BaseSpawnSettings;
    // Start is called before the first frame update
    void Start()
    {
        _grid = new Grid(GridSizeX, GridSizeY);

        float spacingX = DirtBlockGameObject.transform.localScale.x;
        float spacingY = DirtBlockGameObject.transform.localScale.y;

        float currentX = 0;
        float currentY = 0;
        for (int x = 0; x < GridSizeX; x++)
        {
            
            for(int y = 0; y < GridSizeY; y++)
            {

                var gridBlockObject = Instantiate(DirtBlockGameObject, new Vector2(currentX, currentY),Quaternion.identity);
                var db = gridBlockObject.GetComponent<DirtBlock>();

                //check if this block should be air or not
                db.Init(x, y, y >= GridSizeY - GridAirY, false);

                _grid.AddBlock(db, x, y);

                currentY += spacingY;

            }
            currentY = 0;
            currentX += spacingX;
        }
        //set the camera to the middle of the grid
        float camX = spacingX * (GridSizeX / 2) - (GridSizeX % 2 == 0 ? spacingX / 2 : 0);
        float camY = spacingY * (GridSizeY / 2) - (GridSizeY % 2 == 0 ? spacingY / 2 : 0);
        MainCamera.transform.position = new Vector3(camX, camY, MainCamera.transform.position.z);

        //spawn all the bases 
        for(int x = 0; x < BaseSpawnSettings.Length; x++)
        {
            //Initialize the base
            var baseSpawned = Instantiate(BasePrefab, new Vector2(BaseSpawnSettings[x].coord.x, BaseSpawnSettings[x].coord.y), Quaternion.identity).GetComponent<Base>();
            //Call Init with map, diggerprefab, and base settings
            baseSpawned.Init(this, DiggerPrefab, BaseSpawnSettings[x]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public DirtBlock GetBlock(int x, int y)
    {
        return _grid.GetBlock(x, y);
    }


    
}

[Serializable]
public struct BaseSetting {
    public Coord coord;
    public int TotalDiggers;
    /// <summary>
    /// the time between spawning each agent (in seconds)
    /// </summary>
    public float SpawnRate;

    public DiggerSettings DiggerSetting;
}


[Serializable]
public struct Coord
{
    public int x;
    public int y;
}