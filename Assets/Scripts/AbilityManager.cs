using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour 
{
    public List<Ability> abilities;
    public List<float> activeCoolDowns;

   

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

    public void AddAbility(Ability ab, int index)
    {
        if (abilities.Count <= 6)
        {
            abilities[index] = ab;
        }
        else
        {
            throw new Exception("Tried to add too many abilities.");
        }
    }


  
}
