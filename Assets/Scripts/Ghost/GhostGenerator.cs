using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GhostInstance{
    public string ghostName;
    public GhostType ghostType;
    public string profession;
    public string deadBy;
    public int age;
    public List<Record> records = new List<Record>(10);
    public Sprite sprite;
    public bool judgement; //���н��
}

public class GhostGenerator : MonoBehaviour
{
    public NamesSO namesSO;
    public ProfessionsSO profsSO;
    public RecordsSO recordsSO;

    public TextMeshProUGUI idText;
    public TextMeshProUGUI recordText;

    public int nGhosts;
    public List<GhostInstance> ghosts;
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
        Random.InitState(randomSeed);
        GenerateGhosts();
        DisplayGhost(0);
    }

    void GenerateGhosts(){
        // ����nGhost��Ч��
        if (nGhosts > namesSO.names.Length || nGhosts > namesSO.names.Length || nGhosts > namesSO.names.Length) {
            throw new System.Exception("Not enough elements to generate the requested number of ghosts");
        }

        ghosts = new List<GhostInstance>(nGhosts);

        // ����
        names = namesSO.names.OrderBy(x => Random.value).ToArray();
        professions = profsSO.professions.OrderBy(x => Random.value).ToArray();
        records = recordsSO.records.OrderBy(x => Random.value).ToList();

        // ÿ�����õ�records��Ŀ��ȷ�� ������enumerator����
        IEnumerator<Record> enumerator = records.GetEnumerator();
        enumerator.MoveNext();  // �����һ��

        for (int i = 0; i < nGhosts; i++){
            GhostInstance ghost = new GhostInstance();
            ghost.ghostName = names[i]._name;
            ghost.ghostType = names[i]._type;
            if (names[i]._type == GhostType.male || names[i]._type == GhostType.female){
                // ����
                ghost.profession = professions[i];
                int age = Random.Range(25, 100);
                ghost.age = age;
                // ��minimiumRecords�Ļ�����ÿ15������һ����¼
                int nRecord = minimiumRecords + (age - 25) % 15;
                for (int j = 0; j < nGhosts; j++) {
                    ghost.records.Add(enumerator.Current); 
                    enumerator.MoveNext();
                }
            }else{
                // ����
                ghost.profession = "";
                ghost.age = Random.Range(1, 20);
            }
            ghosts.Add(ghost);
        }
    }

    void DisplayGhost(int idx){
        // ���е���β��������
        if(idx >= nGhosts){
            foreach (GhostInstance ghst in ghosts) {
                print(ghst.ghostName + "  " + ghst.judgement);
            }
            return;
        }

        currentGhostIdx = idx;
        // id
        string id;
        if (ghosts[idx].ghostType == GhostType.male || ghosts[idx].ghostType == GhostType.female){
            // ����
            id = $"{ghosts[idx].ghostName}\n " +
                 $"{ghosts[idx].ghostType}\n" +
                 $"Age at Death: {ghosts[idx].age}\n" +
                 $"Profession: {ghosts[idx].profession}\n" +
                 $"Dead by: ";
        }else{  
            //����
            id = $"{ghosts[idx].ghostName}\n " +
                 $"{ghosts[idx].ghostType}\n" +
                 $"Age at Death: {ghosts[idx].age}\n" +
                 $"Dead by: ";
        }
        idText.text = id;

        // records
        string record = "";
        if (ghosts[idx].records != null) { 
            foreach(var rec in ghosts[idx].records){
                record += $"{rec.description}\n";
            }
        }
        recordText.text = record;
    }

    public void Judgement(bool isReincarnate){
        ghosts[currentGhostIdx].judgement = isReincarnate;
        DisplayGhost(++currentGhostIdx);
    }
}
