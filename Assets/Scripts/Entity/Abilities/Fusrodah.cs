using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fusrodah : Ability
{
    public Fusrodah(AttackType attackType, DamageType damageType, float range, float angle, float cooldown, float damageMod, string id, string readable)
        : base(attackType, damageType, range, angle, cooldown, damageMod, id, readable)
    {
       
    }

    /// <summary>
    /// Handler for this attack; figures out who will be attacked, and carries out everything needed for the attack to occur
    /// </summary>
    /// <param name="attacker">The gameobject carrying out the attack</param>
    /// <param name="defender">The gameobject defending against the attack</param>                                                  //swag
    public override void AttackHandler(GameObject attacker, bool isPlayer, GameObject particleAnimation)
    {
        List<GameObject> attacked = OnAttack(attacker.transform, isPlayer);

        if (isPlayer == true)
        {

            foreach (GameObject enemy in attacked)
            {
                if (enemy.GetComponent<AIController>().IsResetting() == false
                    && enemy.GetComponent<AIController>().IsDead() == false)
                {
                    DoDamage(attacker, enemy, isPlayer);

                    // this is a physics attack, so do physics applies
                    DoPhysics(attacker, enemy);
                }
            }
        }

        else
        {
            foreach (GameObject enemy in attacked)
            {
                DoDamage(attacker, enemy, isPlayer);

                // this is a physics attack, so do physics applies
                DoPhysics(attacker, enemy);
            }
        }

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunParticleSystem(DoAnimation(attacker, particleAnimation, 0.2f));
    }

    /// <summary>
    /// Figure out who will be affected by this attack
    /// </summary>
    /// <param name="attacker"></param>
    /// <returns>Returns a list of gameobjects this attack will affect</returns>
    public override List<GameObject> OnAttack(Transform attacker, bool isPlayer)
    {
        List<GameObject> enemiesToAttack = new List<GameObject>();

        Vector3 forward = new Vector3();

        // this is a player attack, forward attack vector will be based on cursor position
        if (isPlayer == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit target;
            Physics.Raycast(ray, out target, Mathf.Infinity);
            Vector3 vectorToMouse = target.point - attacker.position;
            forward = new Vector3(vectorToMouse.x, attacker.forward.y, vectorToMouse.z).normalized;
        }

        int enemyMask = LayerMask.NameToLayer("Enemy");
        int playerMask = LayerMask.NameToLayer("Player");

        Collider[] colliders;

        if (isPlayer == true)
        {
            colliders = Physics.OverlapSphere(attacker.position, range, 1 << enemyMask);
        }

        else
        {
            colliders = Physics.OverlapSphere(attacker.position, range, 1 << playerMask);
        }

        foreach (Collider collider in colliders)
        {
            //Debug.Log(collider.ToString());

            // create a vector from the possible enemy to the attacker

            Vector3 enemyVector = collider.transform.position - attacker.position;
            Vector3 enemyVector2 = attacker.position - collider.transform.position;

            // this is an enemy attack, forward attack vector will be based on target position
            if (isPlayer == false)
            {
                forward = enemyVector;
            }

            // if the angle between the forward vector of the attacker and the enemy vector is less than the angle of attack, the enemy is within the attack angle
            if (Vector3.Angle(forward, enemyVector) < angle)
            {
                RaycastHit hit = new RaycastHit();
                

                if (isPlayer == true)
                {
                    // try to cast a ray from the enemy to the player
                    bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, range);

                    if (!rayCastHit)
                    {

                    }
                    // if the ray hits, the enemy is in line of sight of the player, this is a successful attack hit
                    else
                    {
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);
                            enemiesToAttack.Add(collider.gameObject);
                        }
                    }
                }

                else
                {
                    // try to cast a ray from the player to the enemy
                    bool rayCastHit = Physics.Raycast(new Ray(collider.transform.position, enemyVector2), out hit, range);

                    if (!rayCastHit)
                    {

                    }
                    // if the ray hits, the player is in line of sight of the enemy, this is a successful attack hit
                    else
                    {
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            Debug.DrawRay(collider.transform.position, enemyVector, Color.green, 0.5f);
                            Debug.DrawRay(collider.transform.position, enemyVector2, Color.red, 0.5f);
                            enemiesToAttack.Add(collider.gameObject);
                        }
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
    public override void DoDamage(GameObject attacker, GameObject defender, bool isPlayer)
    {
        //Debug.Log(defender.ToString());
        Entity attackerEntity = attacker.GetComponent<Entity>();
        Entity defenderEntity = defender.GetComponent<Entity>();

        // for now, always just take 10hp off

        defenderEntity.currentHP -= 10f;

        float ratio = (defenderEntity.currentHP / defenderEntity.maxHP);

        if (isPlayer == true)
        {
            defender.renderer.material.color = new Color(1.0f, ratio, ratio);
        }
    }

    /// <summary>
    /// Certain attacks have a physics component to them; this resolves those effects
    /// </summary>
    /// <param name="attacker">Gameobject doing the attacking</param>
    /// <param name="defender">Gameobject affected by the attack</param>
    public override void DoPhysics(GameObject attacker, GameObject defender)
    {
        Vector3 relativeVector = (defender.transform.position - attacker.transform.position).normalized;
        float normalizedMagnitude = 6f - Vector3.Distance(defender.transform.position, attacker.transform.position);
        float force = (normalizedMagnitude / (Mathf.Pow(0.35f, 2)));
        //defender.GetComponent<MovementFSM>().Stop(0.17f);

        defender.GetComponent<MovementFSM>().AddForce(relativeVector.normalized * force * 2, 0.2f, ForceMode.Impulse);
    }

    /// <summary>
    /// Certain attacks have an animation associated with them; this resolves those effects
    /// </summary>                                                                                              //yolo
    /// <param name="attacker">Gameobject doing the attacking</param>
    /// <param name="defender">Gameobject affected by the attack; default null if the attack only has an animation for the attacker</param>
    public override IEnumerator DoAnimation(GameObject attacker, GameObject source, float time, GameObject defender = null)
    {
        GameObject particles = (GameObject)GameObject.Instantiate(source, attacker.transform.position, attacker.transform.rotation);

        particles.transform.parent = attacker.transform;

        yield return new WaitForSeconds(time);

        GameObject.Destroy(particles);
        
        yield return null;
    }

}
