using UnityEngine;
using System.Collections.Generic;

public class AnimationController : MonoBehaviour
{
    #region Properties

    public Transform _attackTransform;

    public AnimationClip _idle;
    public AnimationClip _walk;
    public AnimationClip _run;
    public AnimationClip _death;

    public AnimationClip _sleep;

    public AnimationClip[] _meleeAnimations;
    public AnimationClip[] _spellAnimations;
    public AnimationClip[] _miscAnimations;

    public static float DEFAULT_CHARACTER_RADIUS = 0.3f;

    private float _baseAnimationSpeed;
    private string _movementAnimation;
    private MovementFSM _movementFSM;
    private Entity _entity;

    #endregion

    #region Initialization

    void Awake()
    {
        if (!animation)
        {
            gameObject.AddComponent<Animation>();
        }

        animation.playAutomatically = true;

        animation.AddClip(_idle, "Idle");
        animation["Idle"].layer = 1;

        animation.AddClip(_walk, "Walk");
        animation["Walk"].layer = 1;
        animation["Walk"].wrapMode = WrapMode.Loop;

        animation.AddClip(_run, "Run");
        animation["Run"].layer = 1;
        animation["Run"].wrapMode = WrapMode.Loop;

        animation.AddClip(_death, "Death");
        animation["Death"].layer = 1;
        animation["Death"].wrapMode = WrapMode.Once;

        if (_sleep != null)
        {
            animation.AddClip(_sleep, "Sleep");
            animation["Sleep"].layer = 1;
        }

        else
        {
            animation.AddClip(_idle, "Sleep");
            animation["Sleep"].layer = 1;
        }

        ProcessCombatAnimations(_meleeAnimations);
        ProcessCombatAnimations(_spellAnimations);
        ProcessCombatAnimations(_miscAnimations);

        _movementAnimation = "Run";

        if (tag == "Player")
        {
            animation["cast spell"].speed = 3f;
            animation["attack 1"].speed = 2f;
            animation["attack 2"].speed = 2f;
            animation["attack 3"].speed = 2f;
            animation["attack 4"].speed = 2f;
            animation["attack 5"].speed = 2f;
            animation["attack 6"].speed = 2f;
        }

        _movementFSM = GetComponent<MovementFSM>();
        _entity = GetComponent<Entity>();
    }

    void Start()
    {
        _baseAnimationSpeed = DEFAULT_CHARACTER_RADIUS / GetComponent<MovementFSM>().Radius;
    }

    #endregion

    #region Public Functions

    public void StopMoving()
    {
        try
        {
            animation.CrossFade("Idle", 0.3f);
        }
        catch { }
    }

    public void StartMoving()
    {
        animation.CrossFade(_movementAnimation, 0.2f);
    }

    public void Death()
    {
        animation.Play("Death", PlayMode.StopAll);
    }

    public void Sleep()
    {
        if (!animation.IsPlaying("Sleep"))
        {
            animation.Play("Sleep", PlayMode.StopAll);
        }
    }

    /// <summary>
    /// Perform an animation of the given type and index. No animation will be played if the animation
    /// is not present. The index given is the index of the animation in the array specified on the 
    /// prefab, not the index of the animation out of all the animations
    /// </summary>
    /// <param name="type">The type of animation to play</param>
    /// <param name="index">The index of the animation</param>
    public void Attack(AnimationType type, int index)
    {
        AnimationClip[] animations;

        switch (type)
        {
            case AnimationType.Spell:
                animations = _spellAnimations;
                break;

            case AnimationType.Misc:
                animations = _miscAnimations;
                break;

            default:
                animations = _meleeAnimations;
                break;
        }

        try
        {
            animation.Play(animations[index].name);
        }

        catch
        {
            Debug.LogWarning("There is no " + type + " animation " + "indexed at " + index + ".");
        } 
    }


    public void Attack(string animationName)
    {
        try
        {
            animation.Play(animationName);
        }

        catch
        {
            Debug.LogWarning("There is no animation named " + animationName + ".");
        }
    }

    public void PlayerAttack(Ability ability, equipSlots.equipmentType weaponType)
    {
        AttackType attackType = ability.AttackType;

        string name;

        if (ability.ID == "whirlwind")
        {
            name = "attack 4";
            animation[name].speed = animation[name].clip.length / (GameManager.GLOBAL_COOLDOWN / _entity.currentAtt.AttackSpeed);
            _movementFSM.LockMovement(MovementFSM.LockType.MovementLock, GameManager.GLOBAL_COOLDOWN / _entity.currentAtt.AttackSpeed);
        }

        else if ((AttackType)attackType == AttackType.MELEE)
        {
            List<string> attackAnimations = new List<string>();

            if ((equipSlots.equipmentType)weaponType == equipSlots.equipmentType.Dagger)
            {
                attackAnimations.Add("attack 1");
                attackAnimations.Add("attack 3");
                attackAnimations.Add("attack 5");
            }

            else if ((equipSlots.equipmentType)weaponType == equipSlots.equipmentType.Axe)
            {
                attackAnimations.Add("attack 2");
                attackAnimations.Add("attack 1");
            }

            else
            {
                attackAnimations.Add("attack 1");
                attackAnimations.Add("attack 3");
                attackAnimations.Add("attack 4");
            }

            name = attackAnimations[UnityEngine.Random.Range((int)0, (int)attackAnimations.Count)];
            animation[name].speed = animation[name].clip.length / (GameManager.GLOBAL_COOLDOWN / _entity.currentAtt.AttackSpeed);
            _movementFSM.LockMovement(MovementFSM.LockType.MovementLock, GameManager.GLOBAL_COOLDOWN / _entity.currentAtt.AttackSpeed);

            //Debug.Log(animation[name].clip.length);
            //Debug.Log(animation[name].length);
        }

        else
        {
            name = "cast spell";
            _movementFSM.LockMovement(MovementFSM.LockType.MovementLock, animation[name].length / 3f);
        }

        Attack(name);
    }

    public void WalkToMove()
    {
        _movementAnimation = "Walk";
    }

    public void RunToMove()
    {
        _movementAnimation = "Run";
    }

    public void UpdateMovementSpeed(float value)
    {
       animation["Run"].speed = value * 1.5f;
    }

    #endregion

    #region Private Functions

    private void ProcessCombatAnimations(AnimationClip[] animations)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            string name = animations[i].name;
            animation.AddClip(animations[i], name);
            animation[name].layer = 2;
            animation[name].wrapMode = WrapMode.Once;

            if (_attackTransform != null)
            {
                animation[name].AddMixingTransform(_attackTransform, true);
            }
        }
    }

    #endregion
}

#region Enums

public enum AnimationType
{
    Melee,
    Spell,
    Misc,
}

#endregion
