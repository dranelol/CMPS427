using UnityEngine;
using System.Collections;

public class Infernal : MonoBehaviour 
{
    private AnimationController _animationController;
    private SkinnedMeshRenderer[] _meshRenderer;

    public GameObject _pieces;
    public GameObject _main;
    public string _deathAnimation;

    private GameObject _source;

    void Awake()
    {
        _animationController = GetComponent<AnimationController>();
        _meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        _pieces.animation[_deathAnimation].wrapMode = WrapMode.ClampForever;
    }

    public void Initialize(GameObject source)
    {
        _source = source;

        GetComponent<CapsuleCollider>().enabled = false;
        name = _source.name + "'s Summoned Inferno";

        _pieces.animation["gatherIntoGolem"].wrapMode = WrapMode.ClampForever;
        _pieces.animation["gatherIntoGolem"].speed = 2f;
        _pieces.animation.Play("gatherIntoGolem");

        StartCoroutine(StartCombat(_pieces.animation["gatherIntoGolem"].length / _pieces.animation["gatherIntoGolem"].speed));
    }

    private IEnumerator StartCombat(float time)
    {
        yield return new WaitForSeconds(time);

        transform.parent = _source.transform.parent;
        
        transform.FindChild("EnemyAggroCollider").gameObject.AddComponent<AggroRadius>();
        AggroRadius aggro = transform.FindChild("EnemyAggroCollider").gameObject.GetComponent<AggroRadius>();
        aggro.active = false;

        gameObject.AddComponent<AIController>();
        gameObject.GetComponent<Entity>().SetLevel(_source.GetComponent<Entity>().Level);
        gameObject.GetComponent<EnemyBaseAtts>().InitializeStats();
        gameObject.GetComponent<EnemyBaseAtts>().SetAbilities();
        gameObject.GetComponent<Entity>().UpdateCurrentAttributes();
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        GameObject _target = _source.GetComponent<AIController>().Target;
        GetComponent<CapsuleCollider>().enabled = true;
        _meshRenderer[0].enabled = true;
        _pieces.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        aggro.active = true;
        GetComponent<AIController>().Threat(_target, 1);
    }

    public void Death()
    {
        for (int i = 0; i < _meshRenderer.Length; i++)
        {
            _meshRenderer[i].enabled = false;
        }
        _main.SetActive(false);
        _pieces.SetActive(true);
        _pieces.animation.Play(_deathAnimation);
    }
}
