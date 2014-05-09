using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dervish : Ability
{
    public Dervish(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }

    public override void AttackHandler(GameObject source, Vector3 AoEPoint, Entity attacker, bool isPlayer)
    {
        // do attack "repetition" times with "timeDelta" waiting between each
        Debug.Log("attackhandler?");

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoSpawnAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DervishSpawn, 2.0f, isPlayer, 1.5f));
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoSpawnAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DervishSpawn, 2.0f, isPlayer, 2.0f));
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoSpawnAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DervishSpawn, 2.0f, isPlayer, 2.5f));
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoSpawnAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DervishSpawn, 2.0f, isPlayer, 3.0f));
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoSpawnAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DervishSpawn, 2.0f, isPlayer, 3.5f));
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoSpawnAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DervishSpawn, 2.0f, isPlayer, 4.0f));
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoSpawnAnimation(source, GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DervishSpawn, 2.0f, isPlayer, 4.5f));
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAttackRepeating(source, attacker, isPlayer, 6, 0.25f));
        DoBuff(source, attacker);


        int tempindex = 10;
        while (attacker.abilityManager.abilities[tempindex] != null && attacker.abilityManager.abilities[tempindex].ID != "icebolt")
        {
            tempindex++;
        }
        if (attacker.abilityManager.abilities[tempindex] == null)
        {
            attacker.abilityManager.AddAbility(GameManager.Abilities["dervishdeathgrip"], tempindex);
            attacker.abilityIndexDict["dervishdeathgrip"] = tempindex;

        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(randomdeathgrip(source, attacker, isPlayer, 6, 0.25f,tempindex));
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
                    // try to cast a ray from the enemy to the player
                    bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, range, ~(1 << enemyMask));

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
                    bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, range, ~(1 << playerMask));

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
        float normalizedMagnitude = 5f - Vector3.Distance(target.transform.position, source.transform.position);
        float force = (normalizedMagnitude / (Mathf.Pow(0.4f, 2)));

        target.GetComponent<MovementFSM>().AddForce(relativeVector.normalized * force * 2, 0.2f);
    }

    /// <summary>
    /// Adds the buff to the enemy. Pretty straightforward
    /// 
    /// </summary>
    /// <param name="target">the gameobject of the target</param>
    /// <param name="source">the entity that is applying the buff/debuff</param>
    public void DoBuff(GameObject target, Entity source)
    {
        target.GetComponent<EntityAuraManager>().Add("dervishslow", source);

    }

    public IEnumerator DoAttackRepeating(GameObject source, Entity attacker, bool isPlayer, int repetitions, float waitDelta)
    {
        for (int i = 0; i < repetitions; i++)
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
                            enemy.GetComponent<AIController>().BeenAttacked(source);
                        }

                        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer, defender.gameObject));
                    }
                }
            }

            else
            {
                foreach (GameObject enemy in attacked)
                {
                    Entity defender = enemy.GetComponent<Entity>();
                    DoDamage(source, enemy, attacker, defender, isPlayer);

                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer, defender.gameObject));

                }
            }

            yield return new WaitForSeconds(waitDelta);
        }

        yield return null;
    }

    public IEnumerator randomdeathgrip(GameObject source, Entity attacker, bool isPlayer, int repetitions, float waitdelta, int abilityindex)
    {


        for (int i = 0; i < repetitions; i++)
        {

            Vector3 randdirection = Random.insideUnitSphere;
            randdirection.y = 0;
            randdirection.Normalize();

            attacker.abilityManager.abilities[abilityindex].AttackHandler(source, attacker, isPlayer);


            yield return new WaitForSeconds(waitdelta);
        }



        yield return null;
    }

    public override IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, GameObject target)
    {
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, target.transform.position, target.transform.rotation);

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

    public IEnumerator DoSpawnAnimation(GameObject source, GameObject particlePrefab, float time, bool isPlayer, float orbitScale, GameObject target = null)
    {
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position+new Vector3(0,1,0), source.transform.rotation);
  
        particles.GetComponentInChildren<OrbSpawnSingle>().orbitObject = source;
        particles.GetComponentInChildren<OrbSpawnSingle>().orbitScale = orbitScale;
        particles.GetComponentInChildren<OrbSpawnSingle>().minHeight = 1;
        particles.GetComponentInChildren<OrbSpawnSingle>().maxHeight = 1;

        GameObject particles2 = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position + new Vector3(0, 1, 0), source.transform.rotation);

        particles2.GetComponentInChildren<OrbSpawnSingle>().orbitObject = source;
        particles2.GetComponentInChildren<OrbSpawnSingle>().orbitScale = orbitScale;
        particles2.GetComponentInChildren<OrbSpawnSingle>().initialAngleFromForward = 180;
        particles2.GetComponentInChildren<OrbSpawnSingle>().minHeight = 1;
        particles2.GetComponentInChildren<OrbSpawnSingle>().maxHeight = 1;

        yield return new WaitForSeconds(time);

        GameObject.Destroy(particles);
        GameObject.Destroy(particles2);

        yield return null;
    }

}