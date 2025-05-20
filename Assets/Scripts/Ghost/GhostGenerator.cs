using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public int nGhosts;
    public int randomSeed = 10910;
    public int minimiumRecords = 3;
    [Range(0.0f, 1f)]
    public float animalProbability = 0.25f;

    Name[] names;
    string[] professions;
    List<Record> records;

    void Start()
    {
        namesSO = Resources.Load<NamesSO>("NamesSO");
        profsSO = Resources.Load<ProfessionsSO>("ProfessionsSO");
        recordsSO = Resources.Load<RecordsSO>("RecordsSO");
        spriteListsSO = Resources.Load<SpriteLists>("SpriteListsSO");

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
        List<GhostInstance> ghosts = new List<GhostInstance>(nGhosts);
        // 检验nGhost有效性
        if (nGhosts > namesSO.names.Length || nGhosts > namesSO.names.Length || nGhosts > namesSO.names.Length)
        {
            throw new System.Exception("Not enough elements to generate the requested number of ghosts");
        }

        ghosts = new List<GhostInstance>(nGhosts);

        // 打乱
        names = namesSO.names.OrderBy(x => Random.value).ToArray();
        List<Name> humanNames = namesSO.names.Where(name => name._type == GhostType.male || name._type == GhostType.female).ToList();
        List<Name> dogNames = namesSO.names.Where(name => name._type == GhostType.dog).ToList();
        List<Name> ratNames = namesSO.names.Where(name => name._type == GhostType.rat).ToList();
        List<Name> catNames = namesSO.names.Where(name => name._type == GhostType.cat).ToList();
        List<Name> animalNames = namesSO.names.Where(name => name._type == GhostType.cat || name._type == GhostType.dog || name._type == GhostType.rat).ToList();

        professions = profsSO.professions.OrderBy(x => Random.value).ToArray();
        records = recordsSO.records.OrderBy(x => Random.value).ToList();
        List<Record> dogRecords = records.Where(r => r.typeCondition == GhostType.dog).ToList();
        List<Record> catRecords = records.Where(r => r.typeCondition == GhostType.cat).ToList();
        List<Record> ratRecords = records.Where(r => r.typeCondition == GhostType.rat).ToList();
        List<Record> humanRecords = recordsSO.records.Where(r => r.typeCondition == GhostType.male || r.typeCondition == GhostType.female).ToList();

        for (int i = 0; i < nGhosts; i++)
        {
            GhostInstance ghost = new GhostInstance();

            float typeRnd = Random.Range(0, 1.0f);

            // 选人，为了避免结局同时触发，最后一个一定是人类
            if(typeRnd >= animalProbability || i == nGhosts - 1 || GameManager.Instance.RoundsPlayed == 1){
                ghost.ghostName = humanNames[i]._name;
                ghost.ghostType = humanNames[i]._type;
                ghost.profession = professions[i];
                int age = Random.Range(15, 100);    // 年龄
                ghost.age = age;

                // 记录
                //int startIdx = minimiumRecords * i;
                //for (int j = startIdx; j < startIdx + minimiumRecords; j++) {
                //    ghost.records.Add(humanRecords[j]);
                //}
                ghost.records = RandomPick(3, humanRecords);
                
                RandomPickSprite(ghost);
            }else if(typeRnd < animalProbability){
                // 选动物
                Name animalName = RandomPick(1, animalNames)[0];
                ghost.ghostName = animalName._name;
                ghost.ghostType = animalName._type;
                ghost.profession = "";
                ghost.age = Random.Range(1, 20);
                // 只按ghostType查找
                foreach (GhostSpriteList gs in spriteListsSO.gsLists) {
                    if (gs.type == ghost.ghostType) {
                        // 从spriteList中随机选取一张
                        int nSprite = gs.spriteList.Length;
                        ghost.sprite = gs.spriteList[Random.Range(0, nSprite)];
                    }
                }
                // 随机选两个生平
                if (ghost.ghostType == GhostType.cat) {
                    ghost.records = RandomPick(2, catRecords);
                }
                else if (ghost.ghostType == GhostType.dog) {
                    ghost.records = RandomPick(2, dogRecords);
                }
                else if (ghost.ghostType == GhostType.rat) {
                    ghost.records = RandomPick(2, ratRecords);
                }
            }
            // 添加到数组
            ghosts.Add(ghost);
        }
        return ghosts;
    }

    List<T> RandomPick<T>(int n, List<T> list)
    {
        var shuffled = list.OrderBy(x => Random.value).ToList();
        List<T> result = new List<T>(n);
        for (int i = 0; i < n; i++)
        {
            result.Add(shuffled[i]);
        }
        return result;
    }

    void RandomPickSprite(GhostInstance ghost) {
        // 判断年龄区间
        GhostAge ghostAge;
        if (ghost.age <= 35) {
            ghostAge = GhostAge.young;
        }
        else if (ghost.age <= 65) {
            ghostAge = GhostAge.middle;
        }
        else {
            ghostAge = GhostAge.old;
        }
        // 查找对应ghostAge和ghostType的sprite列表
        foreach (GhostSpriteList gs in spriteListsSO.gsLists) {
            if (gs.age == ghostAge && gs.type == ghost.ghostType) {
                // 从spriteList中随机选取一张
                int nSprite = gs.spriteList.Length;
                ghost.sprite = gs.spriteList[Random.Range(0, nSprite)];
            }
        }
    }
}
