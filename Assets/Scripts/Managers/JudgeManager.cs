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
    public Dictionary<string, Ending> endingsDict;   // 结局字典 Ending.Title作为key 结局判断里有一些还在用getEndingByName 可以改为直接用这个字典
    public bool isEndingBad2 = false; // 判断是否解锁了最坏结局 解锁的当局设置为true并在下一句出现特殊幽灵和结局
    public bool hasViewedMidDialogue = false; // 特殊幽灵的中段对话完成后设为true
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
        endingsDict = new Dictionary<string, Ending>();

        GameManager.onStartNewRound += OnStartNewRoundCallback;
        

        isFirstJudgement = true;
    }

    private void Start() {
        if (GameManager.Instance.language == Lang.Chinese) {
            endings = Resources.Load<EndingsSO>("EndingsSO" + "_CN");
        }
        else {
            endings = Resources.Load<EndingsSO>("EndingsSO");
        }

        // 初始化结局访问记录, 结局字典
        foreach (Ending ending in endings.endings) {
            endingHistory.Add(ending.Title, false);
            endingsDict.Add(ending.Title, ending);
        }
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
            id = $"{ghosts[idx].ghostName}\n ";
            if (GameManager.Instance.language == Lang.Chinese) {
                if (ghosts[idx].ghostType == GhostType.male) {
                    id += "男";
                }else if(ghosts[idx].ghostType == GhostType.female) {
                    id += "女";
                }
                id += "\n";
                id += $"死亡年龄: {ghosts[idx].age}\n";
            }
            else {
                id += $"{ghosts[idx].ghostType}\n";
                id += $"Age at Death: {ghosts[idx].age}\n";
            }
            id += $"{ghosts[idx].profession}\n";
                 //$"Dead by: ";
        }
        else
        {
            //动物
            id = $"{ghosts[idx].ghostName}\n ";
            if (GameManager.Instance.language == Lang.Chinese) {
                if (ghosts[idx].ghostType == GhostType.cat) {
                    id += "猫";
                }else if (ghosts[idx].ghostType == GhostType.dog) {
                    id += "狗";
                }else if(ghosts[idx].ghostType == GhostType.rat) {
                    id += "鼠鼠";
                }
                id += "\n";
                id += $"死亡年龄: {ghosts[idx].age}\n";
            }
            else {
                id += $"{ghosts[idx].ghostType}\n";
                id += $"Age at Death: {ghosts[idx].age}\n";
            }
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

        // 疯女人出现的回合或是已经触发最坏结局的情况下不会出现局内触发结局
        if (GameManager.Instance.RoundsPlayed == 4 || isEndingBad2){
            return false;
        }

        // 老鼠结局 总计转生次数大于等于animalEndingCnt
        int[] cnt = new int[2];
        if (history.TryGetValue("rat", out cnt) && cnt[0] >= animalEndingCnt && !endingHistory["Rat"]) {
            endingHistory["Rat"] = true;
            inRoundEnding = endings.GetEndingByName("Rat");
        }

        // 猫结局 总计转生次数大于等于animalEndingCnt
        if (history.TryGetValue("cat", out cnt) && cnt[0] >= animalEndingCnt && !endingHistory["Cat"]) {
            endingHistory["Cat"] = true;
            inRoundEnding = endings.GetEndingByName("Cat");
        }

        // 狗结局 总计转生次数大于等于animalEndingCnt
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

        // 清除文档阅览记录
        ClearDocHistory();

        // 更新对话标志位
        hasViewedMidDialogue = false;
    }

    void CheckEnding(){
        if(isEndingBad2 && !endingHistory["Bad2"]){
            isEndingBad2 = false;
            endingHistory["Bad2"] = true;
            currentEnding = endings.GetEndingByName("Bad2");
            return;
        }
        // 优先级：最坏>全转生/地狱>好人/坏人>普通
        
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
        if (totalGoodness >= 25 && totalGoodness < 50 && !endingHistory["Good1"]) {
            endingHistory["Good1"] = true;
            currentEnding = endingsDict["Good1"];
            return;
        }
        else if(totalGoodness >= 50){
            endingHistory["Good2"] = true;
            currentEnding = endingsDict["Good2"];
            return;
        }

        // 坏人结局
        if (totalGoodness <= -25  && totalGoodness > -50 && !endingHistory["Bad1"]) {
            endingHistory["Bad1"] = true;
            currentEnding = endingsDict["Bad1"];
            return;
        }else if(totalGoodness <= -50 && !endingHistory["Bad2"]){
            isEndingBad2 = true;
            return;
        }

        endingHistory["Normal"] = true;
        currentEnding = endingsDict["Normal"];
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
        if (ghosts[currentGhostIdx] is SpecialGhostInstance ghst && documentHistory["records"] && documentHistory["id"] && !    hasViewedMidDialogue){
            print("start special dialogue");
            if (ghst.midDialogue != null) { 
                DialogueManager.StartConversation(ghst.midDialogue);
            }
            hasViewedMidDialogue = true;
        }
    }

    void ClearDocHistory() {
        foreach (var key in documentHistory.Keys.ToList()) {
            documentHistory[key] = false;
        }
    }

    // 检查当前幽灵是否只有对话，用于对话结束后跳过审判
    public bool IsPlainDialogue() {
        if (ghosts[currentGhostIdx] is SpecialGhostInstance ghst){
            return ghst.isPlainDialogue;
        } else{ return false; }
    }

    public bool ShouldSkipJudge() {
        if (ghosts[currentGhostIdx] is SpecialGhostInstance ghst) {
            return ghst.shouldSkipJudge;
        }
        else { return false; }
    }

    public bool HaveReadAllDocs(){
        return documentHistory["records"] && documentHistory["id"];
    }
}
