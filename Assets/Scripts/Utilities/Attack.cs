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
            //Debug.Log(collider.ToString());
            Vector3 enemyVector = collider.transform.position - attacker.position;
            Vector3 enemyVector2 = attacker.position - collider.transform.position;
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
                Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);




                bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2),out hit, attackRange, 1 << playerMask);
                
                if (hit.collider.gameObject.tag == "Player")
                {
                    //Debug.Log("dat hit!");
                    enemiesToAttack.Add(collider.gameObject);
                }
                 
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

    public static void DoDamage(GameObject attacker, GameObject defender)
    {
        Entity attackerEntity = attacker.GetComponent<Entity>();
        Entity defenderEntity = defender.GetComponent<Entity>();

        // for now, always just take 10hp off

        defenderEntity.currentHP -= 10f;

        float ratio = (defenderEntity.currentHP / defenderEntity.maxHP);

        defender.renderer.material.color = new Color(1.0f, ratio, ratio);


    }


}
