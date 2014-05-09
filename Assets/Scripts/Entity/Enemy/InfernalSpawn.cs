using UnityEngine;
using System.Collections;

public class InfernalSpawn : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {/*
        if (other.tag == "Player")
        {
            NavMeshHit navMeshHit;

            if (NavMesh.SamplePosition(other.transform.position, out navMeshHit, 20, 1 << LayerMask.NameToLayer("Default")))
            {
                GameObject infernoSpawn = (GameObject)GameObject.Instantiate(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().InfernalSpawn, navMeshHit.position, Quaternion.identity);
                infernoSpawn.GetComponent<Infernal>().Initialize(other.gameObject, true);
            }

            GameObject particles;

            particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position, source.transform.rotation);

            ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

            foreach (ParticleSystem item in particleSystems)
            {
                item.transform.parent = null;
                item.emissionRate = 0;
                item.enableEmission = false;

            }

            GameObject.Destroy(particles);
        }*/
    }
}
