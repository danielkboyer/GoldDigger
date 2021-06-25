using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour,IMap
{

    public int _totalGold;

    private int _currentDiggers = 0;

    private BaseSetting _settings;

    private float _currentTime;

    private IMap _map;

    private GameObject _diggerPrefab;

    private bool _isInit = false;

    private int _id;


    private int currentId = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(IMap map,GameObject diggerPrefab, BaseSetting baseSetting, int id)
    {
        _isInit = true;
        this._map = map;
        this._diggerPrefab = diggerPrefab;
        this._settings = baseSetting;

        this._settings.DiggerSetting.Position = this._settings.coord;

        this._id = id;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isInit)
            return;
        _currentTime += Time.deltaTime;

        if(_currentTime >= _settings.SpawnRate && _currentDiggers < _settings.TotalDiggers)
        {
            _currentTime -= _settings.SpawnRate;
            SpawnDigger();
            
        }
    }


    void SpawnDigger()
    {
        var digger = Instantiate(_diggerPrefab, new Vector2(_settings.coord.x, _settings.coord.y), Quaternion.identity).GetComponent<Digger>();
        digger.Init(_map,_settings.DiggerSetting,_id,currentId++, this._settings.coord.x, this._settings.coord.y);
        _currentDiggers++;
    }

    public DirtBlock GetBlock(int x, int y)
    {
        if (!_isInit)
            throw new System.Exception("Must Init base first");

        return _map.GetBlock(x, y);
    }
}
