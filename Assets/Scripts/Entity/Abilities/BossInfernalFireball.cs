using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossInfernalFireball : Ability
{
    public BossInfernalFireball(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }


    public override void SpawnProjectile(GameObject source, Vector3 target, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {

        GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().BossInfernalFireballProjectile, source.transform.position + new Vector3(0,10.0f,0), Quaternion.LookRotation(forward));
        Debug.Log("shootin dat infernal");
        projectile.GetComponent<ProjectileBehaviour>().owner = owner;
        projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 10.0f;
        projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;
        projectile.GetComponent<ProjectileBehaviour>().target = target;
        projectile.GetComponent<ProjectileBehaviour>().CollidesWithTerrain = true;
        projectile.GetComponent<ProjectileBehaviour>().AOEOnExplode = true;
        projectile.GetComponent<ProjectileBehaviour>().speed = 5f;

        //projectile.rigidbody.velocity = forward;

        Vector3 direction = (target - projectile.transform.position).normalized;

        projectile.transform.rotation = Quaternion.LookRotation(direction);
    }

    public override void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {
        List<GameObject> attacked = OnAttack(source, isPlayer);

        if (isPlayer == true)
        {
            Debug.Log(attacked.Count);
            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    Entity defender = enemy.GetComponent<Entity>();
                    DoDamage(source, enemy, attacker, defender, isPlayer);
                    if (enemy.GetComponent<AIController>().IsInCombat() == false)
                    {
                        enemy.GetComponent<AIController>().BeenAttacked(attacker.gameObject);
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

            }
        }

        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(source.transform.position, out navMeshHit, range, 1 << LayerMask.NameToLayer("Default")))
        {
            GameObject infernoSpawn = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().InfernalSpawn, navMeshHit.position, Quaternion.identity);
            infernoSpawn.GetComponent<Infernal>().Initialize(attacker.gameObject);
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

        Debug.Log("colliders gained: " + colliders.Length);
        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.ToString());

            // create a vector from the possible enemy to the source.transform

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
                    // try to cast a ray from the enemy to the original position
                    bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, range, ~(1 << enemyMask));

                    if (!rayCastHit)
                    {
                        Debug.Log("no hit, good");
                        Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                        Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);
                        enemiesToAttack.Add(collider.gameObject);
                    }
                }

                else
                {
                    // try to cast a ray from the player to the original position
                    bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, ~(1 << playerMask));

                    if (!rayCastHit)
                    {
                        enemiesToAttack.Add(collider.gameObject);
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
        if (isPlayer == true)
        {
            Debug.Log("damage: " + damageAmt);
        }

        defender.ModifyHealth(-damageAmt);
    }

    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target = null)
    {
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position, source.transform.rotation);

        yield return new WaitForSeconds(time);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;

        }

        GameObject.Destroy(particles);

        yield return null;
    }

}
