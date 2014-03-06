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
    /// Handler for this attack; figures out who will be attacked, and carries out everything needed for the attack to occur
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>
    public override void AttackHandler(GameObject attacker)
    {
        List<GameObject> attacked = OnAttack(attacker.transform);

        Debug.Log(attacked.Count);

        foreach (GameObject enemy in attacked)
        {
            if (enemy.GetComponent<AIController>().IsResetting() == false
                && enemy.GetComponent<AIController>().IsDead() == false)
            {
                DoDamage(attacker, enemy);

                // this is a physics attack, so do physics applies
                DoPhysics(attacker, enemy);
            }
        }
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
        Debug.Log(defender.ToString());
        Entity attackerEntity = attacker.GetComponent<Entity>();
        Entity defenderEntity = defender.GetComponent<Entity>();

        // for now, always just take 10hp off

        defenderEntity.currentHP -= 10f;

        float ratio = (defenderEntity.currentHP / defenderEntity.maxHP);

        defender.renderer.material.color = new Color(1.0f, ratio, ratio);
    }

    /// <summary>
    /// Certain attacks have a physics component to them; this resolves those effects
    /// </summary>
    /// <param name="attacker">Gameobject doing the attacking</param>
    /// <param name="defender">Gameobject affected by the attack</param>
    public override void DoPhysics(GameObject attacker, GameObject defender)
    {
        Vector3 relativeVector = (defender.transform.position - attacker.transform.position);
        float normalizedMagnitude = 5f - Vector3.Distance(defender.transform.position, attacker.transform.position);
        float force = (normalizedMagnitude / (Mathf.Pow(0.4f, 2)));
        defender.GetComponent<MovementFSM>().Stop(0.2f);
        defender.rigidbody.isKinematic = false;
        defender.rigidbody.AddForce(relativeVector.normalized * force, ForceMode.Impulse);

        StartCoroutine(Attack.RemovePhysics(defender.rigidbody, 0.2f));
        
    }
}
