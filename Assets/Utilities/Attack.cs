using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack
{
    public static List<GameObject> OnAttack(Transform attacker, float attackAngle, float attackRange)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = attacker.forward.normalized;

        int enemyMask = LayerMask.NameToLayer("Enemy");
        int playerMask = LayerMask.NameToLayer("Player");

        Collider[] colliders = Physics.OverlapSphere(attacker.position, attackRange, 1 << enemyMask);

        foreach (Collider collider in colliders)
        {
            //Vector3 enemyVector = collider.transform.position - attacker.position;
            Vector3 enemyVector = collider.transform.position - attacker.position;
            //Debug.Log(enemyVector);
            //Debug.Log(Vector3.Angle(forward, enemyVector));
            
            if (Vector3.Angle(forward, enemyVector) < attackAngle)
            {
                //Debug.Log(collider.ToString());
                //Debug.Log(Vector3.Angle(forward, enemyVector));
                //Debug.Log(Vector3.Angle(forward, enemyVector).ToString());
                // draw ray between enemy and player
                // raycast with a layermask for enemies
                //Debug.Log("enemy in angle: " + Vector3.Angle(forward, enemyVector).ToString());
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(new Ray(collider.transform.position, enemyVector),out hit, attackRange, 1 << playerMask);

                Debug.Log("hit: " + hit.transform.ToString());
                
                /*
                if (hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("asd");
                }
                 */
                // if the first thing the raycast hits is the player, player do damage to enemy

                //Debug.Log(hit.ToString());
                //Debug.Log(hit.collider.tag);
                //Debug.Log(hit.collider.gameObject.tag);
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
