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

    /// <summary>
    /// Time left to stop and dig
    /// </summary>
    private float DigTime = 0;

    /// <summary>
    /// Penalty for digging
    /// </summary>
    private float DigPenalty;

    private int _baseID;

    IMap _map;
    bool isInit;

    private bool _hasGold;

    private bool _isFighting;

    private bool _isStunned;

    private float _stunTime;

    private string _mentality;
    // copycat   - cooperate then copy other player moves
    // cooperate - always cooperate
    // cheat     - always cheat
    // grudger   - cooperate, if cheated, then always cheat
    // random    - sir rando

    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init(IMap map, DiggerSettings start, int baseID)
    {
        this._diggerSettings = new DiggerSettings() {
            DigPenalty = start.DigPenalty,
            SightDistance = start.SightDistance,
            Position = new Coord() { x = start.Position.x, y = start.Position.y },
            Speed = start.Speed,
            mentality = new Mentality() { ProbabilityOfFight = start.mentality.ProbabilityOfFight },
            moveTowards = start.moveTowards
        };
        this.MoveTowards = _diggerSettings.moveTowards;
        this._map = map;
        isInit = true;
        this.DigPenalty = start.DigPenalty;
        this._baseID = baseID;
        this._mentality = "cooperative";
        this._isFighting = false;
        this._hasGold = false;
        this._isStunned = false;
        this._stunTime = 0;
    }
    /// <summary>
    /// called when the agent reaches their destination and they need to choose where to move next
    /// </summary>
    void ChooseMove(DirtBlock[] up, DirtBlock[] down, DirtBlock[] right, DirtBlock[] left, DirtBlock on)
    {
        System.Random random = new System.Random();
        double randomnumber = random.NextDouble();
        double upScore = random.NextDouble(); 
        double leftScore = random.NextDouble(); 
        double rightScore = random.NextDouble(); 
        double downScore = random.NextDouble();

        if (_map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y + 1) == null)
        {
            upScore = -double.MaxValue;
        }
        if (_map.GetBlock(_diggerSettings.Position.x - 1 , _diggerSettings.Position.y) == null)
        {
            leftScore = -double.MaxValue;
        }
        if (_map.GetBlock(_diggerSettings.Position.x + 1 , _diggerSettings.Position.y) == null)
        {
            rightScore = -double.MaxValue;
        }
        if (_map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y - 1) == null)
        {
            downScore = -double.MaxValue;
        }


        Move next = Move.DOWN;
        double maxScore = downScore;
        if (upScore > maxScore)
        {
            maxScore = upScore;
            next = Move.UP;
        }
        if (leftScore > maxScore)
        {
            maxScore = leftScore;
            next = Move.LEFT;
        } if (rightScore > maxScore)
        {
            maxScore = rightScore;
            next = Move.RIGHT;
        }

        MoveTowards = next;

        
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
            _currentStep = 0;

            //update the players position
            if (MoveTowards == Move.LEFT)
            {
                _diggerSettings.Position.x -= 1;
            }
            else if (MoveTowards == Move.DOWN)
            {
                _diggerSettings.Position.y -= 1;
            }
            else if (MoveTowards == Move.UP)
            {
                _diggerSettings.Position.y += 1;
            }
            else
            {
                _diggerSettings.Position.x += 1;
            }

            DirtBlock[] upBlocks = new DirtBlock[_diggerSettings.SightDistance];
            DirtBlock[] downBlocks = new DirtBlock[_diggerSettings.SightDistance];
            DirtBlock[] leftBlocks = new DirtBlock[_diggerSettings.SightDistance];
            DirtBlock[] rightBlocks = new DirtBlock[_diggerSettings.SightDistance];
            DirtBlock currentBlock = _map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y);

            //add crumbs
            if (_hasGold)
            {
                currentBlock.AddGoldCrumb();
            }
            else
            {
                currentBlock.AddHomeCrumb();
            }
            for (int x = 0; x < _diggerSettings.SightDistance; x++)
            {

                upBlocks[x] = _map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y - 1 - x);
                downBlocks[x] = _map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y + 1 + x);
                leftBlocks[x] = _map.GetBlock(_diggerSettings.Position.x - 1 - x, _diggerSettings.Position.y);
                rightBlocks[x] = _map.GetBlock(_diggerSettings.Position.x + 1 + x, _diggerSettings.Position.y);
            }
            
            ChooseMove(upBlocks, downBlocks, rightBlocks, leftBlocks, currentBlock);
            
            if (_map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y) != null && !_map.GetBlock(_diggerSettings.Position.x, _diggerSettings.Position.y)._isAir)
            {
                this.DigTime += this.DigPenalty;
                currentBlock.SetIsAir(true);
                currentBlock.gameObject.GetComponent<Renderer>().enabled = false;
            }
        }

    }
    private bool CheckTime()
    {
        if (this._stunTime % 60 < 2)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // digger will only fight another digger that is from another base
        // and only the digger from > baseID will initiate the funct call
        var diggerOther = collision.gameObject.GetComponent<Digger>();
        if (this._baseID > diggerOther._baseID)
        {
            Debug.Log("collision");
            this._isFighting = true;
            battles battle = new battles(this._mentality, diggerOther._mentality);
            if (battle.battle() == 0)
            {
                if (this._hasGold == false && diggerOther._hasGold == true)
                {
                    diggerOther._hasGold = false;
                    this._hasGold = true;
                }
                diggerOther._isStunned = true;
                diggerOther._stunTime = Time.deltaTime;
            }
            else
            {
                if (this._hasGold == true && diggerOther._hasGold == false)
                {
                    diggerOther._hasGold = true;
                    this._hasGold = false;
                }
                this._isStunned = true;
                this._stunTime = Time.deltaTime;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isInit)
            return;
        if (_isFighting)
            return;
        if (_isStunned)
        {
            if (CheckTime())
            {
                return;
            }
            else
            {
                this._isStunned = false;
            }
        }

        if (this.DigTime > 0)
        {
            this.DigTime -= Time.deltaTime;
        }
        else
        {
            _currentTime += Time.deltaTime;
            this.DigTime = 0;
            if (_currentTime * _diggerSettings.Speed >= _moveTime)
            {
                _currentTime = 0;
                MoveAgent();

            }
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


    public float DigPenalty;

    public int SightDistance;

    public Mentality mentality;

    public Move moveTowards;


}

[Serializable]
public struct Mentality
{
    public int ProbabilityOfFight;
}