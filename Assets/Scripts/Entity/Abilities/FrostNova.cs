using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrostNova : Ability
{
    public FrostNova(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }


    public override void SpawnProjectile(GameObject source, GameObject owner, Vector3 forward, string abilityID, bool isPlayer)
    {
        /*
        int segments = 8;

        for (int i = 0; i < segments; i++)
        {

            GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IceBoltProjectile, source.transform.position + Rotations.RotateAboutY(forward, (360 / segments) * i) * 1, Quaternion.LookRotation(Rotations.RotateAboutY(forward, (360 / segments) * i)));

            projectile.GetComponent<ProjectileBehaviour>().owner = owner;
            projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 3.0f;
            projectile.GetComponent<ProjectileBehaviour>().abilityID = abilityID;
            projectile.GetComponent<ProjectileBehaviour>().CollidesWithTerrain = false;

            projectile.rigidbody.velocity = Rotations.RotateAboutY(forward, (360 / segments) * i) * 20.0f;
        }
        */
    }

    public override void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {
        Debug.Log("in attackhandler");
        /*
        Vector3 forward = Vector3.zero;

        // if its a player, attack based on mouse
        if (isPlayer == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayCastTarget;
            Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
            Vector3 vectorToMouse = rayCastTarget.point - source.transform.position;
            forward = new Vector3(vectorToMouse.x, source.transform.forward.y, vectorToMouse.z).normalized;
        }

        // if its an enemy, attack based on forward vector
        else
        {
            forward = source.transform.forward;
        }
         


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
        */

        int tempindex = 10;
        while (attacker.abilityManager.abilities[tempindex] != null &&attacker.abilityManager.abilities[tempindex].ID != "icebolt")
        {
            tempindex++;
        }
        if (attacker.abilityManager.abilities[tempindex] == null)
        {
            attacker.abilityManager.AddAbility(GameManager.Abilities["icebolt"], tempindex);
            attacker.abilityIndexDict["icebolt"] = tempindex;
            Debug.Log("icebolt added to " + tempindex);
        }


        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(launch(source, source, tempindex, isPlayer));
    
    }

    public IEnumerator launch(GameObject source, GameObject owner, int tempindex, bool isplayer)
    {


        int segments = 16;
        for (int i = 0; i < 32; i++)
        {

            //int segments = 8;

            //for (int i = 0; i < segments; i++)
            //{

                GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IceBoltProjectile, owner.transform.position + Rotations.RotateAboutY(owner.transform.forward, (360 / segments) * i) * 1 + Vector3.up, Quaternion.LookRotation(Rotations.RotateAboutY(owner.transform.forward, (360 / segments) * i)));

                projectile.GetComponent<ProjectileBehaviour>().owner = owner;
                projectile.GetComponent<ProjectileBehaviour>().timeToActivate = .5f;
                projectile.GetComponent<ProjectileBehaviour>().abilityID = "icebolt";
                projectile.GetComponent<ProjectileBehaviour>().CollidesWithTerrain = false;

                projectile.rigidbody.velocity = Rotations.RotateAboutY(owner.transform.forward, (360 / segments) * i) * 10.0f;
            //}

            //Vector3 forward = new Vector3(Random.Range(-1.0f, 1.0f), owner.GetComponent<Entity>().transform.forward.y, Random.Range(-1.0f, 1.0f)).normalized;
            //owner.GetComponent<Entity>().abilityManager.abilities[tempindex].SpawnProjectile(source, owner, forward, owner.GetComponent<Entity>().abilityManager.abilities[tempindex].ID, isplayer);
            //SpawnProjectile(SourceEntity.gameObject, rayCastTarget.point, SourceEntity.gameObject, forward, SourceEntity.abilityManager.abilities[tempindex].ID, true);
            yield return new WaitForSeconds(.05f);
        }


        yield return null;
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
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, target.transform.position, source.transform.rotation);

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
