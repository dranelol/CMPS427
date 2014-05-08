using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour {
    // Range at which the player stops moving and begins attacking. BAM
    public float attackRange;

	// Need to keep track of enemy if we click to attack it.
	private Vector3 targetPosition;

	public float RotationSpeed = 10f;
    public NavMeshAgent agent;

    public PlayerEntity entity;
    public MovementFSM moveFSM;
    public CombatFSM combatFSM;
    private AnimationController _animationController;

    private HashSet<Ability> spellBook;
    public HashSet<Ability> SpellBook
    {
        get { return spellBook; }

    }

    private GameManager gameManager;
    public GameManager GameManager
    {
        get { return gameManager; }
    }

    private TalentManager talentManager;
    public TalentManager TalentManager
    {
        get { return talentManager; }
    }

    private bool mouseOverGUI;
    public bool MouseOverGUI
    {
        get { return mouseOverGUI; }
        set { mouseOverGUI = value; }
    }

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        talentManager = transform.GetComponent<TalentManager>();
        spellBook = new HashSet<Ability>();

        DontDestroyOnLoad(transform.gameObject);

        Instantiate(gameManager.SpawnInParticles, transform.position, Quaternion.identity);
        mouseOverGUI = false;
        _animationController = GetComponent<AnimationController>();
    }

	// Use this for initialization
	void Start () {
		targetPosition = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();

        entity = GetComponent<PlayerEntity>();
        moveFSM = GetComponent<MovementFSM>();
        combatFSM = GetComponent<CombatFSM>();
    }
	
	// Update is called once per frame
	void FixedUpdate()
	{
		/*if (agent.hasPath)
		{
			Vector3 newVector = (transform.position + agent.velocity.normalized);
			Vector3 target = newVector - transform.position;
			Vector3 tempRotation = transform.rotation.eulerAngles ;
			tempRotation.y = Mathf.LerpAngle(transform.rotation.eulerAngles.y,  Quaternion.LookRotation(target).eulerAngles.y,Time.deltaTime * RotationSpeed);
			transform.rotation = Quaternion.Euler(tempRotation);
		}*/
	}
	void Update () 
    {
        
      

        Debug.DrawRay(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), -22.5f));
        //Debug.DrawRay(transform.position, Rotations.RotateAboutY(new Vector3(transform.forward.x * 5.0f, transform.forward.y, transform.forward.z * 5.0f), 22.5f));
        
        // if our agent actually has a path to move to
        if (agent.hasPath == true)
        {
            /*
            // find the next steering target and his current position, without caring about y-axis
            Vector3 steeringTarget = new Vector3(agent.steeringTarget.x, 0, agent.steeringTarget.z);
            Vector3 playerPosition = new Vector3(transform.position.x, 0, transform.position.z);

            // create a quaternion to rotate towards the next target
            Quaternion quat = Quaternion.LookRotation(steeringTarget - playerPosition);

            //apply quaternion to the player's rotation
            //transform.rotation = quat;
            */
        }
        
        /*
        if (Vector3.Distance(transform.position, agent.destination) < 1.0f)
        {
            Debug.Log("Destination: " + agent.destination);
            Debug.Log("STOPPING 1");
            moveFSM.Stop();
        }
        */
        // If we have a target...

        if (targetPosition != Vector3.zero)
        {
            Debug.Log("this shouldnt happen ever");
            // If we're in attack range...
            Vector3 diff = targetPosition - transform.position;
            if (diff.magnitude <= attackRange)
            {
                // attack enemy
                targetPosition = Vector3.zero;
                Debug.Log("STOPPING 2");
                moveFSM.Stop();
            }
            else
            {
                
                moveFSM.SetPath(targetPosition);
            }
        }
        #region moving and turning
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (mouseOverGUI == true)
            {
                return;
            }

            moveFSM.LockMovement(MovementFSM.LockType.ShiftLock);
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            int terrainMask = LayerMask.NameToLayer("Terrain");

            int enemyMask = LayerMask.NameToLayer("Enemy");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit target;

            if (Physics.Raycast(ray, out target, Mathf.Infinity, 1 << terrainMask))
            {
                moveFSM.Turn(target.point);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (mouseOverGUI == true)
            {
                return;
            }

            moveFSM.UnlockMovement(MovementFSM.LockType.ShiftLock);
        }

        // If the move/attack key was pressed...
        if (Input.GetAxis("Move/Attack") != 0) 
        {
            if (mouseOverGUI == true)
            {
                return;
            }
            
            int terrainMask = LayerMask.NameToLayer("Terrain");

            int enemyMask = LayerMask.NameToLayer("Enemy");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
			RaycastHit target;

            // If the raycast hit a collider...

			if (Physics.Raycast(ray, out target, Mathf.Infinity, 1 << terrainMask))
			{
               // Debug.Log(target.collider.gameObject.layer);
                //Debug.Log(target.collider.name);

                

                // If the collider was an enemy...
                if (target.collider.gameObject.tag == "Enemy")
                {
                    // Set the target position to the enemy's position.
                    //Debug.Log("this should also never happen");
                    targetPosition = target.collider.gameObject.transform.position;
                }

                else
                {
                    // Otherwise, move towards the point of collision.
                    targetPosition = Vector3.zero;
                    NavMeshHit hit;

                    if (NavMesh.SamplePosition(target.point, out hit, 20, 1 << LayerMask.NameToLayer("Default")))
                    {
                        //Debug.Log("it should always reach here");
                        //Debug.Log("target: " + target);
                        
                        moveFSM.SetPath(hit.position);
                    }

                }
			}

        }

        
        #endregion

        #region abilities

        #region ability 1

        if (Input.GetMouseButton(1))
        {

            if (mouseOverGUI == true)
            {
                return;
            }
            
            if (entity.abilityManager.abilities[1] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[1] <= Time.time)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rayCastTarget;
                    Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                    Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                    Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                    moveFSM.Turn(transform.position + forward, 5f);

                    try
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[1], entity.EquippedEquip[equipSlots.slots.Main].equipmentType);
                    }

                    catch
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[1], equipSlots.equipmentType.Sword);
                    }

                    if (entity.abilityManager.abilities[1].AttackType == AttackType.MELEE)
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                        entity.abilityManager.abilities[1].AttackHandler(gameObject, entity, true);

                    }

                    else if (entity.abilityManager.abilities[1].AttackType == AttackType.PROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                        entity.abilityManager.abilities[1].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[1].ID, true);
                    }

                    else if (entity.abilityManager.abilities[1].AttackType == AttackType.HONINGPROJECTILE)
                    {
                        //combatFSM.Attack(0.0f);

                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                        // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object

                        int terrainMask = LayerMask.NameToLayer("Terrain");

                        int enemyMask = LayerMask.NameToLayer("Enemy");

                        entity.abilityManager.abilities[1].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[1].ID, true);
                    }

                    else if (entity.abilityManager.abilities[1].AttackType == AttackType.GROUNDTARGET)
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                        entity.abilityManager.abilities[1].AttackHandler(gameObject, rayCastTarget.point, entity, true);

                    }

                    else
                    {
                        combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                        entity.abilityManager.abilities[1].AttackHandler(gameObject, entity, true);


                    }


                    entity.abilityManager.activeCoolDowns[1] = Time.time + entity.abilityManager.abilities[1].Cooldown;


                }
            }
        }
        #endregion

        #region ability 2
        if (Input.GetKey(KeyCode.Q))
        {
            if (entity.abilityManager.abilities[2] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[2] <= Time.time)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rayCastTarget;
                    Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                    Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                    Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                    moveFSM.Turn(transform.position + forward, 5f);

                    try
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[2], entity.EquippedEquip[equipSlots.slots.Main].equipmentType);
                    }

                    catch
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[2], equipSlots.equipmentType.Sword);
                    }

                    if (entity.CurrentResource >= entity.abilityManager.abilities[2].ResourceCost)
                    {
                        entity.ModifyResource(entity.abilityManager.abilities[2].ResourceCost * -1);
                        Debug.Log("Attack Speed: " + entity.currentAtt.AttackSpeed.ToString());


                        if (entity.abilityManager.abilities[2].AttackType == AttackType.MELEE)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                            entity.abilityManager.abilities[2].AttackHandler(gameObject, entity, true);

                        }

                        else if (entity.abilityManager.abilities[2].AttackType == AttackType.PROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                            entity.abilityManager.abilities[2].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[2].ID, true);
                        }

                        else if (entity.abilityManager.abilities[2].AttackType == AttackType.HONINGPROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object

                            int terrainMask = LayerMask.NameToLayer("Terrain");

                            int enemyMask = LayerMask.NameToLayer("Enemy");


                            entity.abilityManager.abilities[2].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[2].ID, true);
                        }

                        else if (entity.abilityManager.abilities[2].AttackType == AttackType.GROUNDTARGET)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            entity.abilityManager.abilities[2].AttackHandler(gameObject, rayCastTarget.point, entity, true);

                        }

                        else
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                            entity.abilityManager.abilities[2].AttackHandler(gameObject, entity, true);


                        }

                        entity.abilityManager.activeCoolDowns[2] = Time.time + entity.abilityManager.abilities[2].Cooldown;

                    }
                }
            }
        }

        #endregion

        #region ability 3

        

        if (Input.GetKey(KeyCode.W))
        {
            if (entity.abilityManager.abilities[3] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[3] <= Time.time)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rayCastTarget;
                    Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                    Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                    Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                    moveFSM.Turn(transform.position + forward, 5f);

                    try
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[3], entity.EquippedEquip[equipSlots.slots.Main].equipmentType);
                    }

                    catch
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[3], equipSlots.equipmentType.Sword);
                    }

                    if (entity.CurrentResource >= entity.abilityManager.abilities[3].ResourceCost)
                    {
                        entity.ModifyResource(entity.abilityManager.abilities[3].ResourceCost * -1);

                        if (entity.abilityManager.abilities[3].AttackType == AttackType.MELEE)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                            entity.abilityManager.abilities[3].AttackHandler(gameObject, entity, true);
                        }


                        else if (entity.abilityManager.abilities[3].AttackType == AttackType.PROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object
                            int terrainMask = LayerMask.NameToLayer("Terrain");

                            int enemyMask = LayerMask.NameToLayer("Enemy");

                            entity.abilityManager.abilities[3].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[3].ID, true);
                        }

                        else if (entity.abilityManager.abilities[3].AttackType == AttackType.HONINGPROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object

                            int terrainMask = LayerMask.NameToLayer("Terrain");

                            int enemyMask = LayerMask.NameToLayer("Enemy");


                            entity.abilityManager.abilities[3].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[3].ID, true);
                        }

                        else if (entity.abilityManager.abilities[3].AttackType == AttackType.GROUNDTARGET)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            entity.abilityManager.abilities[3].AttackHandler(gameObject, rayCastTarget.point, entity, true);

                        }

                        else
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                            entity.abilityManager.abilities[3].AttackHandler(gameObject, entity, true);


                        }


                        entity.abilityManager.activeCoolDowns[3] = Time.time + entity.abilityManager.abilities[3].Cooldown;
                    }
                }
            }
        }
        #endregion

        #region ability 4
        
        if (Input.GetKey(KeyCode.E))
        {
            if (entity.abilityManager.abilities[4] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[4] <= Time.time)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rayCastTarget;
                    Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                    Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                    Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                    moveFSM.Turn(transform.position + forward, 5f);
                    try
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[4], entity.EquippedEquip[equipSlots.slots.Main].equipmentType);
                    }

                    catch
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[4], equipSlots.equipmentType.Sword);
                    }
                    if (entity.CurrentResource >= entity.abilityManager.abilities[4].ResourceCost)
                    {
                        entity.ModifyResource(entity.abilityManager.abilities[4].ResourceCost * -1);

                        if (entity.abilityManager.abilities[4].AttackType == AttackType.MELEE)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                            entity.abilityManager.abilities[4].AttackHandler(gameObject, entity, true);
                        }


                        else if (entity.abilityManager.abilities[4].AttackType == AttackType.PROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object

                            entity.abilityManager.abilities[4].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[4].ID, true);
                        }

                        else if (entity.abilityManager.abilities[4].AttackType == AttackType.HONINGPROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                            int terrainMask = LayerMask.NameToLayer("Terrain");

                            int enemyMask = LayerMask.NameToLayer("Enemy");


                            entity.abilityManager.abilities[4].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[4].ID, true);
                        }

                        else if (entity.abilityManager.abilities[4].AttackType == AttackType.GROUNDTARGET)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);


                            entity.abilityManager.abilities[4].AttackHandler(gameObject, rayCastTarget.point, entity, true);

                        }
                        else
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                            entity.abilityManager.abilities[4].AttackHandler(gameObject, entity, true);


                        }


                        entity.abilityManager.activeCoolDowns[4] = Time.time + entity.abilityManager.abilities[4].Cooldown;
                    }
                }
            }
        }
        #endregion

        #region ability 5
        
        if (Input.GetKey(KeyCode.R))
        {
            if (entity.abilityManager.abilities[5] != null)
            {
                if (combatFSM.IsIdle() == true && entity.abilityManager.activeCoolDowns[5] <= Time.time)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rayCastTarget;
                    Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
                    Vector3 vectorToMouse = rayCastTarget.point - transform.position;
                    Vector3 forward = new Vector3(vectorToMouse.x, transform.forward.y, vectorToMouse.z).normalized;

                    moveFSM.Turn(transform.position + forward, 5f);

                    try
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[5], entity.EquippedEquip[equipSlots.slots.Main].equipmentType);
                    }

                    catch
                    {
                        _animationController.PlayerAttack(entity.abilityManager.abilities[5], equipSlots.equipmentType.Sword);
                    }

                    if (entity.CurrentResource >= entity.abilityManager.abilities[5].ResourceCost)
                    {
                        entity.ModifyResource(entity.abilityManager.abilities[5].ResourceCost * -1);

                        if (entity.abilityManager.abilities[5].AttackType == AttackType.MELEE)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN / entity.currentAtt.AttackSpeed);
                            entity.abilityManager.abilities[5].AttackHandler(gameObject, entity, true);
                        }
                        else if (entity.abilityManager.abilities[5].AttackType == AttackType.PROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                            entity.abilityManager.abilities[5].SpawnProjectile(gameObject, gameObject, forward, entity.abilityManager.abilities[5].ID, true);
                        }

                        else if (entity.abilityManager.abilities[5].AttackType == AttackType.HONINGPROJECTILE)
                        {
                            //combatFSM.Attack(0.0f);

                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);

                            // if this is a projectile, attackhandler is only called when the projectile scores a hit.
                            // so, the keypress doesn't spawn the attackhandler, it simply inits the projectile object


                            int terrainMask = LayerMask.NameToLayer("Terrain");

                            int enemyMask = LayerMask.NameToLayer("Enemy");

                            entity.abilityManager.abilities[5].SpawnProjectile(gameObject, rayCastTarget.point, gameObject, forward, entity.abilityManager.abilities[5].ID, true);
                        }

                        else if (entity.abilityManager.abilities[5].AttackType == AttackType.GROUNDTARGET)
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);


                            entity.abilityManager.abilities[5].AttackHandler(gameObject, rayCastTarget.point, entity, true);

                        }

                        else
                        {
                            combatFSM.Attack(GameManager.GLOBAL_COOLDOWN);
                            entity.abilityManager.abilities[5].AttackHandler(gameObject, entity, true);
                        }


                        entity.abilityManager.activeCoolDowns[5] = Time.time + entity.abilityManager.abilities[5].Cooldown;
                    }
                }
            }
        }
        #endregion

        #endregion

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            entity.abilityManager.AddAbility(GameManager.Abilities["cleave"], 1);
            entity.abilityManager.AddAbility(GameManager.Abilities["shadowfury"], 2);
            entity.abilityManager.AddAbility(GameManager.Abilities["dropdasteel"], 3);
            entity.abilityManager.AddAbility(GameManager.Abilities["shadowtrap"], 4);
            entity.abilityManager.AddAbility(GameManager.Abilities["deathanddecay"], 5);

            entity.abilityIndexDict["cleave"] = 1;
            entity.abilityIndexDict["shadowfury"] = 2;
            entity.abilityIndexDict["dropdasteel"] = 3;
            entity.abilityIndexDict["shadowtrap"] = 4;
            entity.abilityIndexDict["deathanddecay"] = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            for (int i = 0; i<6; i++)
            {
                entity.removeEquipment((equipSlots.slots)i);
                entity.addEquipment(gameManager.EquipmentFactory.randomEquipment(0, 1, (equipSlots.slots)i));
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            entity.SetLevel(15);
            Debug.Log(entity.Level + " is your new level!");
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            string blah = "";

            //blah = blah + entity.GetEquip(equipSlots.slots.Main).equipmentName + " \n" + entity.GetEquip(equipSlots.slots.Off).equipmentName;
            if (entity.HasEquipped(equipSlots.slots.Main))
                blah = blah + entity.GetEquip(equipSlots.slots.Main).equipmentName + " \n";
            else blah = blah + "HAS NO SWORD \n";
            if (entity.HasEquipped(equipSlots.slots.Off))
                blah = blah + entity.GetEquip(equipSlots.slots.Off).equipmentName + " \n";
            else blah = blah + "HAS NO OFFHAND \n";
            if (entity.HasEquipped(equipSlots.slots.Head))
                blah = blah + entity.GetEquip(equipSlots.slots.Head).equipmentName + " \n";
            else blah = blah + "HAS NO HAT \n";
            if (entity.HasEquipped(equipSlots.slots.Chest))
                blah = blah + entity.GetEquip(equipSlots.slots.Chest).equipmentName + " \n";
            else blah = blah + "HAS NO SHIRT \n";
            if (entity.HasEquipped(equipSlots.slots.Legs))
                blah = blah + entity.GetEquip(equipSlots.slots.Legs).equipmentName + " \n";
            else blah = blah + "HAS NO PANTS \n";
            if (entity.HasEquipped(equipSlots.slots.Feet))
                blah = blah + entity.GetEquip(equipSlots.slots.Feet).equipmentName + " \n";
            else blah = blah + "HAS NO SHOE \n";

            Debug.Log(blah);
            blah = entity.currentAtt.Health.ToString() + " Health\n";
            blah = blah + entity.currentAtt.Resource.ToString() + " Resource\n";
            blah = blah + entity.currentAtt.Power.ToString() + " Power\n";
            blah = blah + entity.currentAtt.Defense.ToString() + " Defense\n";
            blah = blah + entity.currentAtt.MinDamage.ToString() + " MinDamage\n";
            blah = blah + entity.currentAtt.MaxDamage.ToString() + " Maxdamage\n";
            Debug.Log(blah);
            Debug.Log(entity.CurrentHP.ToString());


        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            entity.ModifyHealth(entity.currentAtt.Health-entity.CurrentHP);
            entity.ModifyResource(entity.currentAtt.Resource - entity.CurrentResource);
        }

        #region ABILITY TESTS

        if (Input.GetKeyDown(KeyCode.O))
        {

            GameObject rotationEffect = (GameObject)Instantiate(gameManager.CleaveParticles, transform.position, Quaternion.identity);

            //rotationEffect.transform.parent = transform;

            OrbSpawnSingle orbSpawn = rotationEffect.GetComponent<OrbSpawnSingle>();

            orbSpawn.orbitObject = gameObject;
            orbSpawn.initialAngleFromForward = Rotations.AngleSigned(transform.forward, Vector3.forward, Vector3.up) + 25.0f;
            //orbSpawn.rotations = 0.25f;
            //orbSpawn.angularSpeed = 360.0f;
            //orbSpawn.oscillationSpeed = 0.5f;

        }

        if (Input.GetKeyDown(KeyCode.P))
        {

            GameObject rotationEffect = (GameObject)Instantiate(gameManager.WhirlwindParticles, transform.position, Quaternion.identity);

            //rotationEffect.transform.parent = transform;

            OrbSpawnSingle orbSpawn = rotationEffect.GetComponent<OrbSpawnSingle>();

            orbSpawn.orbitObject = gameObject;
            orbSpawn.initialAngleFromForward = Rotations.AngleSigned(transform.forward, Vector3.forward, Vector3.up) + 25.0f;
            //orbSpawn.rotations = 0.25f;
            //orbSpawn.angularSpeed = 360.0f;
            //orbSpawn.oscillationSpeed = 0.5f;

        }
        #endregion

        #region equipment stuff

        // Equipping light weapon.
        if (Input.GetKeyDown(KeyCode.A))
        {
            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(0, equipSlots.slots.Main);

            entity.Inventory.AddItem(tempEquip);   
        }

        // Equipping heavy weapon.
        if (Input.GetKeyDown(KeyCode.S))
        {
            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Main);

            entity.Inventory.AddItem(tempEquip);
        }

        // Equipping chestpiece.
        if (Input.GetKeyDown(KeyCode.D))
        {
            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Chest);

            entity.Inventory.AddItem(tempEquip);
        }

        // Equipping pants.
        if (Input.GetKeyDown(KeyCode.F))
        {
            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Legs);

            entity.Inventory.AddItem(tempEquip);
        }

        // Equipping boots.
        if (Input.GetKeyDown(KeyCode.G))
        {
            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Feet);

            entity.Inventory.AddItem(tempEquip);
        }

        // Equipping helmet.
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Head);

            entity.Inventory.AddItem(tempEquip);
        }

        // Equipping offhand.
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            equipment tempEquip = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EquipmentFactory.randomEquipment(2, equipSlots.slots.Off);

            entity.Inventory.AddItem(tempEquip);
        }


        

        #endregion

        

        //Check for level up
        if (entity.Experience >= entity.NextLevelExperience)
        {
            LevelUp();
            entity.Experience = entity.NextLevelExperience - entity.Experience;
            entity.NextLevelExperience *= 2;
        }

    }

    void LevelUp()
    {
        entity.Level++;
        
        //Play animation

        talentManager.GiveTalentPoints(1);
        entity.GiveAttributePoints(5);


    }

    
}
