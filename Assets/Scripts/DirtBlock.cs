using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : MonoBehaviour
{
    public bool _isAir;

    private bool _isDug;

    private int _x;
    private int _y;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int x, int y, bool isAir, bool isDug)
    {
        this._x = x;
        this._y = y;
        this._isAir = isAir;
        this._isDug = isDug;

        if(this._isAir == true)
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
