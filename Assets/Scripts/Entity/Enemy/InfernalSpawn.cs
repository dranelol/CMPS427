using UnityEngine;
using System.Collections;

public class InfernalSpawn : MonoBehaviour 
{
    public GameObject target;
    public GameObject prefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<SphereCollider>().enabled = false;
            target = other.gameObject;

            GameObject projectile = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().BossInfernalFireballProjectile, transform.position + new Vector3(0, 10.0f, 0), Quaternion.LookRotation((new Vector3(0, -10f, 0)).normalized));

            projectile.GetComponent<ProjectileBehaviour>().owner = gameObject;
            projectile.GetComponent<ProjectileBehaviour>().timeToActivate = 10.0f;
            projectile.GetComponent<ProjectileBehaviour>().target = transform.position;
            projectile.GetComponent<ProjectileBehaviour>().CollidesWithTerrain = true;
            projectile.GetComponent<ProjectileBehaviour>().AOEOnExplode = true;
            projectile.GetComponent<ProjectileBehaviour>().EnvironmentProjectile = true;
            projectile.GetComponent<ProjectileBehaviour>().speed = 5f;

            Vector3 direction = (transform.position - projectile.transform.position).normalized;

            projectile.transform.rotation = Quaternion.LookRotation(direction);

            Invoke("DoItBro", 2f);
        }
    }

    private void DoItBro()
    {
        GameObject particles = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().BossInfernalFireballExplosion, transform.position, Quaternion.identity);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;
        }

        GameObject.Destroy(particles);

        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(transform.position, out navMeshHit, 20, 1 << LayerMask.NameToLayer("Default")))
        {
            GameObject infernoSpawn = (GameObject)GameObject.Instantiate(prefab, navMeshHit.position, Quaternion.identity);
            infernoSpawn.transform.parent = transform;
            infernoSpawn.GetComponent<BossInfernal>().Initialize(target);
        }  
    }
}
