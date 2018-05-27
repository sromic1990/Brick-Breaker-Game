using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBallPowerUp : MonoBehaviour, IItem
{
    public GameObject Me { get; set; }

    void Awake()
    {
        Me = this.gameObject;
    }

    public void MoveDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }
}
