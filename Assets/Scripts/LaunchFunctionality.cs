using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchFunctionality
{
    private Vector2 mouseStartPosition, mouseEndPosition, tempVelocity;
    private float ballVelocityX, ballVelocityY;
    private bool hasLaunchedMainBallInCurrentTurn;
	
    public Vector2 MouseStartPosition{ get { return mouseStartPosition; } set { mouseStartPosition = value; } }
	public Vector2 MouseEndPosition { get { return mouseEndPosition; } set { mouseEndPosition = value; } }

    public LaunchFunctionality()
    {
        ResetValues();
    }

    public void Launch(BallIdentifier ball)
    {
        if(!hasLaunchedMainBallInCurrentTurn)
        {
			ballVelocityX = mouseStartPosition.x - mouseEndPosition.x;
			ballVelocityY = mouseStartPosition.y - mouseEndPosition.y;
			
			tempVelocity = new Vector2(ballVelocityX, ballVelocityY).normalized;

            LaunchBall(ball);

            hasLaunchedMainBallInCurrentTurn = true;
        }
        else
        {
            LaunchBall(ball);
        }

    }

    private void LaunchBall(BallIdentifier ball)
    {
        ball.rigidbody.velocity = tempVelocity * GameManager.Instance.gameInfo.ballConstantSpeed;

        if (ball.rigidbody.velocity == Vector2.zero)
        {
            Debug.Log("uwudjnd");
            GameManager.Instance.AllBallsLanded();
            return;
        } 
    }

    public void ResetValues()
    {
        //Debug.Log("Launch Reset values");

        MouseStartPosition = Vector2.zero;
        MouseEndPosition = Vector2.zero;
        tempVelocity = Vector2.zero;
        ballVelocityX = 0.0f;
        ballVelocityY = 0.0f;
        hasLaunchedMainBallInCurrentTurn = false;
    }
}
