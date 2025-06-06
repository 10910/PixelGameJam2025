using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-110)]

public class AudioManager : MonoBehaviour
{
    public List<Sound> musicSounds, sfxSounds;

    public AudioSource musicSource, sfxSource;
    // public AudioSource sfxSourece1, sfxSource2, sfxSource3;
    // 修改此部分，增加四个用于播放 SFX 的 AudioSource
    public AudioSource[] sfxSources = new AudioSource[4];

    [SerializeField]
    private AudioSource dialogueSFXSource;
    public static AudioManager Instance;
    public FloatVariable globalMusicVolume;
    public FloatVariable globalSFXVolume;

    private float defaultMusicVolume = 0.5f;
    private float defaultMenuMusicVolume = 0.152f;

    private float defaultSFXVolume = 0.5f;

    public BoolVariable isFirstPlayGame;
    private GhostManager ghostManager;

    [SerializeField]
    private BoolVariable isNPC;

    [SerializeField]
    private BoolVariable isPC;

    [Header("sfx")]

    //tetrominosound
    public AudioClip buttonCloseSoundAudioClip;
    public AudioClip buttonSelectSoundAudioClip;
    public AudioClip buttonClickSoundAudioClip;
    public AudioClip scaleClickAudioClip;
    public AudioClip documentClickAudioClip;
    public AudioClip scaleJudgeAudioClip;
    public AudioClip hellAudioClip;
    public AudioClip rebirthAudioClip;
    public AudioClip lightOnAudioClip;

    public AudioClip typingdialogueAudioClip;
    public AudioClip catDialogueAudioClip;
    public AudioClip dogDialogueAudioClip;
    public AudioClip ratDialogueAudioClip;
    public AudioClip humanDialogueAudioClip;
    public AudioClip womanDialogueAudioClip;
    public AudioClip demonDialogueAudioClip;



    [Header("music")]
    //music
    public AudioClip noormalbattleSoundAudioClip;
    public AudioClip menuSoundAudioClip;
    public AudioClip gamePlaySoundAudioClip;

    public AudioClip CreditSoundAudioClip;
    private int currentSFXIndex = 0;

    // public
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ghostManager = FindAnyObjectByType<GhostManager>(FindObjectsInactive.Include);

        // 初始化四个 SFX AudioSource
        for (int i = 0; i < sfxSources.Length; i++)
        {
            GameObject sfxSourceObject = new GameObject("SFX Source " + (i + 1));
            sfxSourceObject.transform.SetParent(transform);
            sfxSources[i] = sfxSourceObject.AddComponent<AudioSource>();
        }
        Sound buttonCloseSound = new Sound("buttonClose", buttonCloseSoundAudioClip, 1f);
        //button
        Sound buttonSelectSound = new Sound("buttonSelect", buttonSelectSoundAudioClip, 0.072f);
        Sound buttonClickSound = new Sound("buttonClick", buttonClickSoundAudioClip, 1f);

        Sound scaleClickClickSound = new Sound("scaleClick", scaleClickAudioClip, 0.2f);
        Sound scaleJudgeClickSound = new Sound("scaleJudge", scaleJudgeAudioClip, 1f);
        Sound documentClickSound = new Sound("documentClick", documentClickAudioClip, 1.1f);
        Sound hellSound = new Sound("hell", hellAudioClip, 1f);
        Sound rebirthSound = new Sound("rebirth", rebirthAudioClip, 1f);
        Sound lightOnSound = new Sound("lightOn", lightOnAudioClip, 1f);
        Sound typingdialogueSound = new Sound("typingdialogue", typingdialogueAudioClip, 1f);
        Sound catDialogueSound = new Sound("catDialogue", catDialogueAudioClip, 0.8f);
        Sound dogDialogueSound = new Sound("dogDialogue", dogDialogueAudioClip, 0.8f);
        Sound ratDialogueSound = new Sound("ratDialogue", ratDialogueAudioClip, 0.8f);
        Sound humanDialogueSound = new Sound("humanDialogue", humanDialogueAudioClip, 0.8f);
        Sound womanDialogueSound = new Sound("womanDialogue", womanDialogueAudioClip, 0.8f);
        Sound demonDialogueSound = new Sound("demonDialogue", demonDialogueAudioClip, 0.8f);
        //原本都是0.3

        sfxSounds.Add(buttonCloseSound);

        sfxSounds.Add(buttonSelectSound);
        sfxSounds.Add(buttonClickSound);
        sfxSounds.Add(scaleClickClickSound);
        sfxSounds.Add(scaleJudgeClickSound);
        sfxSounds.Add(documentClickSound);
        sfxSounds.Add(hellSound);
        sfxSounds.Add(rebirthSound);
        sfxSounds.Add(lightOnSound);
        sfxSounds.Add(typingdialogueSound);
        sfxSounds.Add(catDialogueSound);
        sfxSounds.Add(dogDialogueSound);
        sfxSounds.Add(ratDialogueSound);
        sfxSounds.Add(humanDialogueSound);
        sfxSounds.Add(womanDialogueSound);
        sfxSounds.Add(demonDialogueSound);
        Sound noormalbattleSound = new Sound("noormalBattle", noormalbattleSoundAudioClip, 0.05f);

        Sound menuSound = new Sound("menu", menuSoundAudioClip, 0.152f);
        //battle win
        // Sound battleWinSound = new Sound("battleWin", battleWinSoundAudioClip, 0.2f);

        Sound CreditSound = new Sound("credits", CreditSoundAudioClip, 0.4f);

        Sound gamePlaySound = new Sound("gamePlay", gamePlaySoundAudioClip, 0.05f);
        musicSounds.Add(noormalbattleSound);
        musicSounds.Add(menuSound);
        musicSounds.Add(gamePlaySound);
        musicSounds.Add(CreditSound);

        if (isFirstPlayGame.currentValue)
        {
            ResetMusicVolume();
            ResetSFXVolume();
        }


        // MusicVolume(defaultMusicVolume);
        // SFXVolume(defaultSFXVolume);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayMusic(string name)
    {
        Debug.Log("Playing music: " + name);
        // Sound s = Array.Find(musicSounds, sound => sound.name == name);
        Sound s = musicSounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found in music sounds: " + name);
            return;
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.volume = CalculateActualVolume(globalMusicVolume.currentValue, s.defaultVolume);
            musicSource.loop = true;
            musicSource.Stop();
            musicSource.Play();
        }
    }

    public void PlayMusic(Sound sound)
    {
        PlayMusic(sound.name);
    }

    //play music not loop
    public void PlayMusicNotLoop(string name)
    {
        Sound s = musicSounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found in music sounds: " + name);
            return;
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.volume = CalculateActualVolume(globalMusicVolume.currentValue, s.defaultVolume);
            musicSource.loop = false;
            musicSource.Play();
        }
    }

    // Helper method to calculate the actual volume
    private float CalculateActualVolume(float globalVolume, float defaultVolume)
    {
        if (globalVolume == 0.5f)
        {
            return defaultVolume;
        }
        return defaultVolume * (globalVolume / 0.5f);
    }

    public void PlaySFX(Sound sound)
    {
        PlaySFX(sound.name);
    }

    public void PlaySFX(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        // Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found in sfx sounds: " + name);
            return;
        }
        else
        {
            // sfxSource.volume = CalculateActualVolume(globalSFXVolume.currentValue, s.defaultVolume);
            // sfxSource.PlayOneShot(s.clip);

            // 依次使用四个 AudioSource 播放音效
            sfxSources[currentSFXIndex].volume = CalculateActualVolume(globalSFXVolume.currentValue, s.defaultVolume);
            sfxSources[currentSFXIndex].PlayOneShot(s.clip);
            currentSFXIndex = (currentSFXIndex + 1) % sfxSources.Length;
        }
    }

    public void PlayDialogueSFX(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        dialogueSFXSource.volume = CalculateActualVolume(globalSFXVolume.currentValue, s.defaultVolume);
        dialogueSFXSource.clip = s.clip;
        dialogueSFXSource.Play();
    }

    public void StopSFX(string name)
    {
        Sound s = sfxSounds.Find(sound => sound.name == name);
        sfxSources[currentSFXIndex].Stop();
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    public void StartMusic()
    {
        if (musicSource.mute)
        {
            musicSource.mute = false;
        }
        musicSource.Play();
    }

    public void MusicVolume(float volume)
    {
        globalMusicVolume.SetValue(volume);

        // Adjust the current music volume based on the new global volume
        if (musicSource.clip != null)
        {
            Sound currentMusic = musicSounds.Find(sound => sound.clip == musicSource.clip);
            if (currentMusic != null)
            {
                musicSource.volume = CalculateActualVolume(volume, currentMusic.defaultVolume);
            }
        }
    }

    public void SFXVolume(float volume)
    {
        globalSFXVolume.SetValue(volume);
        // sfxSource.volume = volume;

        // Adjust the current SFX volume based on the new global volume
        if (sfxSource.clip != null)
        {
            Sound currentSFX = sfxSounds.Find(sound => sound.clip == sfxSource.clip);
            if (currentSFX != null)
            {
                sfxSource.pitch = Random.Range(0.9f, 1.1f);
                sfxSource.volume = CalculateActualVolume(volume, currentSFX.defaultVolume);
            }
        }
    }

    //resetmusicvolume
    public void ResetMusicVolume()
    {
        globalMusicVolume.SetValue(defaultMusicVolume);
        // MusicVolume(defaultMusicVolume);
    }
    public void ResetSFXVolume()
    {
        globalSFXVolume.SetValue(defaultSFXVolume);
        // SFXVolume(defaultSFXVolume);
    }

    //做一个改变音量的方法 监听音量改变 如果音量改变了我需要改变所有地方的默认音量
    //或者所有地方的音乐播放都是根据计算的


    //volume is a float between 0 and 1
    //实际音量是x
    //x:0.5=?:0.6 x/0.5=?/0.6 ?=
    //让默认的音量是0.5 如果global也是0.5就不变 如果global变成0.6 x=（0.6-0.5）/0.5*0.5+0.5
    //如果global变成0.4 x=（0.4-0.5）/0.5*0.5+0.5


    //游戏胜利
    public void PlayBattleWinMusic()
    {
        //play game win music
        Debug.Log("player win music");
        // PlayMusicNotLoop("battleWin");
        PlayMusicNotLoop("battleWin");
    }

    public void PlayRestRoomMusic()
    {
        PlayMusicNotLoop("restRoom");
    }

    public void PlayGamePlayMusic()
    {
        Debug.Log("PlayGamePlayMusic");
        PlayMusic("gamePlay");
    }

    public void PlayDialogue()
    {
        if (isPC.currentValue)
        {
            //Debug.Log("PlayDialogue demon");
            PlayDialogueSFX("demonDialogue");
        }
        else if (isNPC.currentValue)
        {
            if (ghostManager.currentGhost.ghostType == GhostType.cat)
            {
                //Debug.Log("PlayDialogue cat");
                PlayDialogueSFX("catDialogue"); // 使用声音名称
            }
            else if (ghostManager.currentGhost.ghostType == GhostType.dog)
            {
                //Debug.Log("PlayDialogue dog");
                PlayDialogueSFX("dogDialogue");
            }
            else if (ghostManager.currentGhost.ghostType == GhostType.rat)
            {
                //Debug.Log("PlayDialogue rat");
                PlayDialogueSFX("ratDialogue");
            }
            else if (ghostManager.currentGhost.ghostType == GhostType.male)
            {
                //Debug.Log("PlayDialogue human");
                PlayDialogueSFX("humanDialogue");
            }
            else if (ghostManager.currentGhost.ghostType == GhostType.female)
            {
                //Debug.Log("PlayDialogue woman");
                PlayDialogueSFX("womanDialogue");
            }
        }
    }

    public void PlayTpyingSFX()
    {
        Debug.Log("PlayTpyingSFX");
        PlayDialogueSFX("typingdialogue");
    }

    public void StopDialogue()
    {
        StopSFX("dialogue");
    }

}
