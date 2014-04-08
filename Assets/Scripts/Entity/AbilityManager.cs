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
        abilities = new List<Ability>(7);
        activeCoolDowns = new List<float>(7);

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
        Debug.Log(ab.Name);
        if (abilities.Count <= 7)
        {
            abilities[index] = ab;
            PlayerPrefs.SetString("ability" + (index - 2).ToString(), ab.ID);

        }
        else
        {
            throw new Exception("Tried to add too many abilities.");
        }
    }

    public void RemoveAbility(int index)
    {
        if (abilities[index] != null)
        {
            Debug.Log("removing "+ abilities[index].Name);
            abilities[index] = null;
        }

    }


  
}
