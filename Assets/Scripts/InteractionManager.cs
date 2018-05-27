using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager
{
    private InteractionState currentInteractionState;
}

public enum InteractionState
{
    Enabled = 0,
    Disabled = 1
}
