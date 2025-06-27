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
    [SerializeField]
    private GhostDialogueSO oldManghostDialogueSO;
    [SerializeField]
    private GhostDialogueSO oldWomanghostDialogueSO;
    [SerializeField]
    private GhostDialogueSO middleAgedWomanghostDialogueSO;

    [SerializeField]
    private GhostDialogueSO middleAgedManghostDialogueSO;

    [SerializeField]
    private GhostDialogueSO youngManghostDialogueSO;
    [SerializeField]
    private GhostDialogueSO youngWomanghostDialogueSO;

    private bool isDialogueEndForced = false;


    private int conversationEndCount = 0;
    private int conversationStartCount = 0;
    [Header("Actions")]
    public static Action onDialogueEnd;
    public static Action onDialogueStart;
    private DialogueSystemTrigger dialogueSystemTrigger;
    public string conversationName = "Cat_Conversation";

    private void Awake()
    {
        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();
        isDialogueEndForced = false;

        // 注册对话结束事件
        DialogueManager.instance.conversationEnded += OnConversationEnd;
        DialogueManager.instance.conversationStarted += OnConversationStart;
        dialogueSystemTrigger.conversation = conversationName;

        catGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/CatGhostDialogueSO");
        dogGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/DogGhostDialogueSO");
        ratGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/RatGhostDialogueSO");
        humanGhostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/HumanGhostDialogueSO");
        oldManghostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/OldManGhostDialogueSO");
        oldWomanghostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/OldWomanGhostDialogueSO");
        middleAgedWomanghostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/MiddleAgedWomanGhostDialogueSO");
        middleAgedManghostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/MiddleAgedManGhostDialogueSO");
        youngManghostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/YoungManGhostDialogueSO");
        youngWomanghostDialogueSO = Resources.Load<GhostDialogueSO>("GhostDialogue/YoungWomanGhostDialogueSO");
    }
    void OnEnable()
    {
        isDialogueEndForced = false;
        conversationEndCount = 0;
        conversationStartCount = 0;
    }

    private void OnDisable()
    {
        ForceEndDialogue();
    }

    private void OnDestroy()
    {
        // 取消注册
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationEnded -= OnConversationEnd;
            DialogueManager.instance.conversationStarted -= OnConversationStart;
        }
        ForceEndDialogue();
    }

    public void ForceEndDialogue()
    {
        isDialogueEndForced = true;
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StopConversation();
        }
    }

    // 普通幽灵随机选取，特殊幽灵直接从instance获取对话名
    public void GetNewDialogue()
    {
        if(JudgeManager.Instance.ghosts[JudgeManager.Instance.currentGhostIdx] is SpecialGhostInstance ghst){
            conversationName = ghst.introDialogue;
        }else{
            // 初始化随机数生成器
            Random.InitState((int)DateTime.Now.Ticks);
            switch (GhostManager.Instance.currentGhost.ghostType) {
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
                    GetManDialogue();
                    break;
                case GhostType.female:
                    GetWomanDialogue();
                    break;
                default:
                    break;
            }
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


    public void GetManDialogue()
    {
        if (JudgeManager.Instance.ghosts[JudgeManager.Instance.currentGhostIdx].age >= 65)
        {
            GetOldManDialogue();
        }
        else if (JudgeManager.Instance.ghosts[JudgeManager.Instance.currentGhostIdx].age >= 35)
        {
            GetMiddleAgedManDialogue();
        }
        else
        {
            GetYoungManDialogue();
        }
    }
    public void GetWomanDialogue()
    {
        if (JudgeManager.Instance.ghosts[JudgeManager.Instance.currentGhostIdx].age >= 65)
        {
            GetOldWomanDialogue();
        }
        else if (JudgeManager.Instance.ghosts[JudgeManager.Instance.currentGhostIdx].age >= 35)
        {
            GetMiddleAgedWomanDialogue();
        }
        else
        {
            GetYoungWomanDialogue();
        }
    }

    public void GetYoungManDialogue()
    {
        conversationName = youngManghostDialogueSO.dialogueNames[Random.Range(0, youngManghostDialogueSO.dialogueNames.Count)];
    }

    public void GetYoungWomanDialogue()
    {
        conversationName = youngWomanghostDialogueSO.dialogueNames[Random.Range(0, youngWomanghostDialogueSO.dialogueNames.Count)];
    }

    public void GetMiddleAgedManDialogue()
    {
        conversationName = middleAgedManghostDialogueSO.dialogueNames[Random.Range(0, middleAgedManghostDialogueSO.dialogueNames.Count)];
    }

    public void GetMiddleAgedWomanDialogue()
    {
        conversationName = middleAgedWomanghostDialogueSO.dialogueNames[Random.Range(0, middleAgedWomanghostDialogueSO.dialogueNames.Count)];
    }
    public void GetDogDialogue()
    {
        conversationName = dogGhostDialogueSO.dialogueNames[Random.Range(0, dogGhostDialogueSO.dialogueNames.Count)];
    }

    public void GetOldManDialogue()
    {
        conversationName = oldManghostDialogueSO.dialogueNames[Random.Range(0, oldManghostDialogueSO.dialogueNames.Count)];
    }

    public void GetOldWomanDialogue()
    {
        conversationName = oldWomanghostDialogueSO.dialogueNames[Random.Range(0, oldWomanghostDialogueSO.dialogueNames.Count)];
    }

    //不知道为什么每次对话结束会触发两次
    private void OnConversationEnd(Transform actor)
    {
        Debug.Log("对话已结束");
        // 在这里执行对话结束后的逻辑
        //让小恶魔把资料拿过来
        //判断如果对话不是被强行结束的，则让小恶魔把资料拿过来
        // if (!isDialogueEndForced)
        // {
        //     onDialogueEnd?.Invoke();
        // }

        conversationEndCount++;
        if (conversationEndCount % 2 != 0) return; // 忽略奇数次事件

        if (JudgeManager.Instance.isFirstJudgement)
        {
            //dialogueSystemTrigger.conversation = ;
            DialogueManager.StartConversation("Demon_Tutorial");
            JudgeManager.Instance.isFirstJudgement = false;
            AnimationManager.Instance.littleDemonScale.DisableInteraction();
        }
        else if (JudgeManager.Instance.IsPlainDialogue()){
            JudgeManager.Instance.JudgeEnd();
        }
        else if (JudgeManager.Instance.ShouldSkipJudge() && JudgeManager.Instance.HaveReadAllDocs()) {
            JudgeManager.Instance.JudgeEnd();
        }
        else{
            onDialogueEnd?.Invoke();
        }
    }

    private void OnConversationStart(Transform actor)
    {
        Debug.Log("对话已开始");
        conversationStartCount++;
        if (conversationStartCount % 2 != 0) return; // 忽略奇数次事件
        //禁止天平小恶魔按钮交互
        onDialogueStart?.Invoke();
    }
}
