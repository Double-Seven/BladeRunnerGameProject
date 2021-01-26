using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Class to transform the user's controls to player motions and animations
public class PlayerMovementController : MotionController
{
	public Player player;
	public Animator thisAnimator;
	public float runSpeed = 20f;
	public GameObject dashEffect;
	public CameraShake CameraParent;
	public float quickMoveCooldown;

	private GameFlowManager gameFlowManager;

	private float horizontalMove = 0f;
	//private bool isFiring = false;
	private bool isJumping = false;
	private bool isCrouching = false;
    //private bool isQuickMoving = false;
	private bool isRunning = false;
	private bool isTeleporting = false;
	private Skill quickMoveSkill;

	

    private void Start()
    {
		base.animator = thisAnimator;

		// initialize motion skill
		this.quickMoveSkill = gameObject.AddComponent<QuickMove>().SetQuickMove(null, quickMoveCooldown, player, dashEffect, CameraParent);

		gameFlowManager = GameObject.Find("GameManager").GetComponent<GameFlowManager>();
	}

    // Update is called once per frame
    void Update()
	{

		// determine if the player is try to teleport to the next map
		if(Input.GetButtonDown("Up") && player.isReachingTPpoint)
		{
			gameFlowManager.loadNextLevel(1);  // 1 second delay before the loading animation
			isTeleporting = true;
			player.fade -= Time.deltaTime * 1f;  // dissolve the player when teleport
		}
		else
        {
			// update the fade value of player (dissolve effect)
			if (isTeleporting)
            {
				player.fade -= Time.deltaTime * 2f;  // dissolve the player when teleport
			}
		}
		

		// get the user's input of horizontal movement
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		isRunning = Mathf.Abs(horizontalMove) > 1;

		// detect the player speed and update the animator parameter
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		// detect whether player has pressed Jump and update the animator parameter
		if (Input.GetButtonDown("Jump"))
		{
			isJumping = true;
			animator.SetBool("IsJumping", this.isJumping);
		}

		/*
		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		}
		else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}*/

		
		// detect whether player has pressed Fire and update the animator parameter
		if (Input.GetButtonDown("Fire"))
		{
			//isFiring = true;
		}
		else if(Input.GetButtonUp("Fire"))
		{
			//isFiring = false;
		}

		
		if (Input.GetButtonDown("QuickMove"))
		{
			QuickMove();
		}

		

		// check if we need to trigger the dying animation
		if(player.isDead)
        {
			destroy();
		}

	}

	void QuickMove() {
		this.quickMoveSkill.runSkill(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
	}

	// used for physical updates
	void FixedUpdate()
	{
		// Move our character
		player.Move(horizontalMove * Time.fixedDeltaTime, isCrouching, isJumping);
		isJumping = false;
		//isQuickMoving = false;
	}


	/// <summary>
	/// Event function that resets the parameter in animator
	/// Will be invoked in CharacterController class
	/// </summary>
	public override void OnLanding()
	{
		isJumping = false;
		animator.SetBool("IsJumping", isJumping);
	}

	public void OnCrouching(bool isCrouching)
	{
		// animator.SetBool("IsCrouching", isCrouching);
	}

	/// <summary>
	/// Function to play destroying animation
	/// </summary>
	protected override void destroy()
	{
		// we won't destroy the player game object

		animator.SetTrigger("IsDying");  // trigger the dying animation
		// deactivate the player animator
		//this.player.gameObject.GetComponent<Animator>().enabled = false;

		// deactivate the player moving sript
		this.player.gameObject.GetComponent<PlayerMovementController>().enabled = false;
		this.player.gameObject.GetComponent<PlayerAttackController>().enabled = false;
	}



}
