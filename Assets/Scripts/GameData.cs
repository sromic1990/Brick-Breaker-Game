using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO make this a scriptable object.
//TODO Eventually move all data from GameManager to here.
public class GameData : MonoBehaviour 
{
    public static GameData Instance;

    [Header("Ball Related")]
    public Sprite ballSprite;
    public Vector3 ballScale;
    public float ballRestPositionY;
    public Color mainBallColor;
    public Color followerBallColor;
    public PhysicsMaterial2D ballPhysicsMaterial;
    public float ballRigidbodyGravityScale;
    public int ballDamage;
    public float ballConstantSpeed;

    [Header("PowerUp Related")]
    public int NumberOfExtraBallsPerRow;

    [Header("Ball move after landing")]
    public float TimeForBallToGoToNextLaunchPosition;

    void Awake()
    {
        Instance = this;
    }
}
