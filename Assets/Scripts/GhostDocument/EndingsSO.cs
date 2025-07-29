using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ending{
    public string Title;    // 用于作为字典中的key 不能改
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

    // 每次查找都要遍历 效率低 endings应该用字典 但是已经数组在里面写过值了所以先保持现状
    public Ending GetEndingByName(string name) {
        foreach (Ending ending in endings) {
            if (ending.Title == name){
                return ending;
            }
        }
        return null;
    }
}
