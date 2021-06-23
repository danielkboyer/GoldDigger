using Assets.Scripts;
using System;
using UnityEngine;

public class Digger : MonoBehaviour
{


    private DiggerSettings _diggerSettings;

    public Move MoveTowards;
    /// <summary>
    /// the current time in seconds the agent is at before the agent moves again:
    /// </summary>
    private float _currentTime = 0;
    /// <summary>
    /// the time at which the agent will move in seconds
    /// </summary>
    private const float _moveTime = .25f;

    private float _moveInterval = .05f;

    private int _currentStep = 0;


    IMap _map;
    bool isInit;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init(IMap map, DiggerSettings start)
    {
        this._diggerSettings = new DiggerSettings() {DigTime = start.DigTime,SightDistance = start.SightDistance,Position = new Coord() { x = start.Position.x, y = start.Position.y },Speed = start.Speed,mentality = new Mentality() { ProbabilityOfFight = start.mentality.ProbabilityOfFight } };
        this._map = map;
        isInit = true;
    }
    /// <summary>
    /// called when the agent reaches their destination and they need to choose where to move next
    /// </summary>
    void ChooseMove(DirtBlock[] up, DirtBlock[] down, DirtBlock[] right, DirtBlock[] left, DirtBlock on)
    {

    }
    /// <summary>
    /// Called when the agent should move
    /// </summary>
    private void MoveAgent()
    {
        _currentStep++;
        if (MoveTowards == Move.LEFT)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - _moveInterval, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (MoveTowards == Move.DOWN)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - _moveInterval, gameObject.transform.position.z);
        }
        else if (MoveTowards == Move.UP)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + _moveInterval, gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + _moveInterval, gameObject.transform.position.y, gameObject.transform.position.z);

        }

        if (_currentStep == 20)
        {
            //update the players position
            if (MoveTowards == Move.LEFT)
            {
                _diggerSettings.Position.x -= 1;
            }
            else if (MoveTowards == Move.DOWN)
            {
                _diggerSettings.Position.y += 1;
            }
            else if (MoveTowards == Move.UP)
            {
                _diggerSettings.Position.y -= 1;
            }
            else
            {
                _diggerSettings.Position.x += 1;
            }
            _currentStep = 0;

            DirtBlock[] upBlocks = new DirtBlock[_diggerSettings.SightDistance];
            DirtBlock[] downBlocks = new DirtBlock[_diggerSettings.SightDistance];
            DirtBlock[] leftBlocks = new DirtBlock[_diggerSettings.SightDistance];
            DirtBlock[] rightBlocks = new DirtBlock[_diggerSettings.SightDistance];
            for (int x = 0; x < _diggerSettings.SightDistance; x++)
            {
                upBlocks[x] = _map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y - 1 - x);
                downBlocks[x] = _map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y + 1 + x);
                leftBlocks[x] = _map.GetBlock(_diggerSettings.Position.x - 1 - x, _diggerSettings.Position.y);
                rightBlocks[x] = _map.GetBlock(_diggerSettings.Position.x + 1 + x, _diggerSettings.Position.y);
            }

            ChooseMove(upBlocks, downBlocks, rightBlocks, leftBlocks, _map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y));
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var diggerOther = collision.gameObject.GetComponent<Digger>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isInit)
            return;



        _currentTime += Time.deltaTime;

        if (_currentTime * _diggerSettings.Speed >= _moveTime)
        {
            _currentTime = 0;
            MoveAgent();

        }
    }
}

public enum Move
{
    LEFT,
    DOWN,
    RIGHT,
    UP
}

[Serializable]
public struct DiggerSettings
{
    public Coord Position;
    public float Speed;

    public float DigTime;

    public int SightDistance;

    public Mentality mentality;
}

[Serializable]
public struct Mentality
{
    public int ProbabilityOfFight;
}