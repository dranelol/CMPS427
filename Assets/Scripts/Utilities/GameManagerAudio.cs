using UnityEngine;
using System.Collections.Generic;

public class GameManagerAudio : MonoBehaviour 
{
    private static Dictionary<string, AudioClip> _sounds = new Dictionary<string,AudioClip>();
    public const string _soundFolder = "Combat Sounds";

    void Awake()
    {
        Object[] allSounds = Resources.LoadAll(_soundFolder);

        foreach (Object soundObj in allSounds)
        {
            _sounds.Add(soundObj.name, (AudioClip)soundObj);
        }
    }

    public static AudioClip GetClip(string name)
    {
        if (_sounds.ContainsKey(name))
        {
            return _sounds[name];
        }

        else
        {
            return null;
        }
    }
}
