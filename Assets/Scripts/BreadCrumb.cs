using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumb : MonoBehaviour
{
    public bool HomeCrumb;
    public bool GoldCrumb;

    public float AliveTime = 5;

    private float _secondsAlive = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(bool homeCrumb, bool goldCrumb)
    {
        this.HomeCrumb = homeCrumb;
        this.GoldCrumb = goldCrumb;

        if (this.HomeCrumb)
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
        }
        if (this.GoldCrumb)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    public bool IsAlive(float secondsElapsed)
    {
        _secondsAlive += secondsElapsed;
        if(_secondsAlive >= AliveTime)
        {
            return false;
        }

        return true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
