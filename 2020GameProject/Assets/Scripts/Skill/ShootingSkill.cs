using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSkill : Skill {

    private int numBullets_skill1 = 12;  // number of bullets shooting by skill1

    float cooldownTimer;
    private void Start()
    {
        cooldownTimer = cooldown;
        GameFlowManager.instance.getPlayer().cooldownBarSkill2.SetMaxCooldown(cooldown);
    }

    private void Update()
    {
        GameFlowManager.instance.getPlayer().cooldownBarSkill2.SetCooldown(cooldownTimer);
        cooldownTimer += Time.deltaTime;
    }

    public void SetShootingSkill(Attack attack, float cooldown) {
        base.SetSkill(attack, cooldown);
    }

    public override void createSkill(Transform transform)
    {
        StartCoroutine(CreateSkill(transform));
    }

    public IEnumerator CreateSkill(Transform transform) {
        if (cooldownTimer > cooldown)
        {
            Player player = GameFlowManager.instance.getPlayer();
            
            player.CameraParent.ShakeCamera(0.5f, 0.005f);
            player.GetComponent<MotionController>().animator.Play("Base Layer.Jump", 0, 0f);
            float deltaAngle = 180 / numBullets_skill1;
            // generate bullets flying from the player (one bullet for each deltaAngle degree around the player)
            for (int i = 0; i < numBullets_skill1; i++)
            {
                player.thisRB.velocity = Vector2.zero;
                Attack bullet = Instantiate(this.attack, transform.position, Quaternion.Euler(0, 0, deltaAngle * i));  // generate a bullet
                // Destroy(bullet, 0.001f);
                // set the shooting direction of this bullet
                bullet.GetComponent<Attack>().setDirection(new Vector2(Mathf.Cos(Mathf.Deg2Rad * deltaAngle * i), -Mathf.Sin(Mathf.Deg2Rad * deltaAngle * i)));
                yield return new WaitForSeconds(0.01f);
            }
            cooldownTimer = 0f;
        }
    }
}
