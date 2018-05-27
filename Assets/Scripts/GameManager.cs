using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sourav.Utilities.Scripts;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;

    [Header("Bricks and SpawnPoints")]
    public Transform[] SpawnPoints;
   
    public GameObject SquareBrick;
    public GameObject TriangleBrick;
    public GameObject ExtraBallPowerUp;

    public int numberOfBricksToStart;

    [Header("Pool Data")]
    [SerializeField]
    private int poolBrickAmount;
    [SerializeField]
    private int poolExtraBallPowerUpAmount;
    [SerializeField]
    private int poolExtraBallsAmount;

    [Header("Ball Related Data")]
    public GameObject MainBallPrefab;
    public GameObject FollowerBallPrefab;

    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private int currentNumberOfBalls;

    private bool _canInteract;
    public bool CanInteract
    {
        get { return _canInteract; }
    }

    private GameState _gameState;
    public GameState CurrentGameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    public float ballPositionX;

    public bool FirstBallLanded;

    private ObjectPool<BrickManager> pooledSquareBricks;
    private ObjectPool<BrickManager> pooledTriangleBricks;
    private ObjectPool<ExtraBallPowerUp> pooledExtraBallPowerups;
    public ObjectPool<BallIdentifier> pooledExtraBalls;

    public GameData gameInfo;
    public LaunchFunctionality launchFunctionality;

    private int ballsForRound;
    private int ballsLandedInThisRound;

    public BallController MainBall;

    void Awake()
    {
        Instance = this;
        launchFunctionality = new LaunchFunctionality();

        gameInfo = GameData.Instance;

        pooledSquareBricks = new ObjectPool<BrickManager>("Square Bricks Holder", SquareBrick, poolBrickAmount);
        pooledTriangleBricks = new ObjectPool<BrickManager>("Triangle Bricks Holder", TriangleBrick, poolBrickAmount);
        pooledExtraBallPowerups = new ObjectPool<ExtraBallPowerUp>("ExtraBallPowerups", ExtraBallPowerUp, poolExtraBallPowerUpAmount);
        pooledExtraBalls = new ObjectPool<BallIdentifier>("ExtraBalls", FollowerBallPrefab, poolExtraBallsAmount, true, 100);

        MainBallPrefab = (GameObject)Instantiate(MainBallPrefab, new Vector3(0, gameInfo.ballRestPositionY, 0), Quaternion.identity);
        MainBall = MainBallPrefab.GetComponent<BallController>();

        currentNumberOfBalls = 1;
        ballPositionX = 0.0f;
        FirstBallLanded = false;
        ballsForRound = 0;
        ballsLandedInThisRound = 0;
    }

    void Start()
    {
        EnableInteraction();
    }

    public void CreateLevelBricks(bool deleteOlderItems = false)
    {
        bool createdAllExtraBallPowerupForLine = false;
        int numberOfPowerUpsCreated = 0;

        if(deleteOlderItems)
        {
            pooledSquareBricks.ReturnAllPooledObjects();
            pooledTriangleBricks.ReturnAllPooledObjects();
            pooledExtraBallPowerups.ReturnAllPooledObjects();
        }

        int numberOfBricksCreated = 0;

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            int itemToCreate = Random.Range(0, 4);

            if (itemToCreate == (int)TypeOfItem.SquareBrick)
            {
                //TODO generate square block at the spawn position
                BrickManager bm = pooledSquareBricks.GetPooledObject();
                if(bm != null)
                {
					bm.transform.position = SpawnPoints[i].position;
					bm.BrickHealth = currentLevel;
                    bm.type = TypeOfItem.SquareBrick;
					bm.gameObject.SetActive(true);
					
					numberOfBricksCreated++;
                }
            }
            else if (itemToCreate == (int)TypeOfItem.TriangleBrick)
            {
                //TODO create triangle brick
                BrickManager bm = pooledTriangleBricks.GetPooledObject();
                if(bm != null)
                {
					bm.transform.position = SpawnPoints[i].position;
					bm.BrickHealth = currentLevel;
                    bm.type = TypeOfItem.TriangleBrick;
					bm.gameObject.SetActive(true);
					
					numberOfBricksCreated++;
                }
            }
            else if (itemToCreate == (int)TypeOfItem.ExtraBallPowerUp && !createdAllExtraBallPowerupForLine)
            {
                //TODO Create an extra ball powerUp
                ExtraBallPowerUp ebpu = pooledExtraBallPowerups.GetPooledObject();
                if(ebpu != null)
                {
                    ebpu.transform.position = SpawnPoints[i].position;
                    ebpu.gameObject.SetActive(true);
                }
                numberOfPowerUpsCreated++;
                createdAllExtraBallPowerupForLine = (numberOfPowerUpsCreated >= gameInfo.NumberOfExtraBallsPerRow);
            }

            if (numberOfBricksCreated >= numberOfBricksToStart)
            {
                return;
            }
        }
    }


	public void EnableInteraction()
    {
        //Debug.Log("Enabled interaction");

        _canInteract = true;
        FirstBallLanded = false;
        CurrentGameState = GameState.Aim;
        launchFunctionality.ResetValues();
        ballsLandedInThisRound = 0;

        currentLevel++;

        CreateLevelBricks();
    }

    public void DisableInteraction()
    {
        _canInteract = false;
    }

    public void BallLaunchedForRound()
    {
        //+1 due to presence of unpooled leader ball
        ballsForRound = ExtraBallManager.Instance.numberOfExtraBalls + 1;
        //Debug.Log("Balls for round = " + ballsForRound);
    }

    public void BallLandedOnGround(float ballPositionX)
    {
        // a state machine so that ballPositionX will be changed only once per turn.
        if (!FirstBallLanded)
        {
            this.ballPositionX = ballPositionX;
            FirstBallLanded = true;

            Debug.Log("First Ball landed");
        }

        //all of the following code must execute if all the balls are on ground
        ballsLandedInThisRound++;

        if (ballsForRound == ballsLandedInThisRound && FirstBallLanded)
        {
			AllBallsLanded();
        }
    }

    public void AllBallsLanded()
    {
        Debug.Log("All balls landed");

        //Game wait
        CurrentGameState = GameState.Wait;

        if(pooledExtraBalls.GetCurrentActiveCount() > 0)
        {
            pooledExtraBalls.ReturnAllPooledObjects();
        }

        //Getting all active bricks 
        List<BrickManager> activeBricksSquare = pooledSquareBricks.GetAllActiveObjects();
        List<BrickManager> activeBricksTriangle = pooledTriangleBricks.GetAllActiveObjects();
        List<ExtraBallPowerUp> activeExtraBallPowerUps = pooledExtraBallPowerups.GetAllActiveObjects();

        //Pulling down all visible bricks and extra ball powerups
        for (int i = 0; i < activeBricksSquare.Count; i++)
        {
            activeBricksSquare[i].MoveDown();
            CheckForItemHeightInY(activeBricksSquare[i], TypeOfItem.SquareBrick);
        }

        for (int j = 0; j < activeBricksTriangle.Count; j++)
        {
            activeBricksTriangle[j].MoveDown();
            CheckForItemHeightInY(activeBricksTriangle[j], TypeOfItem.TriangleBrick);
        }

        for (int k = 0; k < activeExtraBallPowerUps.Count; k++)
        {
            activeExtraBallPowerUps[k].MoveDown();
            //TODO check if the powerup is below the ballY
            CheckForItemHeightInY(activeExtraBallPowerUps[k], TypeOfItem.ExtraBallPowerUp);
        }

        ExtraBallManager.Instance.AllExtraBallsFired();

        //Increase level
        //currentLevel++;

        //Add a new level of bricks
        //CreateLevelBricks();

        //Reactivate the game scene
        EnableInteraction();
    }

    private void CheckForItemHeightInY(IGameItem item, TypeOfItem typeOfItem)
    {
        //Check if the object is below the ball level, that is below the ground, disable it
        if(item.Me.transform.position.y < gameInfo.ballRestPositionY)
        {
            ResetItem(item, typeOfItem);
        }
    }

    public void ResetItem(IGameItem item, TypeOfItem typeOfItem)
    {
        switch(typeOfItem)
        {
            case TypeOfItem.SquareBrick:
                pooledSquareBricks.ReturnPooledObject((BrickManager)item);
                break;

            case TypeOfItem.TriangleBrick:
                pooledTriangleBricks.ReturnPooledObject((BrickManager)item);
                break;

            case TypeOfItem.ExtraBallPowerUp:
                pooledExtraBallPowerups.ReturnPooledObject((ExtraBallPowerUp)item);
                break;
        }
    }

    public void CollectedExtraBallPowerUp(ExtraBallPowerUp powerUp)
    {
        if(powerUp!=null)
        {
            pooledExtraBallPowerups.ReturnPooledObject(powerUp);
            //TODO add a ball to current number of balls.
            ExtraBallManager.Instance.AddExtraBall();
        }
    }

    public BallIdentifier GetExtraBall()
    {
        BallIdentifier ballIdentifier = pooledExtraBalls.GetPooledObject();

        return ballIdentifier;
    }

    public void PullDownButtonPressed()
    {
        //CurrentGameState = GameState.Wait;

        MainBall.DropBall();
        BallLandedOnGround(0);
        
        List<BallIdentifier> powerUpBallsInScene = pooledExtraBalls.GetAllActiveObjects();
        for (int i = 0; i < powerUpBallsInScene.Count; i++)
        {
            DropBall(powerUpBallsInScene[i]);
        }

        AllBallsLanded();
    }

    private void DropBall(BallIdentifier ball)
    {
        ball.DropBall();
        BallLandedOnGround(0);
    }
}

public enum GameState
{
    Aim,
    Fire,
    Wait,
    EndOfTurn
}

public enum TypeOfItem
{
    SquareBrick = 0,
    TriangleBrick = 1,
    ExtraBallPowerUp = 2
}

//DONE the ball script must be segregated into two parts, such that there is one main ball in the scene and all other balls are following it. Main ball will have all the properties we mark in the inspector and the other balls must have properties derived from the main ball.
//TODO balls must be stopping only at the ground and not on anywhere else or mid air.
//TODO Bug, upon bringing all balls down, game should stop
//TODO work on statemachine and clean the code.
