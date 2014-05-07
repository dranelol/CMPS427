using UnityEngine;
using System.Collections;
using System;

public class PlayerEntity : Entity 
{
    public float power, defense, attackSpeed, movementSpeed, minDamage, maxDamage;

    private int nextLevelExperience;
    public int NextLevelExperience
    {
        get
        {
            return nextLevelExperience;
        }

        set
        {
            nextLevelExperience = value;
        }
    }

    private int attributePoints;
    public int AttributePoints
    {
        get { return attributePoints; }
        set { attributePoints = value; }
    }

    public Mesh mesh { get { return GetComponent<MeshFilter>().mesh; } }

    GameManager gameManager;

	// Use this for initialization
    public void Awake()
    {
        base.Awake();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        baseAtt.Health = 1000;
        Experience = 0;
        Level = 1;
        nextLevelExperience = 100;

    }

	public void Start() 
    {
        base.Start();


        if (gameManager.loadSaveTest == true)
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
                    equipment tempequip = gameManager.EquipmentFactory.loadequipment(i+"");
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
            abilityManager.AddAbility(GameManager.Abilities["shadowbolt"], 1);
            abilityManager.AddAbility(GameManager.Abilities["improvedshadowbolt"], 2);
            abilityManager.AddAbility(GameManager.Abilities["deathanddecay"], 3);
            abilityManager.AddAbility(GameManager.Abilities["whirlwind"], 4);
            abilityManager.AddAbility(GameManager.Abilities["shadowfury"], 5);

            abilityIndexDict["shadowbolt"] = 1;
            abilityIndexDict["improvedshadowbolt"] = 2;
            abilityIndexDict["deathanddecay"] = 3;
            abilityIndexDict["whirlwind"] = 4;
            abilityIndexDict["shadowfury"] = 5;
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


        ModifyResource(0.5f);
	}

    public void OnApplicationQuit()
    {
       // Inventory.SaveItems();
    }

    public Attributes GetAttributes()
    {
        return currentAtt;
    }

    /// <summary>
    /// Give the specified amount of experience to the player.
    /// </summary>
    /// <param name="expToAdd">Experience to add to the player.</param>
    public void GiveExperience(int expToAdd)
    {
        Experience += expToAdd;
    }

    public void GiveAttributePoints(int attrPointsToAdd)
    {
        attributePoints += attrPointsToAdd;
    }
}
