using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealOrb : Ability
{
    public HealOrb(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable, GameObject particles)
        : base(attackType, damageType, range, angle, cooldown, damageMod, id, readable, particles)
    {

    }

    public override void AttackHandler(GameObject source, GameObject target, Entity attacker, bool isPlayer)
    {
        List<GameObject> attacked = OnAttack(target, isPlayer);

        Debug.Log(attacked.Count);
        foreach (GameObject enemy in attacked)
        {
            if (enemy.GetComponent<AIController>().IsResetting() == false
                && enemy.GetComponent<AIController>().IsDead() == false)
            {
                Entity defender = enemy.GetComponent<Entity>();
                DoDamage(source, enemy, attacker, defender, isPlayer);
                DoPhysics(target, enemy);
            }
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(source, particleSystem, 0.0f, isPlayer, target));
        
    }

    public override void DoDamage(GameObject source, GameObject target, Entity attacker, Entity defender, bool isPlayer)
    {
        // heal every player

        float healAmt = 100.0f;

        defender.ModifyHealth(healAmt);
    }

    public override List<GameObject> OnAttack(GameObject source, bool isPlayer)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = new Vector3();

        int playerMask = LayerMask.NameToLayer("Player");

        Collider[] colliders = Physics.OverlapSphere(source.transform.position, range, 1 << playerMask);
        
        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.ToString());

            // create a vector from the possible enemy to the source.transform

            Vector3 enemyVector = collider.transform.position - source.transform.position;
            Vector3 enemyVector2 = source.transform.position - collider.transform.position;

            // if the angle between the forward vector of the attacker and the enemy vector is less than the angle of attack, the enemy is within the attack angle
            if (Vector3.Angle(forward, enemyVector) < angle)
            {
                RaycastHit hit = new RaycastHit();

                // try to cast a ray from the enemy to the player
                bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, range);

                if (!rayCastHit)
                {

                }
                // if the ray hits, the enemy is in line of sight of the player, this is a successful attack hit
                else
                {
                    Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                    Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);
                    enemiesToAttack.Add(collider.gameObject);
                }
            }
        }
        enemiesToAttack.Add(source);
        return enemiesToAttack;
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
