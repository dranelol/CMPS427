using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour 
{
    public Transform _upperBody;

    public AnimationClip _idle;
    public AnimationClip _run;
    public AnimationClip _death;
    public AnimationClip _attack1;
    public AnimationClip _attack2;
    public AnimationClip _attack3;

    void Awake()
    {
        animation[_idle.name].layer = 1;
        animation[_run.name].layer = 1;
        animation[_death.name].layer = 1;
        animation[_attack1.name].layer = 2;
        animation[_attack2.name].layer = 2;
        animation[_attack3.name].layer = 2;
        animation[_attack1.name].wrapMode = WrapMode.Once;
        animation[_attack2.name].wrapMode = WrapMode.Once;
        animation[_attack3.name].wrapMode = WrapMode.Once;

        animation[_attack1.name].AddMixingTransform(_upperBody, true);
        animation[_attack2.name].AddMixingTransform(_upperBody, true);
        animation[_attack3.name].AddMixingTransform(_upperBody, true);
    }

    public void StopMoving()
    {
        animation.CrossFade(_idle.name, 0.2f);
    }

    public void StartMoving()
    {
        animation.CrossFade(_run.name, 0.2f);
    }

    public void Death()
    {
        animation.Play(_death.name, PlayMode.StopAll);
    }

    public void Attack(int number)
    {
        AnimationClip attackAnim;

        switch (number)
        {
            case 3:
                attackAnim = _attack3;
                break;
            case 2:
                attackAnim = _attack2;
                break;
            default:
                attackAnim = _attack1;
                break;                
        }

        animation.Play(attackAnim.name);
    }
}
