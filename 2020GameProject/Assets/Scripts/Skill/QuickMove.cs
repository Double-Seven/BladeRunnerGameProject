using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMove : Skill {

    // which Character object to apply the skill
    Character target;
    MotionController movementcontroller;
    GameObject effect;
    CameraShake cameracontroller;
    float cooldownTimer = 0f;
    float duration = 0.4f;

    public QuickMove SetQuickMove(Attack attack, float cooldown, Character target, GameObject effect, CameraShake cameracontroller) {
        base.SetSkill(attack, cooldown);
        this.target = target;
        this.effect = effect;
        this.cameracontroller = cameracontroller;
        base.targets.Add(target);
        movementcontroller = target.GetComponent<MotionController>();
        cooldownTimer = cooldown;
        return this;
    }

    private void Start()
    {
        GameFlowManager.instance.getPlayer().cooldownBarDash.SetMaxCooldown(cooldown);
    }

    void Update() {
        cooldownTimer += Time.deltaTime;
        GameFlowManager.instance.getPlayer().cooldownBarDash.SetCooldown(cooldownTimer);
        //        if (target.fade < 1f && cooldownTimer < duration && !target.isDead)
        //           target.fade += Time.deltaTime * 1f;
        if (cooldownTimer >= duration && target.isInvincible) {
            endSkill();
        }
    }

    public override void runSkill(Vector2 move) { 
        if (cooldownTimer < cooldown) return;
        if (target.gameObject.tag == "Player")
            Time.timeScale = 0.4f;
        float verticalMove = move.y * 5;
        bool isJumping = false;
        bool isCrouching = false;
 
        float deltaAngle = verticalMove;
        float JumpForce = 4000f;

        
        Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad*deltaAngle) * JumpForce, Mathf.Sin(Mathf.Deg2Rad*deltaAngle) * JumpForce);

        // if (!this.target.isFacingRight)
            direction.x = direction.x * move.x;
        
       if (verticalMove > 0) {
            isJumping = true;
            // disable quickmove higher when in air
            direction.y = 0;
        }

        // allow for dash jump when on ground
        target.Move(0, isCrouching, isJumping);

        target.thisRB.AddForce(direction);
        ShowEffects(isJumping);

    }

    void ShowEffects(bool isJumping) {
        target.fade = 0.4f; // fade effect in reverse
		target.isInvincible = true;
        foreach (Collider col in target.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
		
		cooldownTimer = 0;
		if (!isJumping) {
			movementcontroller.animator.SetTrigger("Crouch");
		} else {
            movementcontroller.animator.SetBool("IsJumping", true);
			movementcontroller.animator.Play("Jump", 0, 0f);
		}
        if (!effect || !cameracontroller) return;
		Instantiate(effect, target.transform.position, Quaternion.identity);
		cameracontroller.ShakeCamera(0.5f, 0.005f);
    }


    void endSkill() {
        if (target.gameObject.tag == "Player")
            Time.timeScale = 1f;
        target.isInvincible = false; // end of quickmove invincibility
        foreach (Collider col in target.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
        target.fade = 1f;
    }
}
