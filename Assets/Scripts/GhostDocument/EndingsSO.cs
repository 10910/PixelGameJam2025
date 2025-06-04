using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ending{
    public string Title;    // ������Ϊ�ֵ��е�key ���ܸ�
    public string DisplayName;
    public string DisplayCondition;
    [TextArea(5, 10)]
    public string Description;
    public Sprite Image;
}

[CreateAssetMenu(fileName = "EndingsSO", menuName = "Scriptable Objects/EndingsSO")]
public class EndingsSO: ScriptableObject 
{
    public Ending[] endings;

    // ÿ�β��Ҷ�Ҫ���� Ч�ʵ� endingsӦ�����ֵ� �����Ѿ�����������д��ֵ�������ȱ�����״
    public Ending GetEndingByName(string name) {
        foreach (Ending ending in endings) {
            if (ending.Title == name){
                return ending;
            }
        }
        return null;
    }
}
