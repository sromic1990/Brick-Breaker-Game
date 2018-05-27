using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BrickColorController))]
[RequireComponent(typeof(BrickMovementController))]
public class BrickManager : MonoBehaviour, IItem
{
    [SerializeField]
    private Text brickHealthText;
    private int _brickHealth;
    private BrickColorController brickColor;
    private BrickMovementController brickMovement;

    public TypeOfItem type;

    public int BrickHealth
    {
        get { return _brickHealth; }
        set 
        {
            _brickHealth = value;
            SetBrickHealthText(_brickHealth);
            brickColor.SetColorAccordingToHealth(_brickHealth);
            if(_brickHealth <= 0)
            {
                //TODO remove brick from scene.
                GameManager.Instance.ResetItem((IItem)this, type);
            }

        }
    }

    public GameObject Me { get; set; }

	// Use this for initialization
	void Awake () 
    {
        brickHealthText = GetComponentInChildren<Text>();
        brickColor = GetComponent<BrickColorController>();
        brickMovement = GetComponent<BrickMovementController>();
        Me = this.gameObject;
	}
	
    public void TakeDamage(int damageToTake)
    {
        BrickHealth -= damageToTake;
    }

    void SetBrickHealthText(int brickHealth)
    {
        brickHealthText.text = brickHealth.ToString();
    }

    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if(other.gameObject.CompareTag("Ball"))
    //    {
    //        TakeDamage(other.gameObject.GetComponent<BallController>().ballDamage);
    //    }
    //}

    public void MoveDown()
    {
        brickMovement.MoveDown();
    }
}
