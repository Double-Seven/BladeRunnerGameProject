using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Monster : Character
{
    [Header("Battle values")]
    public float monsterHP;

	Vector3 velocity = Vector3.zero;
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    
	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

    // Use this for initialization
    protected override void Start()
    {
        // add self to Manager list
        GameFlowManager.instance.monsters.Add(this);
        this.isFacingRight = false;
        this.healthPoint = monsterHP;
        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
        base.Start();
    }

    private void OnDestroy()
    {
        GameFlowManager.instance.monsters.Remove(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkDieAndFade();
    }


    /// <summary>
    /// Function to move this monster to the given destination
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="speed"></param>
    public override void Move(Vector3 destination, float speed)
    {
        Vector3 direction = (destination - this.transform.position).normalized;

        // change the facing direction if necessary
        if(direction.x <= 0 && isFacingRight)
        {
            Flip();
        }
        else if(direction.x > 0 && !isFacingRight)
        {
            Flip();
        }

        // move this monster to the destination
        this.thisRB.MovePosition(this.transform.position + direction * speed * Time.deltaTime);  
    }


    public override void Move(float move, bool crouch, bool jump)
    {
        // TODO (almost) same as Player.Move, need refactor
        if (this.isGrounded)
		{

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

		}
		/*
		// If the player is backJumping
		if (m_Grounded && backJump)
		{
            this.backJump(1250, 200);
		}*/
    }

    /// <summary>
    /// This function will be called automatically when this monster is colliding with any collider
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // if collided with monster or monster bullet, the player is considered getting attacked
        if (collision.gameObject.tag == "PlayerBullet")
        {
            this.getAttacked(1);
        }

        if (collision.gameObject.tag == "Ground" && !isGrounded) {
            OnLandEvent.Invoke();
            isGrounded = true;
        }
    }
     
}
