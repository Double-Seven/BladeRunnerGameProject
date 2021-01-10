using UnityEngine;
using System.Collections;


// Class to control the monster motion and animation
public class PlayerMonsterMovementController : MotionController
{
	public Animator thisAnimator;
	public float movingSpeed = 10f;

	private Player player;
	private Monster monster;
	private Vector3 currentPosition;
	private Vector3 wanderDest = Vector3.zero;  // the current destination of wandering action
	private bool isDestroyed = false; 

    private Skill quickMoveSkill;

    private float quickMoveCooldown = 2f;


    private void Start()
    {
		base.animator = thisAnimator;
		monster = GetComponent<Monster>();

		currentPosition = this.transform.position;

		// get the player gameObject from the game flow manager
		player = GameFlowManager.instance.getPlayer();

        this.quickMoveSkill = gameObject.AddComponent<QuickMove>().SetQuickMove(null, quickMoveCooldown, monster, null, null);
	}



    // Update is called once per frame
    void Update()
	{

		// check for destroying condition
		if(monster.checkHP() && !isDestroyed)
        {
			isDestroyed = true;
			// deactivate the monster moving sript
			this.gameObject.GetComponent<PlayerMonsterAttackController>().enabled = false;
            this.gameObject.GetComponent<PlayerMonsterMovementController>().enabled = false;
			this.gameObject.GetComponent<PlayerMonsterAI>().enabled = false;
			animator.SetTrigger("IsDying");
            monster.isDead = true;
			// destroy(); // this monster will be called when the animation finished (using animation event setting)
        }
	}


	// used for physical updates
	void FixedUpdate()
	{
		// Move our character
		//monster.Move(player.transform.position, movingSpeed);

		// calculate the current velocity using the positions
		float currentSpeed = (this.monster.transform.position - currentPosition).magnitude / Time.deltaTime;
		currentPosition = this.monster.transform.position;

		// use the monster speed to update the animator parameter
		animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
	}


	/// <summary>
    /// Function to perform wandering action for this monster
    /// </summary>
	public void wander()
    {
		// if currently no wandering destination, assign a new one
		if(wanderDest == Vector3.zero)
        {
			int xDir = Random.Range(0,2);
			// move to left if xDir is 0, move to right if xDir is 1
			Vector3 direction = new Vector3((xDir == 0 ? -1 : 1), transform.position.y, transform.position.z);

			float randDistance = Random.Range(2, 5);  // get a random moving distance between 2 and 5

			direction.x *= randDistance;
			this.wanderDest = direction;

			// move the monster toward the target
			monster.Move(wanderDest, movingSpeed);
		}
		else  // else, move toward the current destination
        {
			// if the monster is reaching the wander destination, reset the destination to zero
			if(Vector3.Distance(this.transform.position, this.wanderDest) <= 0.1f)
            {
				this.wanderDest = Vector3.zero;
            }
			else
            {
				// move the monster toward the target
				monster.Move(wanderDest, movingSpeed);
			}
        }
    }

	/// <summary>
    /// Function to perform path finding to the target
    /// </summary>
    /// <param name="target"></param>
	public void pathFinding(Vector3 target)
    {
		// move the monster toward the target
		monster.Move(target.x / Mathf.Abs(target.x) * movingSpeed * Time.fixedDeltaTime, false, false);
	}


    public void jump() {
        animator.SetBool("IsJumping", true);
        monster.Move(0f, false, true);
    }

    public void quickMove() {
        // disable air dash due to bug
        if (!monster.isGrounded) return;
        Vector2 move = Vector2.zero;
        // always dash in the direction it's facing
        move.x = monster.isFacingRight ? 1 : -1;
        // move.y = Random.Range(-1, 2) > 0 ? 1 : -1;
        quickMoveSkill.runSkill(move);
    }

	/// <summary>
	/// Event function that resets the parameter in animator
	/// Will be invoked in CharacterController class
	/// </summary>
	public override void OnLanding()
	{
		animator.SetBool("IsJumping", false);
	}

	public void OnCrouching(bool isCrouching)
	{
		//animator.SetBool("IsCrouching", isCrouching);
	}


}
