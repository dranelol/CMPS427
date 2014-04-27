using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EntityAuraManager : MonoBehaviour
{
    #region Members

    private Dictionary<string, Dictionary<Entity, Aura>> _auraDictionary;
    private Dictionary<string, GameObject> _particleDictionary;
    private List<Aura> _buffs;
    private List<Aura> _debuffs;
    private Entity _entity;

    #endregion

    void Awake()
    {
        _auraDictionary = new Dictionary<string, Dictionary<Entity, Aura>>();
        _particleDictionary = new Dictionary<string, GameObject>();
        _buffs = new List<Aura>();
        _debuffs = new List<Aura>();
        _entity = GetComponent<Entity>();
    }

    #region Methods

    #region Public Methods

    public bool Add(string name, Entity caster, int count = 0)
    {

        if (MasterAuraManager.Contains(name)) // Check if the name exists in the master list
        {
            if (_auraDictionary.ContainsKey(name)) // Check if an aura of this name already exists.
            {
                if (MasterAuraManager.Auras[name].IsStaticAura) // If it does exists and its static, do nothing
                {
                    return false;
                }

                else // If its not static
                {
                    if (_auraDictionary[name].ContainsKey(caster))
                    {
                        _auraDictionary[name][caster].Add(count);
                        return false;
                    }

                    else
                    {
                        // If this is a new caster, instantiate it, activate it, and return true.
                        Aura newAura = MasterAuraManager.GetInstance(name, _entity, caster);
                        _auraDictionary[name].Add(caster, newAura);
                        TrackAura(newAura);
                        StartCoroutine(newAura.Activate());

                        return true;
                    }
                }
            }

            else // If it does not exist, add it and return true
            {
                Aura newAura = MasterAuraManager.GetInstance(name, _entity, caster);

                _auraDictionary.Add(name, new Dictionary<Entity,Aura>());

                _auraDictionary[name].Add(caster, newAura);
                TrackAura(newAura);
                StartCoroutine(newAura.Activate());
                GameObject newAuraParticle = Instantiate(newAura.ParticleEffect, _entity.transform.position, Quaternion.identity) as GameObject;
                _particleDictionary.Add(name, newAuraParticle);
                newAuraParticle.transform.parent = _entity.transform;

                return true;
            }
        }

        else // If not, log warning and return false.
        {
            throw new ArgumentNullException("The aura " + name + " does not exist");
        }
    }

    public bool Remove(string name, Entity caster, int count = 0)
    {
        try
        {
            Aura aura = _auraDictionary[name][caster];
            aura.Remove(count);

            if (aura.StackCount == 0)
            {
                _auraDictionary[name].Remove(caster);

                if (_auraDictionary[name].Count == 0)
                {
                    _auraDictionary.Remove(name);
                    GameObject auraParticleEffect = _particleDictionary[name];
                    _particleDictionary.Remove(name);

                    ParticleSystem[] particleSystems = auraParticleEffect.GetComponentsInChildren<ParticleSystem>();

                    foreach (ParticleSystem item in particleSystems)
                    {
                        item.transform.parent = null;
                        item.emissionRate = 0;
                        item.enableEmission = false;

                    }

                    Destroy(auraParticleEffect);
                }

                UnTrackAura(aura);

                return true;
            }

            return false;
        }

        catch
        {
            return false;
        }
    }

    public void RemoveAll()
    {
        foreach (Aura aura in _buffs)
        {
            aura.Remove();
        }

        foreach (Aura aura in _debuffs)
        {
            aura.Remove();
        }

        _auraDictionary.Clear();
        _buffs.Clear();
        _debuffs.Clear();
    }

    public bool HasAuraByCaster(string name, Entity caster)
    {
        try
        {
            return _auraDictionary[name].ContainsKey(caster);
        }

        catch
        {
            return false;
        }
    }

    public bool HasAura(string name)
    {
        return _auraDictionary.ContainsKey(name);
    }

    #endregion

    #region Private Methods

    private void TrackAura(Aura newAura)
    {
        if ((AuraType)newAura.Type == AuraType.Buff)
        {
            _buffs.Add(newAura);
            _buffs.Sort
            (
                delegate(Aura aura1, Aura aura2)
                {
                    return ((aura1.TimeRemaining).CompareTo(aura2.TimeRemaining));
                }
            );
        }

        else
        {
            _debuffs.Add(newAura);
            _debuffs.Sort
            (
                delegate(Aura aura1, Aura aura2)
                {
                    return ((aura1.TimeRemaining).CompareTo(aura2.TimeRemaining));
                }
            );
        }
    }

    private void UnTrackAura(Aura aura)
    {
        if ((AuraType)aura.Type == AuraType.Buff)
        {
            _buffs.Remove(aura);
            _buffs.Sort
            (
                delegate(Aura aura1, Aura aura2)
                {
                    return ((aura1.TimeRemaining).CompareTo(aura2.TimeRemaining));
                }
            );
        }

        else
        {
            _debuffs.Remove(aura);
            _debuffs.Sort
            (
                delegate(Aura aura1, Aura aura2)
                {
                    return ((aura1.TimeRemaining).CompareTo(aura2.TimeRemaining));
                }
            );
        }
    }

    #endregion

    #endregion

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (_entity.tag == "Player")
            {
                Add("heal", _entity);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(Add("Corruption", _entity));
        }
    }
}
