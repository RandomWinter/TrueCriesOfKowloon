using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {
    public SoundEffect[] sounds;
    
    private void Awake(){
        foreach(var so in sounds){
            so.source = gameObject.AddComponent<AudioSource>();
            so.source.clip = so.clip;

            so.source.volume = so.volume;
            so.source.pitch = so.pitch;
        }
    }
    
    public void Play(string sName){
        var s = Array.Find(sounds, sound => sound.name == sName);
        s.source.Play();
    } 
}
