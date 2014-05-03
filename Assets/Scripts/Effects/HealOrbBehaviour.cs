using UnityEngine;
using System.Collections;

public class HealOrbBehaviour : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Entity playerEntity = other.gameObject.GetComponent<Entity>();

            playerEntity.ModifyHealthPercentage(20.0f);

            Destroy(gameObject);
        }
    }
}
