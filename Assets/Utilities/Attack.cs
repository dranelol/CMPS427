using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack
{
    /// <summary>
    /// Returns a list of enemy gameobjects caught in a valid attack.
    /// </summary>
    /// <param name="attacker">Transform of the attacker</param>
    /// <param name="attackAngle">Angle of attack, in degrees</param>
    /// <param name="attackRadius">Range of attack</param>
    /// <returns></returns>
    public static List<GameObject> OnAttack(Transform attacker, float attackAngle, float attackRadius)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        // first, collide a sphere around the attack position, grabbing all objects we collided with
        Collider[] colliders = Physics.OverlapSphere(attacker.position, attackRadius);

        //forward facing of the attacker
        Vector3 forward = attacker.forward.normalized;

        // loop through these colliders
        foreach(Collider collider in colliders)
        {
            // only care about enemies
            if (collider.gameObject.tag == "Enemy")
            {
                // compute vector between attacker and enemy
                Vector3 enemyVector = (collider.transform.position - attacker.transform.position).normalized;
                // compute angle between enemy vector and attacker forward vector
                float angle = Vector3.Angle(forward, enemyVector);
                // if angle <= attackAngle / 2, enemy is within player's attack radius
                Debug.Log(angle.ToString());
                if (angle <= attackAngle / 2)
                {
                    // cast a ray between player and enemy
                    RaycastHit hit;

                    Physics.Raycast(new Ray(attacker.transform.position, forward), out hit);
                    // if the first thing hit was the enemy, add to enemiesToAttack
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        enemiesToAttack.Add(hit.collider.gameObject);
                    }
                }
            }
        }

        return enemiesToAttack;
    }
}
