using UnityEngine;
using System.Collections;

public class SpawnOnDestroy : MonoBehaviour {
    public GameObject spawnThing;
    public bool particleCleanup;

    void OnDestroy()
    {
        if (particleCleanup == true)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(DoAnimation(spawnThing, 0.5f));
        }

        else
        {
            GameObject.Instantiate(spawnThing, transform.position, Quaternion.identity);
        }
        
    }

    private IEnumerator DoAnimation(GameObject particlePrefab, float time)
    {
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, transform.position, transform.rotation);

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
