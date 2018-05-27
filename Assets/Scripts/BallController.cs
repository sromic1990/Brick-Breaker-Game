using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BallCollision))]
public class BallController : BallIdentifier 
{
    private SpriteRenderer sprite;

    private Vector2 mouseStartPosition;
    private Vector2 mouseEndPosition;

    private float ballVelocityX;
    private float ballVelocityY;

    private LineRenderer arrow;

    void Awake()
    {
        base.Initialize();
        sprite = GetComponent<SpriteRenderer>();
        arrow = GetComponentInChildren<LineRenderer>();
    }

    void Start()
    {
        arrow.gameObject.SetActive(false);
    }

    void Update()
    {
		//Debug.Log("GameManager State = " + GameManager.Instance.CurrentGameState);
        switch(GameManager.Instance.CurrentGameState)
        {
            case GameState.Aim:
                //if (GameManager.Instance.CanInteract)
                //{
					if (Input.GetMouseButtonDown(0))
					{
						MouseClicked();
					}
					if (Input.GetMouseButton(0))
					{
						MouseDrag();
					}
					if (Input.GetMouseButtonUp(0))
					{
						MouseRelease();
					}
                //}
                break;

            case GameState.Fire:
                break;

            case GameState.Wait:
                break;

            case GameState.EndOfTurn:
                break;

            default:
                break;
        }
    }

    public void MouseClicked()
    {
        GameManager.Instance.launchFunctionality.MouseStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    public void MouseDrag()
    {
        //TODO add angle.
        //TODO check if actually dragged.
        //TODO add arrow

        //Debug.Log("Mouse Dragged");

        arrow.gameObject.SetActive(true);
        Vector2 tempMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float diffX = GameManager.Instance.launchFunctionality.MouseStartPosition.x - tempMousePosition.x;
        float diffY = GameManager.Instance.launchFunctionality.MouseStartPosition.y - tempMousePosition.y;
        //diffY must be > 0
        if (diffY <= 0)
        {
            diffY = 0.01f;
            Debug.Log("diffY <= 0");
        }
        float theta = Mathf.Rad2Deg * Mathf.Atan(diffX / diffY);
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, -1*theta);
        //TODO Ball angling improvement. It does not go down below a certain point and always is angled. Never goes straight up.
    }

    public void MouseRelease()
    {
        GameManager.Instance.launchFunctionality.MouseEndPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		arrow.gameObject.SetActive(false);

		GameManager.Instance.CurrentGameState = GameState.Fire;

        //Launching Actual Ball here
        GameManager.Instance.launchFunctionality.Launch(this);

		GameManager.Instance.DisableInteraction();

        GameManager.Instance.BallLaunchedForRound();
    }

}
