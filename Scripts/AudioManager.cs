using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClipEntry
{
    public string key;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgSource;

    [SerializeField] private Dictionary<string, AudioClip> audioClips = new();
    [SerializeField] private List<AudioClipEntry> audioClipsList;

    public static AudioManager Instance;
    

    void Awake(){
        if(Instance != null && this != Instance){
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }

        ConvertToDict();
    }

    void ConvertToDict(){
        foreach(AudioClipEntry audio in audioClipsList){
            audioClips.Add(audio.key, audio.clip);
        }
    }

    public void callSfxSource(string clipName){
        sfxSource.clip = audioClips[clipName];
        sfxSource.Play();
    }
    public void callBgSource(string clipName){
        bgSource.clip = audioClips[clipName];
        bgSource.Play();
    }
}
