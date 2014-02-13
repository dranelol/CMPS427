using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack
{
    public static List<GameObject> OnAttack(Transform attacker, float attackAngle, float attackRange)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = attacker.forward.normalized;

        Collider[] colliders = Physics.OverlapSphere(attacker.position, attackRange);

        foreach (Collider collider in colliders)
        {
            Vector3 enemyVector = (collider.transform.position - attacker.position).normalized;

            if (Vector3.AngleBetween(forward, enemyVector) < attackAngle)
            {
                // draw ray between enemy and player
                // raycast with a layermask for enemies

            }
        }






        return enemiesToAttack;
    }
}
