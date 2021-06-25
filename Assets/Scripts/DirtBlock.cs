using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : MonoBehaviour
{
    public bool _isAir;

    private bool _isDug;

    private int _x;
    private int _y;

    public int HomeBreadCrumbs;
    public int GoldBreadCrumbs;

    public bool _isGold;
    public int GoldAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int x, int y, bool isAir, bool isDug, bool isGold, int goldAmount)
    {
        this._x = x;
        this._y = y;
        this._isAir = isAir;
        this._isDug = isDug;
        this._isGold = isGold;
        this.GoldAmount = goldAmount;

        if(this._isAir == true)
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }

        if(this._isGold == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetIsAir(bool air)
    {
        this._isAir = air;
    }
}
