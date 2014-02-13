using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack
{
    public static List<GameObject> OnAttack(Transform attacker, float attackAngle, float attackRange)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = attacker.forward;

        Collider[] colliders = Physics.OverlapSphere(attacker.position, attackRange);

        foreach (Collider collider in colliders)
        {
            Vector3 enemyVector = (collider.transform.position - attacker.position).normalized;


        }






        return enemiesToAttack;
    }
}
