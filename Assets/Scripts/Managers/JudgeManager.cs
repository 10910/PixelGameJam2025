using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using System.Linq;

public class JudgeHistory
{
    Dictionary<string, int[]> history = new Dictionary<string, int[]>();

    public void Add(JudgeHistory another) {

    }
}

public class JudgeManager : MonoBehaviour
{
    public static JudgeManager Instance;
    [Header("Actions")]
    public static Action onStartNewJudge;
    public static Action onJudgeEnd;
    public static Action onRoundEnd;
    public static Action onAllDocumentRead;


    public int animalEndingCnt;
    public bool isFirstJudgement; // 用于显示教程
    public string idText, recordsText; // 幽灵信息
    public SpriteRenderer ghostSpriteRenderer;

    public DialogueSystemTrigger dialogueTrigger;
    public int currentGoodness; // 本局功德值
    public int totalGoodness; // 累计功德值
    public List<GhostInstance> ghosts;  // 本局生成的幽灵
    // string表示幽灵类型 int数组0位置表示转生 1位置表示地狱
    [ShowInInspector]
    public Dictionary<string, int[]> history = new Dictionary<string, int[]>();
    public Ending currentEnding; // 本局结局
    public Dictionary<string, bool> endingHistory; // 记录结局是否被浏览过, 结局名作为key
    [ShowInInspector]
    public Dictionary<string, bool> documentHistory; // 记录文件是否被浏览过, key是id和document, value为true代表已经打开过至少一次

    public int currentGhostIdx; // 当前审判的幽灵索引
    [SerializeField] private GhostGenerator generator;
    public EndingsSO endings;   // 结局数据

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ghosts = new List<GhostInstance>();
        endingHistory = new Dictionary<string, bool>();
        documentHistory = new Dictionary<string, bool>();
        documentHistory.Add("id", false);
        documentHistory.Add("records", false);

        GameManager.onStartNewRound += OnStartNewRoundCallback;
        endings = Resources.Load<EndingsSO>("EndingsSO");
        
        // 初始化结局访问记录
        foreach (Ending ending in endings.endings){
            endingHistory.Add(ending.Title, false);
        }

        isFirstJudgement = true;
    }

    private void OnDestroy()
    {
        GameManager.onStartNewRound -= OnStartNewRoundCallback;
    }

    //开始新的裁决
    public void StartNewJudge()
    {
        print("new judge start");
        // 灯光打开后触发事件
        AnimationManager.Instance.OpenLight()
            .AppendCallback(() => onStartNewJudge?.Invoke());
            //.AppendInterval(0.5f);
            //.AppendCallback(() => dialogueTrigger.OnUse());
    }

    private void OnStartNewRoundCallback()
    {
        ghosts = generator.GenerateGhosts();
        currentGhostIdx = 0;
        UpdateGhostInfo();
        StartNewJudge();
    }

    public void JudgeEnd()
    {
        Debug.Log("Judge End");
        //onJudgeEnd?.Invoke();
        // 清除文件浏览记录
        ClearDocHistory();

        // 根据审判结果更新功德值和历史记录
        UpdateHistory();

        // 移动到下一个ghost的索引
        currentGhostIdx++;

        if (CheckInRoundEnding()) {
            // 触发局内结局
            RoundEnd();
        }
        else if (currentGhostIdx >= ghosts.Count) {
            // 所有幽灵审判完成
            CheckEnding();
            RoundEnd();
        }
        else
        {
            UpdateGhostInfo();
            // 关灯动画完成之后开始新审判
            AnimationManager.Instance.CloseLight().AppendCallback(() => StartNewJudge());
        }
    }

    void RoundEnd()
    {
        onRoundEnd?.Invoke();
    }

    void UpdateGhostInfo()
    {
        int idx = currentGhostIdx;
        // id
        string id;
        if (ghosts[idx].ghostType == GhostType.male || ghosts[idx].ghostType == GhostType.female)
        {
            // 人类
            id = $"{ghosts[idx].ghostName}\n " +
                 $"{ghosts[idx].ghostType}\n" +
                 $"Age at Death: {ghosts[idx].age}\n" +
                 $"{ghosts[idx].profession}\n";
                 //$"Dead by: ";
        }
        else
        {
            //动物
            id = $"{ghosts[idx].ghostName}\n " +
                 $"{ghosts[idx].ghostType}\n" +
                 $"Age at Death: {ghosts[idx].age}\n";
                 //$"Dead by: ";
        }
        idText = id;

        // records
        string record = "";
        if (ghosts[idx].records != null)
        {
            foreach (var rec in ghosts[idx].records)
            {
                record += $"{rec.description}\n";
            }
        }
        recordsText = record;

        // sprite
        if (ghosts[idx].sprite != null)
        {
            ghostSpriteRenderer.sprite = ghosts[idx].sprite;
        }

        // 设置ghostManager的currentGhostType 用于变婴儿动画显示baby图片
        GhostManager.Instance.currentGhost.ghostType = ghosts[idx].ghostType;
        // 生成新的幽灵 暂时只用来生成新的幽灵对话
        GhostManager.Instance.GenerateNewGhost();
    }

    // 猫/狗/鼠结局 可在每次审判结束时触发 有结局触发时返回true
    bool CheckInRoundEnding(){
        Ending inRoundEnding = null;

        // 老鼠结局 总计转生次数大于等于5
        int[] cnt = new int[2];
        if (history.TryGetValue("rat", out cnt) && cnt[0] >= animalEndingCnt && !endingHistory["Rat"]) {
            endingHistory["Rat"] = true;
            inRoundEnding = endings.GetEndingByName("Rat");
        }

        // 猫结局 总计转生次数大于等于5
        if (history.TryGetValue("cat", out cnt) && cnt[0] >= animalEndingCnt && !endingHistory["Cat"]) {
            endingHistory["Cat"] = true;
            inRoundEnding = endings.GetEndingByName("Cat");
        }

        // 狗结局 总计转生次数大于等于5
        if (history.TryGetValue("dog", out cnt) && cnt[0] >= animalEndingCnt && !endingHistory["Dog"]) {
            endingHistory["Dog"] = true;
            inRoundEnding = endings.GetEndingByName("Dog");
        }
        
        currentEnding = inRoundEnding;

        return inRoundEnding != null;
    }

    void UpdateHistory(){
        // 计算单个幽灵的总善良值
        int ghostGoodness = 0;
        foreach (var record in ghosts[currentGhostIdx].records) {
            ghostGoodness += record.goodness;
        }

        // 累积判决结果和功德值
        string type = ghosts[currentGhostIdx].ghostType.ToString();
        if (!history.ContainsKey(type)) {
            history[type] = new int[2];
        }

        // 计算玩家功德值
        if (ghosts[currentGhostIdx].isReborn) {
            // 转生时好人增加功德值 坏人减少功德值 幽灵善良值正负不变
            history[type][0]++;
        }
        else {
            // 下地狱时好人减少功德值 坏人增加功德值 给幽灵善良值正负取反
            ghostGoodness = -ghostGoodness;
            history[type][1]++;
        }
        totalGoodness += ghostGoodness;
    }

    void CheckEnding(){
        // 默认普通结局，优先级：鼠>猫>狗>全转生/地狱>好人/坏人结局
        Ending roundEnding = endings.GetEndingByName("Normal");
        
        // 所有人转生/下地狱结局
        bool areAllReborn = true;
        bool areAllHell = true;
        foreach (var ghost in ghosts) {
            if (ghost.isReborn) {
                areAllHell = false;
            }
            else {
                areAllReborn = false;
            }
        }
        // 所有人转生
        if (areAllReborn && !endingHistory["AllReborn"]) {
            endingHistory["AllReborn"] = true;
            currentEnding = endings.GetEndingByName("AllReborn");
            return;
        }
        // 所有人下地狱
        if (areAllHell && !endingHistory["AllHell"]) {
            endingHistory["AllHell"] = true;
            currentEnding = endings.GetEndingByName("AllHell");
            return;
        }

        // 好人结局 累计善良值达到一定数量时触发
        if (totalGoodness >= 15 && totalGoodness < 25 && !endingHistory["Good1"]) {
            endingHistory["Good1"] = true;
            roundEnding = endings.GetEndingByName("Good1");
        }
        else if(totalGoodness >= 25){
            endingHistory["Good2"] = true;
            roundEnding = endings.GetEndingByName("Good2");
        }

        // 坏人结局
        if (totalGoodness <= -15  && !endingHistory["Bad1"]) {
            endingHistory["Bad1"] = true;
            roundEnding = endings.GetEndingByName("Bad1");
        }
        
        currentEnding = roundEnding;
    }

    // true = 转生， false = 地狱
    public void SetJudgement(bool judgement)
    {
        print("judgement: " + judgement);
        ghosts[currentGhostIdx].isReborn = judgement;
    }

    // uimanager关闭filepanel时调用， 如果当前幽灵是特殊幽灵，且id和records都已浏览，则触发特殊角色的对话
    public void CheckSpecialConversation(){
        // 这个写法可以判断类型 同时转换类型
        if (ghosts[currentGhostIdx] is SpecialGhostInstance ghst && documentHistory["records"] && documentHistory["id"]){
            print("start special dialogue");
            if (ghst.midDialogue != null) { 
                DialogueManager.StartConversation(ghst.midDialogue);
            }
        }
    }

    void ClearDocHistory() {
        foreach (var key in documentHistory.Keys.ToList()) {
            documentHistory[key] = false;
        }
    }
}
