using UnityEngine;
using System.Collections.Generic;

public class EntitySoundManager : MonoBehaviour 
{
    public List<AudioClip> _aggroClips;
    public List<AudioClip> _attackClips;
    public List<AudioClip> _getHitClips;
    public List<AudioClip> _deathClips;
    public List<AudioClip> _victoryClips;
    public List<AudioClip> _miscClips;

    private const float _attackChance = 0.7f;
    private const float _aggroChance = 0.5f;
    private const float _getHitChance = 0.5f;
    private const float _victoryChance = 0.9f;

	void Awake () 
    {
        if (!GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
	}
	
    public void Aggro()
    {
        if (tag != "Player")
        {
            if (_aggroClips.Count > 0 && UnityEngine.Random.value > _aggroChance)
            {
                GetComponent<AudioSource>().PlayOneShot(RandomClip(_aggroClips));
            }
        }
    }

    public void Attack()
    {
        if (_attackClips.Count > 0 && UnityEngine.Random.value > _attackChance)
        {
            GetComponent<AudioSource>().PlayOneShot(RandomClip(_attackClips));
        }
    }

    public void GetHit()
    {
        if (_getHitClips.Count > 0 && UnityEngine.Random.value > _getHitChance)
        {
            GetComponent<AudioSource>().PlayOneShot(RandomClip(_getHitClips));
        }
    }

    public void Death()
    {
        if (_deathClips.Count > 0)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(RandomClip(_deathClips));
        }
    }

    public void Victor()
    {
        if (_victoryClips.Count > 0 &&  UnityEngine.Random.value > _victoryChance)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().PlayOneShot(RandomClip(_victoryClips));
        }
    }

    public void Misc(string clipName)
    {
        foreach (AudioClip clip in _miscClips)
        {
            if (clip.name == clipName)
            {
                GetComponent<AudioSource>().PlayOneShot(clip);
                return;
            }
        }
    }

    private AudioClip RandomClip(List<AudioClip> list)
    {
        return list[UnityEngine.Random.Range((int)0, (int)list.Count)];
    }
}
