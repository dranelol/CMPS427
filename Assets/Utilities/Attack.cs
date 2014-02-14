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

            if (Vector3.Angle(forward, enemyVector) < attackAngle)
            {
                Debug.Log(Vector3.Angle(forward, enemyVector).ToString());
                // draw ray between enemy and player
                // raycast with a layermask for enemies
                RaycastHit hit;
                int enemyMask = LayerMask.NameToLayer("Enemy");
                Physics.Raycast(new Ray(collider.transform.position, enemyVector),out hit, attackRange, enemyMask);

                // if the first thing the raycast hits is the player, player do damage to enemy

                Debug.Log(hit.ToString());
                
                /*
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("fucked him up!");
                }
                */
            }
        }






        return enemiesToAttack;
    }
}
