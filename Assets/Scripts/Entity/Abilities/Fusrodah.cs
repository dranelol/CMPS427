﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fusrodah : Ability
{
    public Fusrodah(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles, 2)
    {
       
    }
                                            
    public override void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {
        List<GameObject> attacked = OnAttack(source, isPlayer);

        if (isPlayer == true)
        {

            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    Entity defender = enemy.GetComponent<Entity>();
                    DoDamage(source, enemy, attacker, defender, isPlayer);
                    DoPhysics(source, enemy);
                    if (enemy.GetComponent<AIController>().IsInCombat() == false)
                    {
                        enemy.GetComponent<AIController>().BeenAttacked(source);
                    }
                }
            }
        }

        else
        {
            foreach (GameObject enemy in attacked)
            {
                Entity defender = enemy.GetComponent<Entity>();
                DoDamage(source, enemy, attacker, defender, isPlayer);
                DoPhysics(source, enemy);
            }
        }


        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer));
    
    }

    public override List<GameObject> OnAttack(GameObject source, bool isPlayer)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = new Vector3();

        // this is a player attack, forward attack vector will be based on cursor position
        if (isPlayer == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit target;
            Physics.Raycast(ray, out target, Mathf.Infinity);
            Vector3 vectorToMouse = target.point - source.transform.position;
            forward = new Vector3(vectorToMouse.x, source.transform.forward.y, vectorToMouse.z).normalized;
        }

        int enemyMask = LayerMask.NameToLayer("Enemy");
        int playerMask = LayerMask.NameToLayer("Player");

        Collider[] colliders;

        if (isPlayer == true)
        {
            colliders = Physics.OverlapSphere(source.transform.position, range, 1 << enemyMask);
        }

        else
        {
            colliders = Physics.OverlapSphere(source.transform.position, range, 1 << playerMask);
        }

        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.ToString());

            // create a vector from the possible enemy to the attacker

            Vector3 enemyVector = collider.transform.position - source.transform.position;
            Vector3 enemyVector2 = source.transform.position - collider.transform.position;

            // this is an enemy attack, forward attack vector will be based on target position
            if (isPlayer == false)
            {
                forward = enemyVector;
            }

            // if the angle between the forward vector of the attacker and the enemy vector is less than the angle of attack, the enemy is within the attack angle
            if (Vector3.Angle(forward, enemyVector) < angle)
            {
                RaycastHit hit = new RaycastHit();
                

                if (isPlayer == true)
                {
                    // try to cast a ray from the enemy to the player
                    bool rayCastHit = CombatMath.RayCast(source.transform, collider.transform, out hit, range, ~(1 << enemyMask));

                    if (!rayCastHit)
                    {

                    }
                    // if the ray hits, the enemy is in line of sight of the player, this is a successful attack hit
                    else
                    {
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);
                            enemiesToAttack.Add(collider.gameObject);
                        }
                    }
                }

                else
                {
                    // try to cast a ray from the player to the enemy
                    bool rayCastHit = CombatMath.RayCast(source.transform, collider.transform, out hit, range, ~(1 << playerMask));

                    if (!rayCastHit)
                    {

                    }
                    // if the ray hits, the player is in line of sight of the enemy, this is a successful attack hit
                    else
                    {
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            //Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                            //Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);
                            enemiesToAttack.Add(collider.gameObject);
                        }
                    }
                }
            }
        }

        return enemiesToAttack;
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
        Debug.Log("damage: " + damageAmt);

        defender.ModifyHealth(-damageAmt);
    }

    public override void DoPhysics(GameObject source, GameObject target)
    {
        Vector3 relativeVector = (target.transform.position - source.transform.position).normalized;
        float normalizedMagnitude = 6f - Vector3.Distance(target.transform.position, source.transform.position);
        float force = (normalizedMagnitude / (Mathf.Pow(0.35f, 2)));
        //defender.GetComponent<MovementFSM>().Stop(0.17f);

        target.GetComponent<MovementFSM>().AddForce(relativeVector.normalized * force * 7, 0.15f);
    }

    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target = null)
    {
        GameObject particles;

        // if the player is casting the ability, we need to activate it based on the position of the cursor, not the transform's forward
        if (isPlayer == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit targetRay;
            Physics.Raycast(ray, out targetRay, Mathf.Infinity);
            Vector3 vectorToMouse = targetRay.point - source.transform.position;
            Vector3 cursorForward = new Vector3(vectorToMouse.x, source.transform.forward.y, vectorToMouse.z).normalized;


            Quaternion rotation = Quaternion.LookRotation(cursorForward);
            particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position, rotation);
        }

        else
        {
            particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position, source.transform.rotation);
        }

        //particles.transform.parent = attacker.transform;

        yield return new WaitForSeconds(time);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particleSystems)
        {
            Debug.Log("asd");
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;

        }

        GameObject.Destroy(particles);

        yield return null;
    }

}
