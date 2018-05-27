using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler 
{
    public float initialLaunchPositionX { get; set; }
    public float firstBallLandingPositionX { get; set; }

    public FirstBallToLandStatus firstBallToLandStatus { get; set; }

    public int currentNumberOfBalls { get; set; }

    public LevelHandler()
    {
        initialLaunchPositionX = 0.0f;
        firstBallLandingPositionX = 0.0f;
        firstBallToLandStatus = FirstBallToLandStatus.DidNotLandYet;

    }
}

public enum FirstBallToLandStatus
{
    HasLanded = 0,
    DidNotLandYet = 1
}
