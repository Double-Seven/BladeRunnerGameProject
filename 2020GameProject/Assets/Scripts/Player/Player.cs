using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Player : Character
{
	[Header("Battle values")]
	public float playerHP;
	public float protectionTime;  // the protection time after the player is getting attacked

	[Header("Motion values")]
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	/*
	public LayerMask m_WhatIsGround;                            // A mask determining what is ground to the character
	public Transform m_GroundCheck;                         // A position marking where to check if the player is grounded.
	public Transform m_CeilingCheck;                            // A position marking where to check for ceilings
	*/
	public Collider2D m_CrouchDisableCollider;              // A collider that will be disabled when crouching
	public ContactFilter2D groundContactFilter;  // for ground detection

	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
	public BoolEvent OnCrouchEvent;

	
	private Vector3 velocity = Vector3.zero;
    //private bool wasCrouching = false;
	private bool m_wasCrouching = false;
	private float groundCheckTimer = 0;  // a timer for ground check, to avoid ground detection when the player just starts jumping
	public bool isJumping = true;  // used for groundCheck

	private float getAttackedCoolDown = 0;  // timer for the protection time after the player is getting attacked

	// Transportation
	public bool isReachingTPpoint { get; set; } = false;  



	// Start is called before the first frame update
	protected override void Start()
    {
		// initialization
		base.healthPoint = playerHP;
        base.Start();  // call the start() in the base class

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
		
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		getAttackedCoolDown += Time.deltaTime;  // update the protection timer

		// update grounded state
		this.isGrounded = thisRB.IsTouching(groundContactFilter);

		groundCheckTimer += Time.deltaTime; // update timer

		// only call OnLandEvent after 0.1s of the jumping action (reset timer everytime the player jumps)
		if (groundCheckTimer >= 0.1f && isJumping)
		{   
			if (this.isGrounded)
			{
				OnLandEvent.Invoke();
				isJumping = false;
			}
		}
		checkDieAndFade();

	}


	/// <summary>
    /// Function to move the player using controller inputs
    /// </summary>
    /// <param name="move"></param>
    /// <param name="crouch"></param>
    /// <param name="jump"></param>
	public override void Move(float move, bool crouch, bool jump)
	{
		/*
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}*/

		//only control the player if grounded or airControl is turned on
		if (this.isGrounded || m_AirControl)
		{
			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, thisRB.velocity.y);
			// And then smoothing it out and applying it to the character (pass m_Velocity by reference, and SmoothDamp will change it gradually by applying smoothing)
			thisRB.velocity = Vector3.SmoothDamp(thisRB.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && isFacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (this.isGrounded && jump)
		{
			// Add a vertical force to the player.
			this.isGrounded = false;
			this.thisRB.AddForce(new Vector2(0f, m_JumpForce));

			groundCheckTimer = 0;  // reset timer
			isJumping = true;
		}
		/*
		// If the player is backJumping
		if (m_Grounded && backJump)
		{
            this.backJump(1250, 200);
		}*/
	}


    public override void Move(Vector3 destination, float speed)
    {
        // leave blank
    }


    /// <summary>
    /// This function will be called automatically when this player is colliding with any collider
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
		// if collided with monster or monster bullet, the player is considered getting attacked
		if (collision.gameObject.tag == "Monster" || collision.gameObject.tag == "MonsterBullet")
		{
			if(getAttackedCoolDown >= 0.75f)
            {
				this.getAttacked(1);
				getAttackedCoolDown = 0;  // reset the protection timer
			}
		}
	}

}
