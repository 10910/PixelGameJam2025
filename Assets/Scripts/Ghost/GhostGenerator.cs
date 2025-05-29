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
    public bool isReborn; //审判结果
}

public class GhostGenerator : MonoBehaviour
{
    public NamesSO namesSO;
    public ProfessionsSO profsSO;
    public RecordsSO recordsSO;
    public SpriteLists spriteListsSO;
    public GhostInstancesSO specialGhostsSO;

    public int nGhosts;
    public int randomSeed = 10910;
    public int minimiumRecords = 5;

    Name[] names;
    string[] professions;
    List<Record> records;
    Dictionary<string, GhostInstance> specialGhosts;

    void Start()
    {
        namesSO = Resources.Load<NamesSO>("NamesSO");
        profsSO = Resources.Load<ProfessionsSO>("ProfessionsSO");
        recordsSO = Resources.Load<RecordsSO>("RecordsSO");
        spriteListsSO = Resources.Load<SpriteLists>("SpriteListsSO");
        specialGhostsSO = Resources.Load<GhostInstancesSO>("SpecialGhostsSO");
        specialGhosts = new Dictionary<string, GhostInstance>();
        foreach (GhostInstance ghst in specialGhostsSO.ghostInstances) { 
            specialGhosts.Add(ghst.ghostName, ghst);
        }

#if UNITY_EDITOR
        // 仅在编辑器模式下设置随机种子
        Random.InitState(randomSeed);
#endif
    }

    void OnDestroy()
    {
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
        List<Record> dogRecords = records.Where(r => r.typeCondition == GhostType.dog).ToList();
        List<Record> catRecords = records.Where(r => r.typeCondition == GhostType.cat).ToList();
        List<Record> ratRecords = records.Where(r => r.typeCondition == GhostType.rat).ToList();
        List<Record> humanRecords = recordsSO.records.Where(r => r.typeCondition == GhostType.male || r.typeCondition == GhostType.female).ToList();

        // 随机幽灵
        for (int i = 0; i < nGhosts; i++)
        {
            GhostInstance ghost = new GhostInstance();

            float typeRnd = Random.Range(0, 1.0f);

            // 1. 超过animalProbability时此次生成的幽灵是人类
            // 2. 为了避免动物和其他结局同时触发，每局最后一个幽灵一定是人类
            // 3. 第一轮只会生成人类
            if(typeRnd >= animalProbability || i == nGhosts - 1 || GameManager.Instance.RoundsPlayed == 1){
                ghost.ghostName = humanNames[i]._name;
                ghost.ghostType = humanNames[i]._type;
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

        // 特殊幽灵
        if (GameManager.Instance.RoundsPlayed == 1){
            // 第一局最后出现黑帮老大
            ghosts.Add(specialGhosts["Elias"]);
        }
        else if(GameManager.Instance.RoundsPlayed == 2) {   // todo：应该改成：根据本局结局是否特殊加入
            // 第二局最后出现疯恶魔
            ghosts.Add(specialGhosts["CrazyDemon"]);
        }
        else if(GameManager.Instance.RoundsPlayed == 3){
            // 第三局中段出现疯女人
            ghosts.Insert(ghosts.Count / 2, specialGhosts["Marla"]);
        }

        return ghosts;
    }
}
