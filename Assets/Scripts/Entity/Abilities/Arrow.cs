using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arrow : Ability
{
    public Arrow(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, id, readable, particles)
    {
       
    }

    public override void SpawnProjectile(GameObject source, GameObject owner, string abilityID, bool isPlayer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit target;
        Physics.Raycast(ray, out target, Mathf.Infinity);
        Vector3 vectorToMouse = target.point - source.transform.position;
        Vector3 forward = new Vector3(vectorToMouse.x, source.transform.forward.y, vectorToMouse.z).normalized;

        GameObject projectile = (GameObject)GameObject.Instantiate(particleSystem, source.transform.position, Quaternion.Euler(forward));

        projectile.GetComponent<ProjectileBehaviour>().owner = owner;
        projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 5.0f;
        projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;

        // apply velocity

        projectile.rigidbody.velocity = forward * 20.0f;
    }

    public override void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {
        if (isPlayer == true)
        {
            if (target.GetComponent<AIController>().IsResetting() == false
                    && target.GetComponent<AIController>().IsDead() == false)
            {
                Entity defender = target.GetComponent<Entity>();
                DoDamage(source, target, attacker, defender, isPlayer);

            }
        }

        else
        {
            Entity defender = target.GetComponent<Entity>();
            DoDamage(source, target, attacker, defender, isPlayer);
        }
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {

        float damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);

        if (isPlayer == true)
        {
            Debug.Log("damage: " + damageAmt);
        }

        defender.currentHP -= damageAmt;

        float ratio = (defender.currentHP / defender.maxHP);

        if (isPlayer == true)
        {
            target.renderer.material.color = new Color(1.0f, ratio, ratio);
        }
    }
}
