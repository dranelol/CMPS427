using UnityEngine;
using System.Collections;

public class Infernal : MonoBehaviour 
{
    private AnimationController _animationController;
    private SkinnedMeshRenderer _meshRenderer;
    private GameObject _golem;
    private GameObject _golemPieces;
    public GameObject _source;

    void Awake()
    {
        _animationController = GetComponent<AnimationController>();
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _golem = transform.FindChild("GOLEM_").gameObject;
        _golemPieces = transform.FindChild("GOLEM_SPLIT_INTO_PIECES").gameObject;
    }

    public void Initialize(GameObject source)
    {
        _source = source;

        GetComponent<CapsuleCollider>().enabled = false;
        name = _source.name + "'s Summoned Inferno";

        _golemPieces.animation["gatherIntoGolem"].wrapMode = WrapMode.ClampForever;
        _golemPieces.animation["gatherIntoGolem"].speed = 2f;
        _golemPieces.animation["fallIntoPieces"].wrapMode = WrapMode.ClampForever;
        _golemPieces.animation.Play("gatherIntoGolem");

        StartCoroutine(StartCombat(_golemPieces.animation["gatherIntoGolem"].length / _golemPieces.animation["gatherIntoGolem"].speed));
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
        _meshRenderer.enabled = true;
        _golemPieces.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        aggro.active = true;
        GetComponent<AIController>().Threat(_target, 1);
    }

    public void Death()
    {
        _meshRenderer.enabled = false;
        _golemPieces.SetActive(true);
        _golemPieces.animation.Play("fallIntoPieces");
    }
}
