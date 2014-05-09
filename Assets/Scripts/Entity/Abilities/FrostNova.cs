using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FrostNova : Ability
{
    public FrostNova(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, float resourceCost, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, resourceCost, id, readable, particles)
    {

    }


    public override void AttackHandler(GameObject source, Entity attacker, bool isPlayer)
    {
        Debug.Log("in attackhandler");
        
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
            GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().IceBoltProjectile, owner.transform.position + Rotations.RotateAboutY(owner.transform.forward, (360 / segments) * i) * 1 + Vector3.up, Quaternion.LookRotation(Rotations.RotateAboutY(owner.transform.forward, (360 / segments) * i)));

            projectile.GetComponent<ProjectileBehaviour>().owner = owner;
            projectile.GetComponent<ProjectileBehaviour>().timeToActivate = .5f;
            projectile.GetComponent<ProjectileBehaviour>().abilityID = "icebolt";
            projectile.GetComponent<ProjectileBehaviour>().CollidesWithTerrain = false;

            projectile.rigidbody.velocity = Rotations.RotateAboutY(owner.transform.forward, (360 / segments) * i) * 10.0f;
           
            yield return new WaitForSeconds(.05f);
        }


        yield return null;
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
