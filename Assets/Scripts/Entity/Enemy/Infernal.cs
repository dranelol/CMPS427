using UnityEngine;
using System.Collections;

public class Infernal : MonoBehaviour 
{
    private SkinnedMeshRenderer[] _meshRenderer;

    public GameObject _pieces;
    public GameObject _main;
    public string _deathAnimation;

    private GameObject _source;
    public bool boss;

    void Awake()
    {
        _meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        _pieces.animation[_deathAnimation].wrapMode = WrapMode.ClampForever;
    }

    public void Initialize(GameObject source, bool bossMode = false)
    {
        _source = source;
        boss = bossMode;

        GetComponent<CapsuleCollider>().enabled = false;

        if (!boss)
        {
            name = _source.name + "'s Summoned Inferno";
        }

        else
        {
            name = "Infernal Overlord";
        }
        
        _pieces.animation["gatherIntoGolem"].wrapMode = WrapMode.ClampForever;
        _pieces.animation["gatherIntoGolem"].speed = 2f;
        _pieces.animation.Play("gatherIntoGolem");

        StartCoroutine(StartCombat(_pieces.animation["gatherIntoGolem"].length / _pieces.animation["gatherIntoGolem"].speed));
    }

    private IEnumerator StartCombat(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject _target;

        if (!boss)
        {
            transform.parent = _source.transform.parent;
            _target = _source.GetComponent<AIController>().Target;
        }

        else
        
        transform.FindChild("EnemyAggroCollider").gameObject.AddComponent<AggroRadius>();
        AggroRadius aggro = transform.FindChild("EnemyAggroCollider").gameObject.GetComponent<AggroRadius>();
        aggro.activeTrigger = false;

        gameObject.AddComponent<AIController>();
        gameObject.GetComponent<Entity>().SetLevel(_source.GetComponent<Entity>().Level);
        gameObject.GetComponent<EnemyBaseAtts>().InitializeStats(_source.GetComponent<Entity>().Level);
        gameObject.GetComponent<EnemyBaseAtts>().SetAbilities();
        gameObject.GetComponent<Entity>().UpdateCurrentAttributes();
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        GetComponent<CapsuleCollider>().enabled = true;
        _meshRenderer[0].enabled = true;
        _pieces.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        aggro.activeTrigger = true;
        // GetComponent<AIController>().Threat(_target, 1);
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
