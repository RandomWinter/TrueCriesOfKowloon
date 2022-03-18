using UnityEngine;

[System.Serializable]
public class SoundEffect{
    [Header("Setup Sound")]
    public string name;
    public AudioClip clip;

    [Header("Adjust Sound Effect")]
    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;

    [HideInInspector] public AudioSource source;
}
