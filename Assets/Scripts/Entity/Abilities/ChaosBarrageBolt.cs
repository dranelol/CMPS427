using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaosBarrageBolt : Ability
{
    public ChaosBarrageBolt(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }


    public override void SpawnProjectile(GameObject source, Vector3 target, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {
        int segments = 4;

        for (int i = 0; i < segments; i++)
        {
            GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ChaosboltProjectile, CombatMath.GetCenter(source.transform) + Rotations.RotateAboutY(forward, (360 / segments) * i) * 2, Quaternion.LookRotation(Rotations.RotateAboutY(forward, (360 / segments) * i)));
            //GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().FireballProjectile, CombatMath.GetCenter(source.transform) + Rotations.RotateAboutY(forward, (360 / segments) * i) * 2, Quaternion.LookRotation(Rotations.RotateAboutY(forward, (360 / segments) * i)));

            projectile.GetComponent<ProjectileBehaviour>().owner = owner;
            projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 5.0f;
            projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;
            projectile.GetComponent<ProjectileBehaviour>().target = target;
            projectile.GetComponent<ProjectileBehaviour>().homing = true;
            projectile.GetComponent<ProjectileBehaviour>().speed = 15f;


            //Vector3 randPos = source.transform.position + forward + Random.onUnitSphere * 1f;
            Vector3 randPos = source.transform.position + Rotations.RotateAboutY(forward, (360 / segments) * i) + Random.onUnitSphere * 1f;

            randPos.Set(randPos.x, randPos.y + 2, randPos.z);

            Vector3 direction = (randPos - source.transform.position).normalized;

            projectile.transform.rotation = Quaternion.LookRotation(direction);
            //projectile.rigidbody.velocity = Rotations.RotateAboutY(direction, (360 / segments) * i) * 20.0f;
            //projectile.rigidbody.velocity = direction * 20.0f;
        }

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
                if (target.GetComponent<AIController>().IsInCombat() == false)
                {
                    target.GetComponent<AIController>().BeenAttacked(source);
                }

            }
        }

        else
        {
            Entity defender = target.GetComponent<Entity>();
            DoDamage(source, target, attacker, defender, isPlayer);
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer, target));
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
        float damageAmt;
        if (isPlayer == true)
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);
        }
        else
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, 0);
        }
        if (isPlayer == true)
        {
            Debug.Log("damage: " + damageAmt);
        }

        defender.ModifyHealth(-damageAmt);
    }



    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target)
    {
        Debug.Log("explode");
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, target.transform.position, source.transform.rotation);

        yield return new WaitForSeconds(time);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();
        
        

        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            //item.enableEmission = false;

        }

        GameObject.Destroy(particles);

        yield return null;
    }

}
