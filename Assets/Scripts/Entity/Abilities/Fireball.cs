using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireball : Ability
{
    public Fireball(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles, 2)
    {
        
    }

    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {
        int segments = 1;
        // create "segments" amount of fireballs, spaced evenly around the source
        for(int i = 0; i < segments; i++)
        {
            // create projectile
            GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().FireballProjectile, CombatMath.GetCenter(source.transform) + Rotations.RotateAboutY(forward, (360 / segments) * i) * 2, Quaternion.LookRotation(Rotations.RotateAboutY(forward, (360 / segments) * i)));

            // set the owner to the entity spawning the projectile
            projectile.GetComponent<ProjectileBehaviour>().owner = owner;
            // activate in 3 seconds
            projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 3.0f;

            projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;
            // we don't want fireball to collide with terrain
            projectile.GetComponent<ProjectileBehaviour>().CollidesWithTerrain = false;

            projectile.GetComponent<Rigidbody>().velocity = Rotations.RotateAboutY(forward, (360 / segments) * i) * 20.0f;
        }

    }

    public override void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {
        if (isPlayer == true)
        {
            // if our target isn't dead or resetting
            if (target.GetComponent<AIController>().IsResetting() == false
                    && target.GetComponent<AIController>().IsDead() == false)
            {
                // get defender, do damage, put the target in combat if they weren't before
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

        // run the animation associated with the on-hit of this ability
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer, target));
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
        // run damage calc, apply damage
        float damageAmt;
        if (isPlayer == true)
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);

            if (attacker.gameObject.GetComponent<TalentManager>().Bonuses["fire"] == true)
            {
                damageAmt += damageAmt * 0.1f;
            }
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
        // instantiate particle system for the on-hit of this ability
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, CombatMath.GetCenter(target.transform), source.transform.rotation);

        // wait for "time" seconds
        yield return new WaitForSeconds(time);

        // get all particle systems from the prefab
        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        // unparent each particle system, and disable emission. we do this to make sure that each particle system has a chance to finish its full cycle before being destroyed
        // each particle system has a script to destroy itself when emission fully stops
        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;

        }

        // destroy the entire particle system object
        GameObject.Destroy(particles);

        yield return null;
    }
    
}
