using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

[System.Serializable]
public class GhostInstance
{
    public string ghostName;
    public GhostType ghostType;
    public string profession;
    public string deadBy;
    public int age;
    public List<Record> records = new List<Record>(10);
    public Sprite sprite;
    public bool judgement; //审判结果
}


public class GhostGenerator : MonoBehaviour
{
    public NamesSO namesSO;
    public ProfessionsSO profsSO;
    public RecordsSO recordsSO;
    public SpriteLists spriteListsSO;

    public TextMeshProUGUI idText;
    public TextMeshProUGUI recordText;
    public SpriteRenderer spriteRenderer;

    public int nGhosts;
    public List<GhostInstance> ghosts;
    // string表示幽灵类型 int数组0位置表示转生 1位置表示地狱
    [ShowInInspector]
    public Dictionary<string, int[]> history = new Dictionary<string, int[]>();
    public int randomSeed = 10910;
    public int minimiumRecords = 5;

    Name[] names;
    string[] professions;
    List<Record> records;
    int currentGhostIdx;
    void Start()
    {
        namesSO = Resources.Load<NamesSO>("NamesSO");
        profsSO = Resources.Load<ProfessionsSO>("ProfessionsSO");
        recordsSO = Resources.Load<RecordsSO>("RecordsSO");
        spriteListsSO = Resources.Load<SpriteLists>("SpriteListsSO");

        JudgeManager.onJudgeEnd += nextGhost;

        Random.InitState(randomSeed);
        GenerateGhosts();
        DisplayGhost(0);
    }

    void OnDestroy()
    {
        JudgeManager.onJudgeEnd -= nextGhost;
    }

    void GenerateGhosts()
    {
        // 检验nGhost有效性
        if (nGhosts > namesSO.names.Length || nGhosts > namesSO.names.Length || nGhosts > namesSO.names.Length)
        {
            throw new System.Exception("Not enough elements to generate the requested number of ghosts");
        }

        ghosts = new List<GhostInstance>(nGhosts);

        // 打乱
        names = namesSO.names.OrderBy(x => Random.value).ToArray();
        professions = profsSO.professions.OrderBy(x => Random.value).ToArray();
        records = recordsSO.records.OrderBy(x => Random.value).ToList();

        // 每个人用的records数目不确定 所以用enumerator访问
        IEnumerator<Record> enumerator = records.GetEnumerator();
        enumerator.MoveNext();  // 进入第一项

        for (int i = 0; i < nGhosts; i++)
        {
            GhostInstance ghost = new GhostInstance();
            ghost.ghostName = names[i]._name;
            ghost.ghostType = names[i]._type;
            if (ghost.ghostType == GhostType.male || ghost.ghostType == GhostType.female)
            {
                // 人类
                ghost.profession = professions[i];
                int age = Random.Range(15, 100);
                ghost.age = age;
                // 在minimiumRecords的基础上每15岁增加一条记录
                int nRecord = minimiumRecords; //+ (age - 15) % 15;
                for (int j = 0; j < nGhosts; j++)
                {
                    ghost.records.Add(enumerator.Current);
                    enumerator.MoveNext();
                }
                // 判断年龄区间
                GhostAge ghostAge;
                if (age <= 35)
                {
                    ghostAge = GhostAge.young;
                }
                else if (age <= 65)
                {
                    ghostAge = GhostAge.middle;
                }
                else
                {
                    ghostAge = GhostAge.old;
                }
                // 查找对应ghostAge和ghostType的sprite列表
                foreach (GhostSpriteList gs in spriteListsSO.gsLists)
                {
                    if (gs.age == ghostAge && gs.type == ghost.ghostType)
                    {
                        // 从spriteList中随机选取一张
                        int nSprite = gs.spriteList.Length;
                        ghost.sprite = gs.spriteList[Random.Range(0, nSprite)];
                    }
                }
            }
            else
            {
                // 动物
                ghost.profession = "";
                ghost.age = Random.Range(1, 20);
                // 只按ghostType查找
                foreach (GhostSpriteList gs in spriteListsSO.gsLists)
                {
                    if (gs.type == ghost.ghostType)
                    {
                        // 从spriteList中随机选取一张
                        int nSprite = gs.spriteList.Length;
                        ghost.sprite = gs.spriteList[Random.Range(0, nSprite)];
                    }
                }
            }
            ghosts.Add(ghost);
        }
    }

    void DisplayGhost(int idx)
    {
        // 审判到结尾后输出结果
        if (idx >= nGhosts)
        {
            getResult();
            foreach (GhostInstance ghst in ghosts)
            {
                print(ghst.ghostName + "  " + ghst.judgement);
            }
            return;
        }

        currentGhostIdx = idx;
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
        idText.text = id;

        // records
        string record = "";
        if (ghosts[idx].records != null)
        {
            foreach (var rec in ghosts[idx].records)
            {
                record += $"{rec.description}\n";
            }
        }
        recordText.text = record;

        // sprite
        if (ghosts[idx].sprite != null)
        {
            spriteRenderer.sprite = ghosts[idx].sprite;
        }

        // 设置ghostManager的currentGhostType 用于变婴儿动画显示baby图片
        GhostManager.Instance.currentGhost.ghostType = ghosts[idx].ghostType;
    }

    // true = 转生， false = 地狱
    public void setJudgement(bool judgement)
    {
        print("judgement: " + judgement);
        ghosts[currentGhostIdx].judgement = judgement;
    }


    public void nextGhost()
    {
        print("next ghost data");
        currentGhostIdx++;
        DisplayGhost(currentGhostIdx);
    }

    void getResult()
    {
        int totalGoodness = 0;  // 玩家功德值
        foreach (var ghost in ghosts)
        {
            // 计算单个幽灵的总善良值
            int goodness = 0;
            foreach (var record in ghost.records)
            {
                goodness += record.goodness;
            }

            // 累积判决结果和功德值
            string type = ghost.ghostType.ToString();
            if (!history.ContainsKey(type))
            {
                history[type] = new int[2];
            }

            if (ghost.judgement)
            {
                // 好人转生取正 坏人转生取反
                goodness = goodness > 0 ? goodness : -goodness;
                history[type][0]++;
            }
            else
            {
                // 好人下地狱取反 坏人下地狱取正
                goodness = goodness < 0 ? -goodness : goodness;
                history[type][1]++;
            }
            totalGoodness += goodness;
        }
        print(history);
    }
}
