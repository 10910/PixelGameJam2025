using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
public class GhostDialogue : MonoBehaviour
{

    [Header("Actions")]
    public static Action onDialogueEnd;

    

    void OnEnable()
    {
        // 注册对话结束事件
        DialogueManager.instance.conversationEnded += OnConversationEnd;
    }

    void OnDisable()
    {
        // 取消注册
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationEnded -= OnConversationEnd;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueManager.instance.conversationEnded += OnConversationEnd;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //不知道为什么每次对话结束会触发两次
    void OnConversationEnd(Transform actor)
    {
        Debug.Log("对话已结束");
        // 在这里执行对话结束后的逻辑
        //让小恶魔把资料拿过来
        onDialogueEnd?.Invoke();
    }
}
