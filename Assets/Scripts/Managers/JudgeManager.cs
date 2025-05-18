using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class JudgeHistory
{
    Dictionary<string, int[]> history = new Dictionary<string, int[]>();

    public void Add(JudgeHistory another)
    {

    }
}

public class JudgeManager : MonoBehaviour
{
    public static JudgeManager Instance;
    [Header("Actions")]
    public static Action onStartNewJudge;
    public static Action onJudgeEnd;
    public static Action onRoundEnd;


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

    public int currentGhostIdx; // 当前审判的幽灵索引
    [SerializeField] private GhostGenerator generator;
    private EndingsSO endings;   // 结局数据
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
        endingHistory = new Dictionary<string, bool>();
        demon = FindFirstObjectByType<Demon>(FindObjectsInactive.Include);
        GameManager.onStartNewRound += OnStartNewRoundCallback;
        endings = Resources.Load<EndingsSO>("EndingsSO");

        // 初始化结局访问记录
        foreach (Ending ending in endings.endings)
        {
            endingHistory.Add(ending.Title, false);
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
        Debug.Log("JudgeManager PullToHell");
        UIManager.Instance.OpenPullToHellEffect();
        demon.PullToHell();
        // 停止幽灵对话
        GhostManager.Instance.StopGhostDialogue();
    }

    public void Rebirth()
    {
        GhostManager.Instance.currentGhost.gameObject.SetActive(false);
        UIManager.Instance.OpenRebirthEffect();
        demon.Rebirth();
        // 停止幽灵对话
        GhostManager.Instance.StopGhostDialogue();
    }

    public void JudgeEnd()
    {
        Debug.Log("Judge End");
        //onJudgeEnd?.Invoke();
        // 移动到下一个ghost的索引
        currentGhostIdx++;
        if (currentGhostIdx >= ghosts.Count)
        {
            // 索引达到末尾时本局结束
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
        // 所有幽灵审判完成 进入结局画面
        print("all judgement over");
        SetResult();
        CheckEnding();
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
                 $"Profession: {ghosts[idx].profession}\n" +
                 $"Dead by: ";
        }
        else
        {
            //动物
            id = $"{ghosts[idx].ghostName}\n " +
                 $"{ghosts[idx].ghostType}\n" +
                 $"Age at Death: {ghosts[idx].age}\n" +
                 $"Dead by: ";
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

    void SetResult()
    {
        int goodness = 0;  // 玩家功德值
        foreach (var ghost in ghosts)
        {
            // 计算单个幽灵的总善良值
            int ghostGoodness = 0;
            foreach (var record in ghost.records)
            {
                ghostGoodness += record.goodness;
            }

            // 累积判决结果和功德值
            string type = ghost.ghostType.ToString();
            if (!history.ContainsKey(type))
            {
                history[type] = new int[2];
            }

            if (ghost.isReborn)
            {
                // 好人转生取正 坏人转生取反
                ghostGoodness = ghostGoodness > 0 ? ghostGoodness : -ghostGoodness;
                history[type][0]++;
            }
            else
            {
                // 好人下地狱取反 坏人下地狱取正
                ghostGoodness = ghostGoodness < 0 ? -ghostGoodness : ghostGoodness;
                history[type][1]++;
            }
            goodness += ghostGoodness;
        }
        currentGoodness = goodness;
        totalGoodness += currentGoodness;
    }

    void CheckEnding()
    {
        // 默认普通结局， 多个结局同时发生时只会触发最后检测的 优先级：鼠>猫>狗>全转生>好人结局
        Ending roundEnding = endings.GetEndingByName("Normal");

        // 老鼠结局 总计转生次数大于等于5
        int[] cnt = new int[2];
        if (history.TryGetValue("Rat", out cnt) && cnt[0] >= 5 && !endingHistory["Rat"])
        {
            endingHistory["Rat"] = true;
            roundEnding = endings.GetEndingByName("Rat");
        }

        // 猫结局 总计转生次数大于等于5
        if (history.TryGetValue("Cat", out cnt) && cnt[0] >= 5 && !endingHistory["Cat"])
        {
            endingHistory["Cat"] = true;
            roundEnding = endings.GetEndingByName("Cat");
        }

        // 狗结局 总计转生次数大于等于5
        if (history.TryGetValue("Dog", out cnt) && cnt[0] >= 5 && !endingHistory["Dog"])
        {
            endingHistory["Dog"] = true;
            roundEnding = endings.GetEndingByName("Dog");
        }

        // 所有人转生/下地狱结局
        bool areAllReborn = true;
        bool areAllHell = true;
        foreach (var ghost in ghosts)
        {
            if (ghost.isReborn)
            {
                areAllHell = false;
            }
            else
            {
                areAllReborn = false;
            }
        }
        // 所有人转生
        if (areAllReborn && !endingHistory["AllReborn"])
        {
            endingHistory["AllReborn"] = true;
            roundEnding = endings.GetEndingByName("AllReborn");
        }
        // 所有人下地狱
        if (areAllHell && !endingHistory["AllHell"])
        {
            endingHistory["AllHell"] = true;
            roundEnding = endings.GetEndingByName("AllHell");
        }

        // 好人结局 累计善良值达到一定数量时触发
        if (totalGoodness >= 10 && totalGoodness < 20 && !endingHistory["Good1"])
        {
            endingHistory["Good1"] = true;
            roundEnding = endings.GetEndingByName("Good1");
        }
        else if (totalGoodness >= 20)
        {
            endingHistory["Good2"] = true;
            roundEnding = endings.GetEndingByName("Good2");
        }
        currentEnding = roundEnding;
    }

    // true = 转生， false = 地狱
    public void SetJudgement(bool judgement)
    {
        print("judgement: " + judgement);
        ghosts[currentGhostIdx].isReborn = judgement;
    }
}
