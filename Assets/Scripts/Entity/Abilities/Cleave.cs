using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cleave : Ability
{
    public Cleave(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles, 0)
    {
       
    }

    // PBAoE AttackHandler
    public override void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {
        List<GameObject> attacked = OnAttack(source, isPlayer);

        if (isPlayer == true)
        {
            // this is player -> enemy
            foreach (GameObject enemy in attacked)
            {
                // if our target isn't dead or resetting
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    // get defender, do damage, put the target in combat if they weren't before
                    Entity defender = enemy.GetComponent<Entity>();
                    DoDamage(source, enemy, attacker, defender, isPlayer);
                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoHitAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OnHitNormalParticles, 0.2f, isPlayer, defender.gameObject));
                    // GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(defender.gameObject, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OnHitNormalParticles, 0.2f, isPlayer, defender.gameObject));

                    if (enemy.GetComponent<AIController>().IsInCombat() == false)
                    {
                        enemy.GetComponent<AIController>().BeenAttacked(source);
                    }
                    if(attacker.abilityManager.abilities[6] != null)
                    {
                        attacker.abilityManager.abilities[6].AttackHandler(attacker.gameObject, defender.gameObject, isPlayer);
                    }
                }
            }
        }

        else
        {
            // this is enemy -> player
            foreach (GameObject enemy in attacked)
            {
                Entity defender = enemy.GetComponent<Entity>();
                DoDamage(source, enemy, attacker, defender, isPlayer);
                if (attacker.abilityManager.abilities[6] != null)
                {
                    attacker.abilityManager.abilities[6].AttackHandler(attacker.gameObject, defender.gameObject, isPlayer);
                }
                
            }
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer));
    }

    public override List<GameObject> OnAttack(GameObject source, bool isPlayer)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = new Vector3();

        // if this is a player attack, forward attack vector will be based on cursor position
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

        // if this is a player attack, check for enemies in range
        if (isPlayer == true)
        {

            colliders = Physics.OverlapSphere(source.transform.position, range + source.GetComponent<MovementFSM>().Radius, 1 << enemyMask);

        }

         // if this is an enemy attack, check for players in range
        else
        {
            colliders = Physics.OverlapSphere(source.transform.position, range + source.GetComponent<MovementFSM>().Radius, 1 << playerMask);

        }

        foreach (Collider collider in colliders)
        {
            

            // create a vector from the possible enemy to the attacker
            Vector3 normalizedAttackPosition = new Vector3(source.transform.position.x, 1, source.transform.position.z);
            Vector3 normalizedDefenderPosition = new Vector3(collider.transform.position.x, 1, collider.transform.position.z);

            Vector3 enemyVector = normalizedDefenderPosition - normalizedAttackPosition;
            Vector3 enemyVector2 = normalizedAttackPosition - normalizedDefenderPosition;

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

                    //bool rayCastHit = Physics.Raycast(new Ray(normalizedDefenderPosition, enemyVector2), out hit, range + source.GetComponent<MovementFSM>().Radius, ~(1 << enemyMask));

                    bool rayCastHit = CombatMath.RayCast(source.transform, collider.transform, out hit, range, ~(1 << enemyMask));
                    Debug.DrawRay(normalizedDefenderPosition, enemyVector, Color.green, 0.5f);
                    Debug.DrawRay(normalizedDefenderPosition, enemyVector2, Color.red, 0.5f);
                    if (!rayCastHit)
                    {

                    }
                    // if the ray hits, the enemy is in line of sight of the player, this is a successful attack hit
                    else
                    {
                        if (hit.collider.gameObject.tag == "Player")
                        {

                            Debug.DrawRay(normalizedDefenderPosition, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(normalizedDefenderPosition, enemyVector2, Color.red, 0.5f);

                            enemiesToAttack.Add(collider.gameObject);
                        }
                    }
                }

                else
                {
                    // try to cast a ray from the player to the enemy

                    //bool rayCastHit = Physics.Raycast(new Ray(normalizedDefenderPosition, enemyVector2), out hit, range + source.GetComponent<MovementFSM>().Radius, ~(1 << playerMask));

                    bool rayCastHit = CombatMath.RayCast(source.transform, collider.transform, out hit, range, ~(1 << playerMask));

                    if (!rayCastHit)
                    {
                    }
                    // if the ray hits, the player is in line of sight of the enemy, this is a successful attack hit
                    else
                    {
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            //Debug.DrawRay(normalizedDefenderPosition, enemyVector, Color.green, 0.5f);
                            //Debug.DrawRay(normalizedDefenderPosition, enemyVector2, Color.red, 0.5f);

                            enemiesToAttack.Add(collider.gameObject);
                        }

                        else
                        {
                            Debug.Log("not something that we didn't hit");
                            Debug.Log(hit.collider.name);
                        }
                    }
                }
            }
        }

        return enemiesToAttack;
    }


    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {

        // run damage calc, apply damage
        float damageAmt;
        if (isPlayer == true)
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, damageMod);
        }
        else
        {
            damageAmt = DamageCalc.DamageCalculation(attacker, defender, 0);
        }

        defender.ModifyHealth(-damageAmt);

    }

    public IEnumerator DoHitAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target)
    {
        // instantiate particle system for the on-hit of this ability
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, CombatMath.GetCenter(target.transform), target.transform.rotation);
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

    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target = null)
    {
        GameObject particles;
        // instantiate particle system for the on-hit of this ability
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
            particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position + source.transform.forward * source.GetComponent<MovementFSM>().Radius * 0.5f, source.transform.rotation);

        }
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
