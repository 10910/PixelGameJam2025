using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecialGhostInstance: GhostInstance{
    public string introDialogue;    // 人物出场时的对话
    public string midDialogue;      // 阅读文件后的对话
    public string endDialogue;      // 审判确定后的对话
    public bool isPlainDialogue;     // 用于和“同事”的对话，true时不会走审判流程
    public string dictName;         // 按这个名字在generator里查找字典 区别于name
}

[CreateAssetMenu(fileName = "GhostInstancesSO", menuName = "Scriptable Objects/GhostInstancesSO")]
public class GhostInstancesSO: ScriptableObject
{  
    public SpecialGhostInstance[] ghostInstances; 
} 
