using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsTest : MonoBehaviour 
{
    GameManager gameManager;
    PlayerEntity entity;
    List<Ability> spellBook;
    int counter = 0;

    public GUIText spell;
    
	// Use this for initialization
	void Start () 
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        entity = GetComponent<PlayerEntity>();
        spellBook = new List<Ability>();

        spellBook = gameManager.Effects;

        entity.abilityManager.AddAbility(GameManager.Abilities["cleave"], 1);

        entity.abilityIndexDict["cleave"] = 1;


        spell.text = spellBook[counter].Name;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            counter = Mathf.Clamp(counter + 1, 0, spellBook.Count);
            entity.abilityManager.AddAbility(spellBook[counter], 1);
            entity.abilityIndexDict[spellBook[counter].ID] = 1;
            spell.text = spellBook[counter].Name;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            counter = Mathf.Clamp(counter - 1, 0, spellBook.Count);
            entity.abilityManager.AddAbility(spellBook[counter], 1);
            entity.abilityIndexDict[spellBook[counter].ID] = 1;
            spell.text = spellBook[counter].Name;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(DoAnimation(gameObject, gameManager.ChaosboltExplosion, 1.0f));
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject test;
            for(int i = 0; i < 100000; i++)
            {
                test = gameManager.BlinkParticles;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
            GameObject test;
            for (int i = 0; i < 100000; i++)
            {
                test = gameManager.GetComponent<GameManager>().BlinkParticles;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject test;
            for (int i = 0; i < 100000; i++)
            {
                test = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().BlinkParticles;
            }
        }

        
	}

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 500, 50), spellBook[counter].Name);
    }

    public IEnumerator DoAnimation(GameObject source, GameObject particlePrefab, float time)
    {
        GameObject particles;

        particles = (GameObject)GameObject.Instantiate(particlePrefab, source.transform.position, source.transform.rotation);

        yield return new WaitForSeconds(time);

        ParticleSystem[] particleSystems = particles.GetComponentsInChildren<ParticleSystem>();



        foreach (ParticleSystem item in particleSystems)
        {
            item.transform.parent = null;
            item.emissionRate = 0;
            //item.enableEmission = false;

        }

        GameObject.Destroy(particles);

        yield return null;
    }
}
