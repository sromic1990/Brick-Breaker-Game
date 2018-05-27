using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameItem
{
    GameObject Me { get; set; }
}

public interface IItem : IGameItem
{
    void MoveDown();
}

