using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireballTurret : Ability
{
    public FireballTurret(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }

    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {
        int segments = 1;
        GameObject projectile = (GameObject)GameObject.Instantiate(particleSystem, source.transform.position + new Vector3(0,1,0), source.transform.rotation);


        projectile.GetComponent<ProjectileBehaviour>().owner = owner;
        projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 12.0f;
        projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;
        projectile.GetComponent<ProjectileBehaviour>().ExplodesOnTimeout = false;
        projectile.GetComponent<ProjectileBehaviour>().hasCollided = true;
        

        //projectile.rigidbody.velocity = Vector3.zero;


        int tempindex = 10;
        while (owner.GetComponent<Entity>().abilityManager.abilities[tempindex] != null && owner.GetComponent<Entity>().abilityManager.abilities[tempindex].ID != "fireball")
        {
            tempindex++;
        }

        if (owner.GetComponent<Entity>().abilityManager.abilities[tempindex] == null)
        {
            owner.GetComponent<Entity>().abilityManager.AddAbility(GameManager.Abilities["fireball"], tempindex);
            owner.GetComponent<Entity>().abilityIndexDict["fireball"] = tempindex;
            Debug.Log("fireball added to " + tempindex);
        }

        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
        vectorToMouse = rayCastTarget.point - SourceEntity.transform.position;
        forward = new Vector3(vectorToMouse.x, SourceEntity.transform.forward.y, vectorToMouse.z).normalized;
        */

    
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(launch(projectile, owner, tempindex, isPlayer));
    }

    public IEnumerator launch(GameObject source, GameObject owner, int tempindex, bool isplayer)
    {
        for (int i = 0; i < 12; i++)
        {
            List<GameObject> target;
            target = OnAttack(source, isplayer);
            Debug.Log("attacking");

            foreach (GameObject enemy in target)
            {
                Vector3 forward = (enemy.transform.position - source.transform.position).normalized;
                
                owner.GetComponent<Entity>().abilityManager.abilities[tempindex].SpawnProjectile(source, owner, forward, owner.GetComponent<Entity>().abilityManager.abilities[tempindex].ID, isplayer);
                  
            }



            //Vector3 forward = new Vector3(Random.Range(-1.0f, 1.0f), owner.GetComponent<Entity>().transform.forward.y, Random.Range(-1.0f, 1.0f)).normalized;
            //owner.GetComponent<Entity>().abilityManager.abilities[tempindex].SpawnProjectile(source, owner, forward, owner.GetComponent<Entity>().abilityManager.abilities[tempindex].ID, isplayer);
            //SpawnProjectile(SourceEntity.gameObject, rayCastTarget.point, SourceEntity.gameObject, forward, SourceEntity.abilityManager.abilities[tempindex].ID, true);
            yield return new WaitForSeconds(1.0f);
        }


        yield return null;
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
                    // try to cast a ray from the enemy to the turret

                    bool rayCastHit = CombatMath.RayCast(source.transform, collider.transform, out hit, range, ~(1 << enemyMask));
                    //bool rayCastHit = Physics.Raycast(new Ray(normalizedDefenderPosition, enemyVector2), out hit, range, ~(1 << enemyMask));
                    Debug.DrawRay(normalizedDefenderPosition, enemyVector2, Color.red, 0.5f);
                    if (!rayCastHit)
                    {

                    }
                    // if the ray hits, the enemy is in line of sight of the player, this is a successful attack hit
                    else
                    {
                        Debug.Log("hit: " + hit.collider.gameObject.name.ToString());
                        if (hit.collider.gameObject.tag == "Turret")
                        {
                            Debug.DrawRay(normalizedDefenderPosition, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(normalizedDefenderPosition, enemyVector2, Color.red, 0.5f);

                            enemiesToAttack.Add(collider.gameObject);
                        }
                    }
                }

                else
                {
                    // try to cast a ray from the player to the turret

                    bool rayCastHit = CombatMath.RayCast(source.transform, collider.transform, out hit, range, ~(1 << playerMask));
                    //bool rayCastHit = Physics.Raycast(new Ray(normalizedDefenderPosition, enemyVector2), out hit, range, ~(1 << playerMask));

                    if (!rayCastHit)
                    {
                        Debug.Log("fail");
                    }
                    // if the ray hits, the player is in line of sight of the enemy, this is a successful attack hit
                    else
                    {
                        if (hit.collider.gameObject.tag == "Turret")
                        {
                            Debug.DrawRay(normalizedDefenderPosition, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(normalizedDefenderPosition, enemyVector2, Color.red, 0.5f);

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
        List<GameObject> enemytoAttack = new List<GameObject>();

        if (enemiesToAttack.Count > 0)
        {
            enemytoAttack.Add(enemiesToAttack[Random.Range(0, enemiesToAttack.Count)]);
        }

        Debug.Log("enemies found: " + enemiesToAttack.Count.ToString());
        return enemytoAttack;

    }
}
