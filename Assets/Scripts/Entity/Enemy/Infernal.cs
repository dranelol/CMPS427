using UnityEngine;
using System.Collections;

public class Infernal : MonoBehaviour 
{
    private SkinnedMeshRenderer[] _meshRenderer;

    public GameObject _pieces;
    public GameObject _main;
    public string _deathAnimation;

    private GameObject _source;

    void Awake()
    {
        _meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        _pieces.GetComponent<Animation>()[_deathAnimation].wrapMode = WrapMode.ClampForever;
    }

    public void Initialize(GameObject source)
    {
        _source = source;

        GetComponent<CapsuleCollider>().enabled = false;

        name = "Big Tim";
        GetComponent<MouseoverDisplay>().name = name;
   
 
        _pieces.GetComponent<Animation>()["gatherIntoGolem"].wrapMode = WrapMode.ClampForever;
        _pieces.GetComponent<Animation>()["gatherIntoGolem"].speed = 2f;
        _pieces.GetComponent<Animation>().Play("gatherIntoGolem");

        StartCoroutine(StartCombat(_pieces.GetComponent<Animation>()["gatherIntoGolem"].length / _pieces.GetComponent<Animation>()["gatherIntoGolem"].speed));
    }

    private IEnumerator StartCombat(float time)
    {
        yield return new WaitForSeconds(time);

        AggroRadius aggro = transform.FindChild("EnemyAggroCollider").gameObject.AddComponent<AggroRadius>();

        gameObject.AddComponent<AIController>();
        gameObject.GetComponent<Entity>().SetLevel(_source.GetComponent<Entity>().Level);
        gameObject.GetComponent<EnemyBaseAtts>().InitializeStats(_source.GetComponent<Entity>().Level);
        gameObject.GetComponent<EnemyBaseAtts>().SetAbilities();
        gameObject.GetComponent<Entity>().UpdateCurrentAttributes();
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        GameObject _target = _source.GetComponent<AIController>().Target;
        GetComponent<CapsuleCollider>().enabled = true;
        _meshRenderer[0].enabled = true;
        _pieces.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        aggro.activeTrigger = true;
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
        _pieces.GetComponent<Animation>().Play(_deathAnimation);
    }
}
