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
        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(source.transform.position, out navMeshHit, range, 1 << LayerMask.NameToLayer("Default")))
        {
            GameObject infernoSpawn = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().InfernalSpawn, navMeshHit.position, Quaternion.identity);
            infernoSpawn.GetComponent<Infernal>().Initialize(attacker.gameObject);
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.2f, isPlayer));
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
