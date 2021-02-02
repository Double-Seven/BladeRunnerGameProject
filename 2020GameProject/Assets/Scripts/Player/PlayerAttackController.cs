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
    public float skill1CoolDown;  // the cooldown time of skill1

    public Character character;

    private float fireCoolDownTimer = 0;  // timer for the shooting cooldown
    private float spawnRange = 0.01f;  // the vertical spawan range for bullets (to add some randomness to the bullets spawning position)
    private Skill shootingSkill;
    private ForceFieldSkill forceFieldSkill;

    // skill1
    private float skill1CoolDownTimer = 0;  // timer for the skill1 cooldown

    int attacksFired = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        fireCoolDownTimer = shootingCoolDown;

        this.currentAttack = this.attacks[this.attackSelected];
       
        shootingSkill = gameObject.AddComponent<ShootingSkill>();
        shootingSkill.SetSkill(this.currentAttack, skill1CoolDown);
        this.skills.Add(shootingSkill);

        forceFieldSkill = gameObject.AddComponent<ForceFieldSkill>();
        Attack defaultff = null;
        foreach (Attack att in attacks)
        {
            if (att.gameObject.name.Contains("ForceField"))
            {
                defaultff = att;
            }
        }
        forceFieldSkill.SetForceField(defaultff, 10f, character);
    }

    // Update is called once per frame
    protected override void Update()
    {
        this.currentAttack = this.attacks[this.attackSelected];
            
        // update cooldown
        this.updateCooldown();

        this.spawnAttack();
    }

    private void updateCooldown() {
        fireCoolDownTimer += Time.deltaTime;
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

        if (Input.GetButtonDown("Skill2")) {
           this.Skill2();
        }
    }


    // tentative Melee function
    private void Skill2() {
        forceFieldSkill.createSkill(transform);
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
            // shoot a ray laser every 5 fire
            attacksFired++;
            if (attacksFired >= 5)
            {
                this.attackSelected = 1;
                attacksFired = 0;
            } else
            {
                this.attackSelected = 0;
            }
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
        shootingSkill.createSkill(this.transform); 
    }
}