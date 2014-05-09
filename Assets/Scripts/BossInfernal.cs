using UnityEngine;
using System.Collections;

public class BossInfernal : MonoBehaviour
{
    private SkinnedMeshRenderer[] _meshRenderer;
    public GameObject _eyes;

    public GameObject _pieces;
    public GameObject _main;
    public string _deathAnimation;

    private GameObject _target;

    void Awake()
    {
        _meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        _eyes = transform.FindChild("Eyes").gameObject;
        _pieces.animation[_deathAnimation].wrapMode = WrapMode.ClampForever;
    }

    public void Initialize(GameObject target)
    {
        _target = target;

        name = "Infernal Overlord";
        GetComponent<MouseoverDisplay>().name = name;

        _pieces.animation["gatherIntoGolem"].wrapMode = WrapMode.ClampForever;
        _pieces.animation["gatherIntoGolem"].speed = 2f;
        _pieces.animation.Play("gatherIntoGolem");

        StartCoroutine(StartCombat(_pieces.animation["gatherIntoGolem"].length / _pieces.animation["gatherIntoGolem"].speed));
    }

    private IEnumerator StartCombat(float time)
    {
        yield return new WaitForSeconds(time);

        AggroRadius aggro = transform.FindChild("EnemyAggroCollider").gameObject.AddComponent<AggroRadius>();
        aggro.activeTrigger = false;

        gameObject.AddComponent<AIController>();
        gameObject.GetComponent<Entity>().SetLevel(_target.GetComponent<Entity>().Level);
        gameObject.GetComponent<EnemyBaseAtts>().InitializeStats(_target.GetComponent<Entity>().Level);
        gameObject.GetComponent<EnemyBaseAtts>().SetAbilities();
        gameObject.GetComponent<Entity>().UpdateCurrentAttributes();
        gameObject.GetComponent<NavMeshAgent>().enabled = true;

        GetComponent<CapsuleCollider>().enabled = true;
        _meshRenderer[0].enabled = true;
        _eyes.SetActive(true);
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
        _eyes.SetActive(false);
        _pieces.SetActive(true);
        _pieces.animation.Play(_deathAnimation);
    }
}
