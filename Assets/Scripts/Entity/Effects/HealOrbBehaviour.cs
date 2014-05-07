using UnityEngine;
using System.Collections;

public class HealOrbBehaviour : MonoBehaviour 
{
    public GameObject explosionPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Entity playerEntity = other.gameObject.GetComponent<Entity>();

            playerEntity.ModifyHealthPercentage(10.0f);

            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(explosionPrefab, 0.5f));

            Destroy(gameObject);
        }
    }

    private IEnumerator DoAnimation(GameObject particlePrefab, float time)
    {
        GameObject particles = (GameObject)GameObject.Instantiate(particlePrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(time);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            item.enableEmission = false;

        }

        GameObject.Destroy(particles);

        yield return null;
    }
}
