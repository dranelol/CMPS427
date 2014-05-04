using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    #region Properties

    public Transform _attackTransform;

    public AnimationClip _idle;
    public AnimationClip _run;
    public AnimationClip _death;

    public AnimationClip _sleep;

    public AnimationClip[] _meleeAnimations;
    public AnimationClip[] _spellAnimations;
    public AnimationClip[] _miscAnimations;

    public static float DEFAULT_CHARACTER_RADIUS = 0.3f;

    private float _baseAnimationSpeed;

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

        animation.AddClip(_run, "Run");
        animation["Run"].layer = 1;

        animation.AddClip(_death, "Death");
        animation["Death"].layer = 1;

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
    }

    void Start()
    {
        _baseAnimationSpeed = GetComponent<MovementFSM>().Radius / DEFAULT_CHARACTER_RADIUS;
    }

    #endregion

    #region Public Functions

    public void StopMoving()
    {
        animation.CrossFade("Idle", 0.3f);
    }

    public void StartMoving()
    {
        animation.CrossFade("Run", 0.2f);
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
            animation.AddClip(animations[0], name);
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
