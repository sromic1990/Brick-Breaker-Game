using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMovementController : MonoBehaviour 
{
    public enum BrickState
    {
        Stop,
        Move
    }

    public BrickState currentState;

	// Use this for initialization
	void Start () 
    {
        currentState = BrickState.Stop;
	}

    public void MoveDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(currentState == BrickState.Move)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            currentState = BrickState.Stop;
        }
	}
}
