using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using Unity.VisualScripting;

using Random = UnityEngine.Random;
public class GhostDialogue : MonoBehaviour
{
    [SerializeField]
    private GhostDialogueSO catGhostDialogueSO;
    [SerializeField]
    private GhostDialogueSO dogGhostDialogueSO;
    [SerializeField]
    private GhostDialogueSO ratGhostDialogueSO;
    [SerializeField]
    private GhostDialogueSO humanGhostDialogueSO;

    [Header("Actions")]
    public static Action onDialogueEnd;
    private DialogueSystemTrigger dialogueSystemTrigger;
    public string conversationName = "Cat_Conversation";

    private void Awake()
    {
        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();

        // 注册对话结束事件
        DialogueManager.instance.conversationEnded += OnConversationEnd;
        dialogueSystemTrigger.conversation = conversationName;

        catGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/CatGhostDialogueSO");
        dogGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/DogGhostDialogueSO");
        ratGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/RatGhostDialogueSO");
        humanGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/HumanGhostDialogueSO");
    }
    void OnEnable()
    {
    }

    private void OnDestroy()
    {
        // 取消注册
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationEnded -= OnConversationEnd;
        }
        DialogueManager.instance.StopConversation();
    }

    public void GetNewDialogue()
    {
        // 初始化随机数生成器
        Random.InitState((int)DateTime.Now.Ticks);
        switch (GhostManager.Instance.currentGhost.ghostType)
        {
            case GhostType.cat:
                conversationName = catGhostDialogueSO.dialogueNames[Random.Range(0, catGhostDialogueSO.dialogueNames.Count)];
                break;
            case GhostType.dog:
                conversationName = dogGhostDialogueSO.dialogueNames[Random.Range(0, dogGhostDialogueSO.dialogueNames.Count)];
                break;
            case GhostType.rat:
                conversationName = ratGhostDialogueSO.dialogueNames[Random.Range(0, ratGhostDialogueSO.dialogueNames.Count)];
                break;
            case GhostType.male:
                conversationName = humanGhostDialogueSO.dialogueNames[Random.Range(0, humanGhostDialogueSO.dialogueNames.Count)];
                break;
            case GhostType.female:
                conversationName = humanGhostDialogueSO.dialogueNames[Random.Range(0, humanGhostDialogueSO.dialogueNames.Count)];
                break;
            default:
                break;
        }
        dialogueSystemTrigger.conversation = conversationName;
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
    private void OnConversationEnd(Transform actor)
    {
        Debug.Log("对话已结束");
        // 在这里执行对话结束后的逻辑
        //让小恶魔把资料拿过来
        onDialogueEnd?.Invoke();
    }
}
