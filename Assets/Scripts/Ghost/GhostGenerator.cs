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
    public GhostInstancesSO specialGhostsSO;

    public int nGhosts;
    public int randomSeed = 10910;
    public int minimiumRecords = 3;
    [Range(0.0f, 1f)]
    public float animalProbability = 0.25f;

    Name[] names;
    string[] professions;
    List<Record> records;
    Dictionary<string, GhostInstance> specialGhosts;

    void Start()
    {
        namesSO = Resources.Load<NamesSO>("NamesSO");

        if(GameManager.Instance.language == Lang.Chinese){
            recordsSO = Resources.Load<RecordsSO>("RecordsSO" + "_CN");
            specialGhostsSO = Resources.Load<GhostInstancesSO>("SpecialGhostsSO" + "_CN");
            profsSO = Resources.Load<ProfessionsSO>("ProfessionsSO" + "_CN");
        }
        else {
            recordsSO = Resources.Load<RecordsSO>("RecordsSO");
            specialGhostsSO = Resources.Load<GhostInstancesSO>("SpecialGhostsSO");
            profsSO = Resources.Load<ProfessionsSO>("ProfessionsSO");
        }

        spriteListsSO = Resources.Load<SpriteLists>("SpriteListsSO");
        specialGhosts = new Dictionary<string, GhostInstance>();
        foreach (SpecialGhostInstance ghst in specialGhostsSO.ghostInstances) { 
            specialGhosts.Add(ghst.dictName, ghst);
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

        if(GameManager.Instance.spceialGhostTestMode){
            var sGhosts = new List<GhostInstance>();
            sGhosts.Add(specialGhosts["GangBoss"]);
            sGhosts.Add(specialGhosts["CrazyWoman"]);
            sGhosts.Add(specialGhosts["Addict"]);
            sGhosts.Add(specialGhosts["Oldman"]);
            sGhosts.Add(specialGhosts["Player"]);
            //sGhosts.Add(specialGhosts["CrazyDemon"]);

            JudgeManager.Instance.isEndingBad2 = true;
            return sGhosts;
        }

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

        // 特殊幽灵
        if (GameManager.Instance.RoundsPlayed == 1){
            // 第一局最后出现黑帮老大
            ghosts.Add(specialGhosts["GangBoss"]);
            //ghosts.Add(specialGhosts["CrazyDemon"]);
        }
        else if(GameManager.Instance.RoundsPlayed == 2) {   // todo：应该改成：根据本局结局是否特殊加入
            // 第二局最后出现疯恶魔
            //JudgeManager.Instance.isEndingBad2 = true;
            ghosts.Add(specialGhosts["CrazyDemon"]);
            //ghosts.Insert(0, specialGhosts["CrazyWoman"]);
            //ghosts.Insert(2, specialGhosts["Addict"]);
            //ghosts.Add(specialGhosts["Oldman"]);
            //ghosts.Add(specialGhosts["Player"]);
        }
        else if(GameManager.Instance.RoundsPlayed == 4){
            // 第4局中段出现疯女人
            ghosts.Insert(ghosts.Count / 2, specialGhosts["CrazyWoman"]);
        }else if(GameManager.Instance.RoundsPlayed > 4 && !JudgeManager.Instance.hasAddictedAppeared && JudgeManager.Instance.totalGoodness <= -30 ){
            // 第5局以后且负向功德值达到一定量时出现瘾君子
            ghosts.Insert(0, specialGhosts["Addict"]);
            JudgeManager.Instance.hasAddictedAppeared = true;
        }
        else if(JudgeManager.Instance.isEndingBad2){
            // 满足最坏结局功德值要求时加入老人和疯恶魔对玩家的审判
            ghosts.Add(specialGhosts["Oldman"]);
            ghosts.Add(specialGhosts["Player"]);
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
