using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraBallManager : MonoBehaviour
{

    public static ExtraBallManager Instance;

    public float ballWaitTimeBeforeFiring;
    private float ballWaitTimeSecondsTicker;

    public int numberOfExtraBalls;
    public int numberOfExtraBallsRemainingToFire;

    public Text numberOfBallsText;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        ballWaitTimeSecondsTicker = ballWaitTimeBeforeFiring;
        //numberOfExtraBalls = 0;
        numberOfExtraBallsRemainingToFire = numberOfExtraBalls;
        numberOfBallsText.text = numberOfExtraBalls.ToString();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GameManager.Instance.CurrentGameState == GameState.Fire)
        {
            ballWaitTimeSecondsTicker -= Time.deltaTime;
            if(ballWaitTimeSecondsTicker <= 0)
            {
				if(numberOfExtraBallsRemainingToFire > 0)
                {
                    GetAndFireBall();
                }
            }
        }
	}

    private void GetAndFireBall()
    {
        Debug.Log("Get And Fire Ball");
        BallIdentifier ball = GameManager.Instance.pooledExtraBalls.GetPooledObject();
        if(ball != null)
        {
			//set ball position at the main ball launch position
            ball.Me.transform.position = new Vector2(GameManager.Instance.ballPositionX, GameManager.Instance.gameInfo.ballRestPositionY);
			//set ball active
			ball.Me.SetActive(true);
			GameManager.Instance.launchFunctionality.Launch(ball);
        }
        ballWaitTimeSecondsTicker = ballWaitTimeBeforeFiring;
        numberOfExtraBallsRemainingToFire--;
    }

    public void AllExtraBallsFired()
    {
        numberOfExtraBallsRemainingToFire = numberOfExtraBalls;
        ballWaitTimeSecondsTicker = ballWaitTimeBeforeFiring;
    }

    public void AddExtraBall()
    {
        numberOfExtraBalls++;
        numberOfBallsText.text = numberOfExtraBalls.ToString();

        if(GameManager.Instance.pooledExtraBalls.GetCurrentPoolCount() < numberOfExtraBalls)
        {
            GameManager.Instance.pooledExtraBalls.GrowPool();
        }
    }
}