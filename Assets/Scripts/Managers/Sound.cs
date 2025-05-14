using UnityEngine;


[System.Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;

    public float defaultVolume;


    // 构造函数
    // public Sound(string name, AudioClip clip)
    // {
    //     this.name = name;
    //     this.clip = clip;
    // }

    // 构造函数
    public Sound(string name, AudioClip clip, float defaultVolume)
    {
        this.name = name;
        this.clip = clip;
        this.defaultVolume = defaultVolume;
    }

}
