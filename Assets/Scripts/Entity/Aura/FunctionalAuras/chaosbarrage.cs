using UnityEngine;
using System.Collections;

sealed public class chaosbarrage : Aura
{
    #region Template Constants

    /* ---------------------------------------------------AURA INFO----------------------------------------------------- *
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

    /* Use these fields to enter information about the aura. These fields are for convenience only and should **NEVER**
     * be used for anything other than outlining status effect information in one place. The base class stores properties to use
     * when referencing status effect information. Use those instead. */

    private const string TEMPLATE_AURA_DESCRIPTION = "Chaos barraging"; // Description of the status effect (should be non-empty)
    private const string TEMPLATE_AURA_FLAVOR_TEXT = "..."; // Flavor text for the status effect (optional)
    private const string TEMPLATE_AURA_ICON_TEXTURE_NAME = "default_aura_texture"; // The name of the texture for this aura to be displayed on the GUI.
    private const string TEMPLATE_AURA_PARTICLE_EFFECT_NAME = "snowdrop"; // The name of the particle effect to be used by this aura.
    private const AuraType TEMPLATE_AURA_AURATYPE = AuraType.Buff; // The type of aura, buff or debuff.
    private const int TEMPLATE_AURA_MAXIMUM_NUMBER_OF_STACKS = 1; // The number of times this effect can stack. Must be between 1 and 99 (inclusive)
    private const int TEMPLATE_AURA_INITIAL_NUMBER_OF_STACKS = 1; // The number of stacks this aura starts with.
    private const int TEMPLATE_AURA_DURATION = 5; // The number of seconds this aura will remain on a target. The duration is an INTEGER because 
    // status effects should have a finite number of seconds for the duration for simplicity.
    #endregion

    #region Constructors

    /// <summary>
    /// Use this constructor to create a prototype of this aura. The object returned by this constructor will strictly be used as 
    /// a prototype to copy from when applying this aura to an entity. This prototype's methods may not be called. The id parameter
    /// should be the integer key mapped to this prototype in its containing data structure and the programmer should ensure the 
    /// given name is unique.
    /// </summary>
    /// <param name="id">The unique integer ID.</param>
    #region Complete
    public chaosbarrage(string name)
        : base(name, TEMPLATE_AURA_DESCRIPTION, TEMPLATE_AURA_FLAVOR_TEXT, TEMPLATE_AURA_ICON_TEXTURE_NAME, TEMPLATE_AURA_PARTICLE_EFFECT_NAME,
        TEMPLATE_AURA_AURATYPE, TEMPLATE_AURA_DURATION, TEMPLATE_AURA_MAXIMUM_NUMBER_OF_STACKS, TEMPLATE_AURA_INITIAL_NUMBER_OF_STACKS
        , new doability()
    #endregion

            /* ----------------------------------------MODIFY THE REST HERE------------------------------------------------- *
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * You can define your own modules or use the predefined modules to describe how the aura affects the entity. 
         * After you define your module, pass it as a paramater below before the ')' ie. ', new CustomModule())'         */
        
        )
    { }

    #endregion

    #region Custom Modules

    protected class doability : Tick
    {
        public doability()
            : base()
        {
            
        }


        public override void OnTick()
        {

            base.OnTick();
            int tempindex = 10;

            while (SourceEntity.abilityManager.abilities[tempindex] != null && SourceEntity.abilityManager.abilities[tempindex].ID != "chaosbolt")
            {
                tempindex++;
            }

            if (SourceEntity.abilityManager.abilities[tempindex] == null)
            {
                SourceEntity.abilityManager.AddAbility(GameManager.Abilities["chaosbolt"], tempindex);
                SourceEntity.abilityIndexDict["chaosbolt"] = tempindex;

            }
            Debug.Log(SourceEntity.abilityManager.abilities[tempindex].Name);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayCastTarget;
            Physics.Raycast(ray, out rayCastTarget, Mathf.Infinity);
            Vector3 vectorToMouse = rayCastTarget.point - SourceEntity.transform.position;
            Vector3 forward = new Vector3(vectorToMouse.x, SourceEntity.transform.forward.y, vectorToMouse.z).normalized;


            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RunCoroutine(barrage(forward,tempindex,rayCastTarget, SourceEntity));
        }

        public IEnumerator barrage(Vector3 forward, int tempindex, RaycastHit rayCastTarget, Entity sourceEntity)
        {
            for (int i = 0; i < 4; i++)
            {
                sourceEntity.abilityManager.abilities[tempindex].SpawnProjectile(sourceEntity.gameObject, rayCastTarget.point, sourceEntity.gameObject, forward, sourceEntity.abilityManager.abilities[tempindex].ID, true);
                yield return new WaitForSeconds(0.25f);
            }


            yield return null;
        }
    }



    #endregion

    #region Methods

    /// <summary>
    /// Call this from the aura manager to get a unique copy of this aura from the prototype. Target, caster, and prototype cannot be null."
    /// </summary>
    /// <param name="target">The target of the aura.</param>
    /// <param name="caster">The caster of the aura.</param>
    /// <param name="prototpe">The prototype from which to clone.</param>
    /// <returns></returns>
    public override Aura Clone(Entity target, Entity caster, Aura prototpe)
    {
        return new chaosbarrage(target, caster, prototpe);
    }

    #endregion

    #region Private Constructor

    private chaosbarrage(Entity target, Entity caster, Aura prototype) : base(target, caster, prototype) { }

    #endregion
}

