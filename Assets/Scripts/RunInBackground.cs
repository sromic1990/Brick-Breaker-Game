using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunInBackground : MonoBehaviour 
{
    public bool CanRunInBackground;
	
	// Update is called once per frame
	void Update () 
    {
		Application.runInBackground = CanRunInBackground;
	}
}
