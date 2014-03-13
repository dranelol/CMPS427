using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour 
{
    public List<Ability> abilities;
    public List<float> activeCoolDowns;
    private Dictionary<int, List<Ability>> entityToAbilities;

   

	// Use this for initialization
    public void Awake()
    {
        abilities = new List<Ability>(6);
        activeCoolDowns = new List<float>(6);

        for (int i = 0; i < abilities.Capacity; i++)
        {
            abilities.Add(null);
            activeCoolDowns.Add(0.0f);
        }

        
        

        
    }


	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    public void AddAbility()
    {

    }

    public void UseAbility()
    {

    }

  
}
