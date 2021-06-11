using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private DirtBlock[][] _grid;
    // Start is called before the first frame update

    int _gridXSize;
    int _gridYSize;
 
    

    public Grid(int gridXSize, int gridYSize)
    {
        this._gridXSize = gridXSize;
        this._gridYSize = gridYSize;
        _grid = new DirtBlock[gridYSize][];
    }
    public void AddBlock(DirtBlock dirtBlock, int x, int y)
    {
        if(y >= _grid.Length)
        {
            throw new System.Exception($"Cannot add to a layer that is greater then the max layer {y} >= {_grid.Length}");
        }

        if (_grid[y] == null)
        {
            _grid[y] = new DirtBlock[_gridXSize];
        }

        if (x >= _grid[y].Length)
        {
            throw new System.Exception($"Cannot add to a layer that is greater then the max layer {x} >= {_grid[y].Length}");
        }
        
        _grid[y][x] = dirtBlock;
      

    }

    /// <summary>
    /// If x or y is out of bounds, returns null
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public DirtBlock GetBlock(int x, int y)
    {
        if(x< 0 || x >= _grid.Length || y >= _grid[x].Length || y < 0)
        {
            return null;
        }

        return _grid[y][x];
    }
}
