using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallIdentifier : MonoBehaviour, IGameItem
{
    public GameObject Me { get; set; }
    public Rigidbody2D rigidbody;

    void Awake()
    {
        Initialize();
    }

    public void DropBall()
    {
        rigidbody.velocity = new Vector2(0, 0);
        rigidbody.transform.position = new Vector2(0, GameManager.Instance.gameInfo.ballRestPositionY);
    }

    public void Initialize()
    {
        Me = this.gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
    }
}
