﻿using UnityEngine;
using System.Collections;
using System;

public class PlayerEntity : Entity 
{
    public float power, defense, attackSpeed, movementSpeed, minDamage, maxDamage;

    public Mesh mesh { get { return GetComponent<MeshFilter>().mesh; } }

    GameManager gamemanager;

	// Use this for initialization
    public void Awake()
    {
        base.Awake();
        baseAtt.Health = 3000;
        currentHP = 3000;

        gamemanager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


	public void Start() 
    {
        base.Start();

        if (gamemanager.loadsavetest == true)
        {
            for (int i = 2; i < 6; i++)
            {
                if(PlayerPrefs.HasKey("ability"+i) == true)
                {
                    abilityManager.AddAbility(GameManager.Abilities[PlayerPrefs.GetString("ability"+i)], i);
                    abilityIndexDict[PlayerPrefs.GetString("ability"+i)] = i;
                }
            }
            for( int i=0;i<6;i++)
            {
                
                if(PlayerPrefs.HasKey(i+"name") == true)
                {
                    equipment tempequip = gamemanager.EquipmentFactory.loadequipment(i+"");
                    if (addEquipment(tempequip))
                    {
                        Debug.Log("EQUIPPING " + tempequip.equipmentName + " FROM LOAD");
                    }
                    else
                    {
                        Debug.Log("CAN'T EQUIP THE "+tempequip.equipmentName+" FOR SOME REASON");
                    }
                }
            }

            // Load saved items, if any.
            Inventory.LoadItems();
        }

        else
        {
            abilityManager.AddAbility(GameManager.Abilities["shadowbolt"], 2);
            abilityManager.AddAbility(GameManager.Abilities["poisonbolt"], 3);
            abilityManager.AddAbility(GameManager.Abilities["ShockMine"], 4);
            abilityManager.AddAbility(GameManager.Abilities["chaosbolt"], 5);

            abilityIndexDict["shadowbolt"] = 2;
            abilityIndexDict["poisonbolt"] = 3;
            abilityIndexDict["ShockMine"] = 4;
            abilityIndexDict["chaosbolt"] = 5;
        }
	}
	
	// Update is called once per frame
	public void Update () 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit target;
        Physics.Raycast(ray, out target, Mathf.Infinity);
        Vector3 vectorToMouse = target.point - gameObject.transform.position;
        Vector3 normalizedVectorToMouse = new Vector3(vectorToMouse.x, gameObject.transform.forward.y, vectorToMouse.z);
        Debug.DrawRay(gameObject.transform.position, normalizedVectorToMouse.normalized * 5.0f, Color.yellow);
        //Debug.Log(ray.direction);


        //Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        //abilities[4].AttackHandler(GameObject.FindGameObjectWithTag("Player"), true);
        //abilities[5].AttackHandler(GameObject.FindGameObjectWithTag("Player"), true);

        // update these on-demand instead of every update
        power = currentAtt.Power;
        defense = currentAtt.Defense;
        attackSpeed = currentAtt.AttackSpeed;
        movementSpeed = currentAtt.MovementSpeed;
        minDamage = currentAtt.MinDamage;
        maxDamage = currentAtt.MaxDamage;

        //Debug.Log(abilities.Count);
	}

    public void OnApplicationQuit()
    {
        Inventory.SaveItems();
    }

    public Attributes GetAttributes()
    {
        return currentAtt;
    }
}
