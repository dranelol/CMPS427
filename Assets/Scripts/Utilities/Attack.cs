using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack
{
    /// <summary>
    /// Figure out which gameobjects will be affected by this attack
    /// </summary>
    /// <param name="attacker">The attacking gameobject</param>
    /// <param name="attackAngle">The angle of the attack</param>
    /// <param name="attackRange">The range of the attack (radius for arced attacks)</param>
    /// <param name="attackType">The type of attack</param>
    /// <param name="attackPosition">Optional: the position of the attack. By default, this is the attacker's position</param>
    /// <returns>The list containing the affected gameobjects</returns>
    public List<GameObject> OnAttack(Transform attacker, float attackAngle, float attackRange, AttackType attackType, Vector3 attackPosition = new Vector3())
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = attacker.forward.normalized;

        int enemyMask = LayerMask.NameToLayer("Enemy");
        int playerMask = LayerMask.NameToLayer("Player");

        switch(attackType)
        {
            case AttackType.PBAOE:
                #region Point-blank AoE
                {
                    // get a list of all the enemies in range of the attack
                    Collider[] colliders = Physics.OverlapSphere(attacker.position, attackRange, 1 << enemyMask);

                    foreach (Collider collider in colliders)
                    {
                        Debug.Log(collider.ToString());

                        // create a vector from the possible enemy to the attacker

                        Vector3 enemyVector = collider.transform.position - attacker.position;
                        Vector3 enemyVector2 = attacker.position - collider.transform.position;

                        // if the angle between the forward vector of the attacker and the enemy vector is less than the angle of attack, the enemy is within the attack angle
                        if (Vector3.Angle(forward, enemyVector) < attackAngle)
                        {
                            RaycastHit hit = new RaycastHit();
                            Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);

                            // try to cast a ray from the enemy to the player
                            bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, attackRange, 1 << playerMask);

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
                }
                #endregion

                break;

            case AttackType.AOE:
                break;

            // for now, melees are treated exactly the same as point-blank AoEs
            case AttackType.MELEE:
                #region Melee
                {
                    // get a list of all the enemies in range of the attack
                    Collider[] colliders = Physics.OverlapSphere(attacker.position, attackRange, 1 << enemyMask);

                    foreach (Collider collider in colliders)
                    {
                        Debug.Log(collider.ToString());

                        // create a vector from the possible enemy to the attacker

                        Vector3 enemyVector = collider.transform.position - attacker.position;
                        Vector3 enemyVector2 = attacker.position - collider.transform.position;

                        // if the angle between the forward vector of the attacker and the enemy vector is less than the angle of attack, the enemy is within the attack angle
                        if (Vector3.Angle(forward, enemyVector) < attackAngle)
                        {
                            RaycastHit hit = new RaycastHit();
                            Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);

                            // try to cast a ray from the enemy to the player
                            bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, attackRange, 1 << playerMask);

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
                }
                #endregion

                break;

            case AttackType.PROJECTILE:
                break;

            default:

                break;
        }
    






        return enemiesToAttack;
    }

    /// <summary>
    /// Do damage to an enemy
    /// </summary>
    /// <param name="attacker">Gameobject doing the attacking</param>
    /// <param name="defender">Gameobject affected by the attack</param>
    public void DoDamage(GameObject attacker, GameObject defender)
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
    public void DoPhysics(GameObject attacker, GameObject defender, AttackType attackType)
    {
    }
    /// <summary>
    /// Completely removes the velocity from a rigidbody
    /// Note: This is used in most of the force-based attacks
    /// </summary>
    /// <param name="target">Target rigid body from which you are removing velocity</param>
    /// <param name="time">Time, in seconds, after which veloctiy is removed. Default=0</param>
    /// <returns></returns>
    public IEnumerator RemovePhysics(Rigidbody target, float time=0.0f)
    {
        yield return new WaitForSeconds(time);

        if (target != null)
        {
            target.isKinematic = true;
        }

        yield break;
    }

}
