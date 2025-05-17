using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class JudgeHistory{

}

public class JudgeManager : MonoBehaviour
{
    public static JudgeManager Instance;
    [Header("Actions")]
    public static Action onStartNewJudge;
    public static Action onJudgeEnd;
    public static Action onRoundEnd;


    public bool isFirstJudgement; // 用于显示教程
    public string idText, recordsText;
    public int goodness; // 每局累计功德值
    public SpriteRenderer ghostSpriteRenderer;

    public DialogueSystemTrigger dialogueTrigger;
    public GhostGenerator generator;
    public List<GhostInstance> ghosts;  // 本局生成的幽灵
    // string表示幽灵类型 int数组0位置表示转生 1位置表示地狱
    [ShowInInspector]
    public Dictionary<string, int[]> history = new Dictionary<string, int[]>();
    int currentGhostIdx; // 当前审判的幽灵索引

    private Demon demon;

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
        demon = FindFirstObjectByType<Demon>(FindObjectsInactive.Include);
        GameManager.onStartNewRound += OnStartNewRoundCallback;
        //AnimationManager.onJudgeAnimEnd += OnJudgeAnimEndCallback;
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
            .AppendCallback(() => onStartNewJudge?.Invoke())
            .AppendInterval(0.5f)
            .AppendCallback(() => dialogueTrigger.OnUse());
    }

    private void OnStartNewRoundCallback()
    {
        ghosts = generator.GenerateGhosts();
        currentGhostIdx = 0;
        UpdateGhostInfo();
        StartNewJudge();
    }

    public void PullToHell()
    {
        UIManager.Instance.OpenPullToHellEffect();
        demon.PullToHell();
    }

    public void Rebirth()
    {
        GhostManager.Instance.currentGhost.gameObject.SetActive(false);
        UIManager.Instance.OpenRebirthEffect();
        demon.Rebirth();
    }

    public void JudgeEnd()
    {
        Debug.Log("Judge End");
        //onJudgeEnd?.Invoke();
        // 移动到下一个ghost的索引
        currentGhostIdx++;
        if (currentGhostIdx >= ghosts.Count) {
            // 索引达到末尾时本局结束
            RoundEnd();
        }else{
            UpdateGhostInfo();
            // 关灯动画完成之后开始新审判
            AnimationManager.Instance.CloseLight().AppendCallback(() => StartNewJudge());
        }
    }

    void RoundEnd(){
        // 所有幽灵审判完成 进入结局画面
        print("all judgement over");
        GetResult();
        UIManager.Instance.OpenEndingPanel();
        onRoundEnd?.Invoke();
    }

    void UpdateGhostInfo() {
        int idx = currentGhostIdx;
        // id
        string id;
        if (ghosts[idx].ghostType == GhostType.male || ghosts[idx].ghostType == GhostType.female) {
            // 人类
            id = $"{ghosts[idx].ghostName}\n " +
                 $"{ghosts[idx].ghostType}\n" +
                 $"Age at Death: {ghosts[idx].age}\n" +
                 $"Profession: {ghosts[idx].profession}\n" +
                 $"Dead by: ";
        }
        else {
            //动物
            id = $"{ghosts[idx].ghostName}\n " +
                 $"{ghosts[idx].ghostType}\n" +
                 $"Age at Death: {ghosts[idx].age}\n" +
                 $"Dead by: ";
        }
        idText = id;

        // records
        string record = "";
        if (ghosts[idx].records != null) {
            foreach (var rec in ghosts[idx].records) {
                record += $"{rec.description}\n";
            }
        }
        recordsText = record;

        // sprite
        if (ghosts[idx].sprite != null) {
            ghostSpriteRenderer.sprite = ghosts[idx].sprite;
        }

        // 设置ghostManager的currentGhostType 用于变婴儿动画显示baby图片
        GhostManager.Instance.currentGhost.ghostType = ghosts[idx].ghostType;
    }

    void GetResult() {
        int totalGoodness = 0;  // 玩家功德值
        foreach (var ghost in ghosts) {
            // 计算单个幽灵的总善良值
            int goodness = 0;
            foreach (var record in ghost.records) {
                goodness += record.goodness;
            }

            // 累积判决结果和功德值
            string type = ghost.ghostType.ToString();
            if (!history.ContainsKey(type)) {
                history[type] = new int[2];
            }

            if (ghost.judgement) {
                // 好人转生取正 坏人转生取反
                goodness = goodness > 0 ? goodness : -goodness;
                history[type][0]++;
            }
            else {
                // 好人下地狱取反 坏人下地狱取正
                goodness = goodness < 0 ? -goodness : goodness;
                history[type][1]++;
            }
            totalGoodness += goodness;
        }
    }

    // true = 转生， false = 地狱
    public void SetJudgement(bool judgement) {
        print("judgement: " + judgement);
        ghosts[currentGhostIdx].judgement = judgement;
    }
}
