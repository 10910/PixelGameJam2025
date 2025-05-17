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
    public int randomSeed = 10910;
    public int minimiumRecords = 5;

    Name[] names;
    string[] professions;
    List<Record> records;

    void Start()
    {
        namesSO = Resources.Load<NamesSO>("NamesSO");
        profsSO = Resources.Load<ProfessionsSO>("ProfessionsSO");
        recordsSO = Resources.Load<RecordsSO>("RecordsSO");
        spriteListsSO = Resources.Load<SpriteLists>("SpriteListsSO");

        //JudgeManager.onJudgeEnd += nextGhost;

        Random.InitState(randomSeed);
        //GenerateGhosts();
        //DisplayGhost(0);
    }

    void OnDestroy()
    {
        //JudgeManager.onJudgeEnd -= nextGhost;
    }

    public List<GhostInstance> GenerateGhosts()
    {
        List <GhostInstance> ghosts = new List <GhostInstance>(nGhosts);
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

        return ghosts;
    }
}
