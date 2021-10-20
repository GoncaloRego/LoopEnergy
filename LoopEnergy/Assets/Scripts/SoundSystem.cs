using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SoundSystem), menuName = "ScriptableObjects/" + nameof(SoundSystem))]
public class SoundSystem : ScriptableObject
{
    public AudioClip[] audioClips;

    public AudioSource PlaySound(int soundID, AudioSource audioSourceParam)
    {
        var source = audioSourceParam;

        if(source == null)
        {
            var audioSource = new GameObject("Sound", typeof(AudioSource));
            source = audioSource.GetComponent<AudioSource>();
        }

        source.clip = audioClips[soundID];
        source.Play();

        return source;
    }
}
