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

    public AnimationClip[] _attackAnimations;

    public static float DEFAULT_CHARACTER_RADIUS = 0.3f;

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

        _movementAnimation = "Run";

        foreach (AnimationClip clip in _attackAnimations)
        {
            animation.AddClip(clip, clip.name);
            animation[clip.name].layer = 2;
            animation[clip.name].wrapMode = WrapMode.Once;

            if (clip.name == "attack 6")
            {
                animation[clip.name].wrapMode = WrapMode.Loop;
                animation[clip.name].layer = 1;
            }

            if (_attackTransform != null)
            {
                animation[clip.name].AddMixingTransform(_attackTransform, true);
            }
        }

        _movementFSM = GetComponent<MovementFSM>();
        _entity = GetComponent<Entity>();
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


    public void Attack(int attackIndex, float duration)
    {
        string animationName;

        try
        {
            animationName = _attackAnimations[attackIndex].name;
        }

        catch
        {
            animationName = _attackAnimations[0].name;
        }

        if (attackIndex == 4 || attackIndex == 5)
        {
            animation[animationName].speed = 2;
        }

        else if (animation[animationName].clip.length > duration)
        {
            animation[animationName].speed = animation[animationName].clip.length / duration;
        }

        else
        {
            animation[animationName].speed = 1;
        }

        _movementFSM.LockMovement(MovementFSM.LockType.MovementLock, duration * 0.8f);
        animation.Play(animationName);
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
}

#region Enums

public enum AnimationType
{
    Melee,
    Spell,
    Misc,
}

#endregion
