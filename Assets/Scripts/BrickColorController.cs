using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BrickColorController : MonoBehaviour 
{
    public Gradient gradiant;

    private Color brickColor;
    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () 
    {
        sprite = GetComponent<SpriteRenderer>();		
	}
	
    public void SetColorAccordingToHealth(int health)
    {
        //TODO Get Color info from GameManager based on current health
        //TODO Update brickColor as per health. 
    }
}
