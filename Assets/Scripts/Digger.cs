using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour
{

    public float Speed;

    public float DigTime;

    public int SightDistance;

    public (int X, int Y) GridPosition;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// called when the agent reaches their destination and they need to choose where to move next
    /// </summary>
    void ChooseMove()
    {

    }
    /// <summary>
    /// Called when the agent should move
    /// </summary>
    private void MoveAgent()
    {
        _currentStep++;
        if(MoveTowards == Move.LEFT)
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

        if(_currentStep == 20)
        {
            _currentStep = 0;
            ChooseMove();
        }

    }
    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;

        if(_currentTime * Speed >= _moveTime)
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