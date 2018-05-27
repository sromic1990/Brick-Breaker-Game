using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererShowFix : MonoBehaviour 
{
    LineRenderer lr;

	// Use this for initialization
	void Start () 
    {
        lr = GetComponent<LineRenderer>();
        lr.sortingLayerName = "FG";
		
	}
}
