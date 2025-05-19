using UnityEngine;
using PixelCrushers.DialogueSystem;
public class MumbleSpeakController : MonoBehaviour
{
    public AudioSource mumbleAudioSource; // 拖放AudioSource组件到这里
    public AudioClip mumbleSound; // 拖放你的mumble音效到这里

    void Start()
    {
        // 获取Dialogue Manager
        // var dialogueManager = FindObjectOfType<DialogueSystemController>();
        // if (dialogueManager != null)
        // {
        //     // 设置字幕打字机效果的开始和结束事件
        //     var typewriterEffect = dialogueManager.GetComponent<StandardUISubtitlePanel>().
        //         GetComponentInChildren<AbstractTypewriterEffect>();

        //     if (typewriterEffect != null)
        //     {
        //         // 注册打字机效果开始事件
        //         typewriterEffect.onBegin.AddListener(() =>
        //         {
        //             if (mumbleAudioSource != null && mumbleSound != null)
        //             {
        //                 mumbleAudioSource.clip = mumbleSound;
        //                 mumbleAudioSource.loop = true;
        //                 mumbleAudioSource.Play();
        //             }
        //         });

        //         // 注册打字机效果结束事件
        //         typewriterEffect.onEnd.AddListener(() =>
        //         {
        //             if (mumbleAudioSource != null && mumbleAudioSource.isPlaying)
        //             {
        //                 mumbleAudioSource.Stop();
        //             }
        //         });
        //     }
        // }
    }
}