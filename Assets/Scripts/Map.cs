using UnityEngine;

public class Map : MonoBehaviour
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
