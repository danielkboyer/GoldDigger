using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirtBlock : MonoBehaviour
{
    public bool _isAir;

    private bool _isDug;

    private int _x;
    private int _y;

    private List<BreadCrumb> _breadCrumbs = new List<BreadCrumb>();

    public bool _isGold;
    public int GoldAmount;


    public GameObject BreadCrumbPrefab;

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
        int x = 0;
        while (x < _breadCrumbs.Count)
        {
            if (!_breadCrumbs[x].IsAlive(Time.deltaTime))
            {
                Destroy(_breadCrumbs[x].gameObject);
                _breadCrumbs.RemoveAt(x);
                continue;
            }

            x++;
        }
    }

    public void DecrementGoldAmount()
    {
        this.GoldAmount -= 1;
        UnityEngine.Debug.Log(this.GoldAmount);
        if (this.GoldAmount <= 0)
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this._isDug = true;

        }
    }

    public float GetOldestGold(int baseId)
    {
        if (_breadCrumbs.Where(t => t.GoldCrumb && t.Id.Split(' ')[0] == baseId.ToString()).Count() <= 0)
            return 0;
        return _breadCrumbs.Where(t=>t.GoldCrumb && t.Id.Split(' ')[0] == baseId.ToString()).Max(t => t.GetAge());
    }

    public float GetOldestHome(int baseId)
    {
        if (_breadCrumbs.Where(t => t.HomeCrumb && t.Id.Split(' ')[0] == baseId.ToString()).Count() <= 0)
            return 0;
        return _breadCrumbs.Where(t => t.HomeCrumb && t.Id.Split(' ')[0] == baseId.ToString()).Max(t => t.GetAge());
    }
    public void AddGoldCrumb(string agentId)
    {
        if (_breadCrumbs.Any(t => t.Id == agentId && t.GoldCrumb))
            return;
        var xPos = Random.Range(_x - transform.localScale.x / 2, _x + transform.localScale.x / 2);
        var yPos = Random.Range(_y - transform.localScale.y / 2, _y + transform.localScale.y / 2);
        var goldCrumb = Instantiate(BreadCrumbPrefab, new Vector2(xPos, yPos), Quaternion.identity).GetComponent<BreadCrumb>();
        goldCrumb.Init(false, true,agentId);
        _breadCrumbs.Add(goldCrumb);
    }

    public void AddHomeCrumb(string agentId)
    {
        if (_breadCrumbs.Any(t => t.Id == agentId && t.HomeCrumb))
            return;
        var xPos = Random.Range(_x - transform.localScale.x / 2, _x + transform.localScale.x / 2);
        var yPos = Random.Range(_y - transform.localScale.y / 2, _y + transform.localScale.y / 2);
        var breadCrumb = Instantiate(BreadCrumbPrefab, new Vector2(xPos, yPos), Quaternion.identity).GetComponent<BreadCrumb>();
        breadCrumb.Init(true, false,agentId);
        _breadCrumbs.Add(breadCrumb);
    }
    public void SetIsAir(bool air)
    {
        this._isAir = air;
    }
}


