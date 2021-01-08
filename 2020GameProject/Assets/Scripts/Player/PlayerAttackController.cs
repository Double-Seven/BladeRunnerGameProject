using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Class to transform the user's controls to player attacks
public class PlayerAttackController : AttackController
{

    [Header("Shooting values")]
    public Transform muzzlePoint;  // the muzzle point of weapon
    public Transform MeleePoint; // point of melee attack
    public float shootingCoolDown;  // the cooldown time between each shoot
    public float meleeCooldown;
    public float skill1CoolDown;  // the cooldown time of skill1

    public Character character;

    private float fireCoolDownTimer = 0;  // timer for the shooting cooldown
    private float spawnRange = 0.1f;  // the vertical spawan range for bullets (to add some randomness to the bullets spawning position)
    private float meleeCooldownTimer = 0;
    private Skill shootingSkill;

    // skill1
    private float skill1CoolDownTimer = 0;  // timer for the skill1 cooldown
   


    // Start is called before the first frame update
    protected override void Start()
    {
        fireCoolDownTimer = shootingCoolDown;
        skill1CoolDownTimer = skill1CoolDown;
        meleeCooldownTimer = meleeCooldown;

        this.currentAttack = this.attacks[this.attackSelected];
       
        shootingSkill = gameObject.AddComponent<ShootingSkill>();
        shootingSkill.SetSkill(this.currentAttack, skill1CoolDown);
        this.skills.Add(shootingSkill);
    }

    // Update is called once per frame
    protected override void Update()
    {
        // update cooldown
        this.updateCooldown();

        this.spawnAttack();
    }

    private void updateCooldown() {
        fireCoolDownTimer += Time.deltaTime;
        skill1CoolDownTimer += Time.deltaTime;
        meleeCooldownTimer += Time.deltaTime;
    }

    /// <summary>
    /// Function to read input and parameters from the model to execute an attack from list of attacks
    /// </summary>
    protected override void spawnAttack() {
        if (Input.GetButtonDown("Fire") || Input.GetButton("Fire"))
        {

            this.fire();
        } else if (Input.GetButtonUp("Fire"))

        {
            animator.SetBool("IsShooting", false);
        }
        // when the skill1 button is pressed and the player is not on the ground
        if (Input.GetButtonDown("Skill1") && !character.isGrounded)
        {
            this.skill1();
        }

        if (Input.GetButtonDown("Melee")) {
           this.Melee();
        }
    }


    // tentative Melee function
    private void Melee() {
        if (meleeCooldownTimer < meleeCooldown) return;
        float deltaAngle = 1f;
        float moveForce = 1000f;
        animator.Play("Base Layer.BackJump", 0, -0.1f);
        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad*deltaAngle) * moveForce, Mathf.Sin(Mathf.Deg2Rad*deltaAngle) * moveForce);
        if (!this.character.isFacingRight)
            direction.x = direction.x * -1;
        // character.thisRB.AddForce(direction);

        MeleePoint.GetComponent<Animator>().SetTrigger("Melee");
        meleeCooldownTimer = 0f;
    }

    /// <summary>
    /// Function to handle the normal fire action of player
    /// </summary>
    private void fire()
    {
        animator.SetBool("IsShooting", true);

        // if cooldown is terminated, player can shoot
        if (fireCoolDownTimer > shootingCoolDown)
        {
            // add some randomness to the bullets spawning y-position
            Vector3 spawnPos = new Vector3(this.muzzlePoint.position.x, Random.Range(this.muzzlePoint.position.y - spawnRange, this.muzzlePoint.position.y + spawnRange), this.muzzlePoint.transform.position.z);
            Attack bullet = Instantiate(this.currentAttack, spawnPos, Quaternion.Euler(0, 0, 180 * (character.isFacingRight ? 0 : 1)));  // generate a bullet
            // set the shooting direction of this bullet depending on the player facing direction
            bullet.GetComponent<Attack>().setDirection(this.character.isFacingRight ? Vector3.right : Vector3.left);
            fireCoolDownTimer = 0f;
        }
    }


    /// <summary>
    /// Function to handle the skill1 of player
    /// </summary>
    private void skill1()
    { 
        
        // if cooldown is terminated, player can shoot
        if (skill1CoolDownTimer > shootingSkill.cooldown)
        {
            animator.Play("Base Layer.Jump", 0, 0f);
            shootingSkill.createSkill(this.transform);
            skill1CoolDownTimer = 0f;
        }
        
    }
}