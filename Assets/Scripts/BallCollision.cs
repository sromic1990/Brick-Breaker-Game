using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallIdentifier))]
[RequireComponent(typeof(Rigidbody2D))]
public class BallCollision : MonoBehaviour 
{
    private Rigidbody2D rBody;
    private BallIdentifier ballID;

    void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        ballID = GetComponent<BallIdentifier>();
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Bricks"))
        {
            other.gameObject.GetComponent<BrickManager>().TakeDamage(GameManager.Instance.gameInfo.ballDamage);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //Ball has hit the gound.
            Debug.Log("Ball has hit the ground");
            //Stop the ball.
            rBody.velocity = Vector2.zero;
            //bookeeping to stop the ball from jumping up and stopping.
            //Step another level.
            //Set the ball as active.
            //GameManager.Instance.CurrentGameState = GameState.Wait;
            GameManager.Instance.BallLandedOnGround(transform.position.x);
            if(GameManager.Instance.FirstBallLanded)
            {
                Debug.Log("other balls landed");   
                TransformBallToFirstBallLandingPosition();
                return;
            }
			rBody.transform.position = new Vector2(transform.position.x, GameManager.Instance.gameInfo.ballRestPositionY);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PowerUp"))
        {
            GameManager.Instance.CollectedExtraBallPowerUp(other.GetComponent<ExtraBallPowerUp>());
        }
    }

    private void TransformBallToFirstBallLandingPosition()
    {
        //iTween.MoveTo(ballID.Me, iTween.Hash("position", new Vector3(GameManager.Instance.ballPositionX, GameManager.Instance.gameInfo.ballRestPositionY, 0), "time", GameManager.Instance.gameInfo.TimeForBallToGoToNextLaunchPosition, "oncomplete", "BallReachedNextLaunchPosition"));
        Debug.Log("Next Launch position = " + GameManager.Instance.ballPositionX+ " , " + GameManager.Instance.gameInfo.ballRestPositionY);
        this.transform.position = new Vector2(GameManager.Instance.ballPositionX, GameManager.Instance.gameInfo.ballRestPositionY);
        //BallReachedNextLaunchPosition();
    }

   // private void BallReachedNextLaunchPosition()
   // {
   //     if(gameObject.tag != "Ball")
   //     {
   //         Debug.Log("Returning pool balls");
			//GameManager.Instance.pooledExtraBalls.ReturnPooledObject(this.ballID);
    //    }
    //}

}
