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
        if (!GetComponent<Animation>())
        {
            gameObject.AddComponent<Animation>();
        }

        GetComponent<Animation>().playAutomatically = true;

        GetComponent<Animation>().AddClip(_idle, "Idle");
        GetComponent<Animation>()["Idle"].layer = 1;

        GetComponent<Animation>().AddClip(_walk, "Walk");
        GetComponent<Animation>()["Walk"].layer = 1;
        GetComponent<Animation>()["Walk"].wrapMode = WrapMode.Loop;

        GetComponent<Animation>().AddClip(_run, "Run");
        GetComponent<Animation>()["Run"].layer = 1;
        GetComponent<Animation>()["Run"].wrapMode = WrapMode.Loop;

        GetComponent<Animation>().AddClip(_death, "Death");
        GetComponent<Animation>()["Death"].layer = 1;
        GetComponent<Animation>()["Death"].wrapMode = WrapMode.Once;

        if (_sleep != null)
        {
            GetComponent<Animation>().AddClip(_sleep, "Sleep");
            GetComponent<Animation>()["Sleep"].layer = 1;
        }

        else
        {
            GetComponent<Animation>().AddClip(_idle, "Sleep");
            GetComponent<Animation>()["Sleep"].layer = 1;
        }

        _movementAnimation = "Run";

        foreach (AnimationClip clip in _attackAnimations)
        {
            GetComponent<Animation>().AddClip(clip, clip.name);
            GetComponent<Animation>()[clip.name].layer = 2;
            GetComponent<Animation>()[clip.name].wrapMode = WrapMode.Once;

            if (clip.name == "attack 6")
            {
                GetComponent<Animation>()[clip.name].wrapMode = WrapMode.Loop;
                GetComponent<Animation>()[clip.name].layer = 1;
            }

            if (_attackTransform != null)
            {
                GetComponent<Animation>()[clip.name].AddMixingTransform(_attackTransform, true);
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
            GetComponent<Animation>().CrossFade("Idle", 0.3f);
        }
        catch { }
    }

    public void StartMoving()
    {
        GetComponent<Animation>().CrossFade(_movementAnimation, 0.2f);
    }

    public void Death()
    {
        GetComponent<Animation>().Play("Death", PlayMode.StopAll);
    }

    public void Sleep()
    {
        if (!GetComponent<Animation>().IsPlaying("Sleep"))
        {
            GetComponent<Animation>().Play("Sleep", PlayMode.StopAll);
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
            GetComponent<Animation>()[animationName].speed = 2;
        }

        else if (GetComponent<Animation>()[animationName].clip.length > duration)
        {
            GetComponent<Animation>()[animationName].speed = GetComponent<Animation>()[animationName].clip.length / duration;
        }

        else
        {
            GetComponent<Animation>()[animationName].speed = 1;
        }

        _movementFSM.LockMovement(MovementFSM.LockType.MovementLock, duration * 0.8f);
        GetComponent<Animation>().Play(animationName);
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
       GetComponent<Animation>()["Run"].speed = value * 1.5f;
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
