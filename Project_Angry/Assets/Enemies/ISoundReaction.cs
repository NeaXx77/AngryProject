using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISoundReaction : MonoBehaviour
{
    public List<AudioSource> sounds;
    public List<AudioClip> clips;
    public void SetSounds(AudioSource audio){
        sounds.Clear();
        sounds.Add(audio);
    }
    public void SetSounds(AudioSource[] audio){
        sounds.Clear();
        sounds.AddRange(audio);
    }

    public void SetClips(AudioClip[] clips){
        this.clips.AddRange(clips);
    }
    public void SetClips(AudioClip clip){
        clips.Add(clip);
    }
    public void MakeSound(GameObject obj){
        foreach (AudioSource s in sounds)
        {
            foreach (AudioClip c in clips)
            {
                s.clip = c;
                s.PlayOneShot(s.clip);
            }
        }
    }
}