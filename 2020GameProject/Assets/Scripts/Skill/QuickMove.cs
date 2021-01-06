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
    float duration = 0.5f;

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

    void Update() {
        cooldownTimer += Time.deltaTime;
        if (target.fade < 1f)
            target.fade += Time.deltaTime * 1f;
        if (cooldownTimer >= duration && target.isInvincible) {
            endSkill();
        }
    }

    public override void runSkill(Vector2 move) { 
        if (cooldownTimer < cooldown) return;
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
        target.fade = 0.2f; // fade effect in reverse
		target.isInvincible = true;
		target.GetComponent<CapsuleCollider2D>().enabled = false;
		target.GetComponent<CircleCollider2D>().enabled = false;
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
        target.isInvincible = false; // end of quickmove invincibility
		target.GetComponent<CapsuleCollider2D>().enabled = true;
		target.GetComponent<CircleCollider2D>().enabled = true;
    }
}
