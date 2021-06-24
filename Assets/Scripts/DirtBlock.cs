using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : MonoBehaviour
{
    public bool _isAir;

    private bool _isDug;

    public int _x;
    public int _y;

    public int HomeBreadCrumbs;
    public int GoldBreadCrumbs;
    
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
    public void SetIsAir(bool air)
    {
        UnityEngine.Debug.Log(string.Format("changing my isair from {0}", this._isAir ));
        this._isAir = air;
    }
}
