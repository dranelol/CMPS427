using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hadouken : Ability
{
    
    public Hadouken(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable)
        : base(attackType, damageType, range, angle, cooldown, damageMod, id, readable)
    {
       
    }
         
    /// <summary>
    /// Figure out who will be affected by this attack
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns>Returns a list of gameobjects this attack will affect</returns>
    public override List<GameObject> OnAttack(Transform attacker)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = attacker.forward.normalized;

        int enemyMask = LayerMask.NameToLayer("Enemy");
        int playerMask = LayerMask.NameToLayer("Player");

        Collider[] colliders = Physics.OverlapSphere(attacker.position, range, 1 << enemyMask);

        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.ToString());

            // create a vector from the possible enemy to the attacker

            Vector3 enemyVector = collider.transform.position - attacker.position;
            Vector3 enemyVector2 = attacker.position - collider.transform.position;

            // if the angle between the forward vector of the attacker and the enemy vector is less than the angle of attack, the enemy is within the attack angle
            if (Vector3.Angle(forward, enemyVector) < angle)
            {
                RaycastHit hit = new RaycastHit();
                Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);

                // try to cast a ray from the enemy to the player
                bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, range, 1 << playerMask);

                if (!rayCastHit)
                {

                }
                // if the ray hits, the enemy is in line of sight of the player, this is a successful attack hit
                else
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        enemiesToAttack.Add(collider.gameObject);
                    }
                }
            }
        }

        return enemiesToAttack;
    }

    /// <summary>
    /// Do damage with this attack
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>
    public override void DoDamage(GameObject attacker, GameObject defender)
    {

    }

    /// <summary>
    /// Certain attacks have a physics component to them; this resolves those effects
    /// </summary>
    /// <param name="attacker">Gameobject doing the attacking</param>
    /// <param name="defender">Gameobject affected by the attack</param>
    public override void DoPhysics(GameObject attacker, GameObject defende, AttackType attackType)
    {

    }
}
