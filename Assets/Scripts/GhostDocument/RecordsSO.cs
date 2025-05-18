using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Record
{
    public string description;
    public int goodness;   //好坏程度
    public GhostType typeCondition; // 用于筛选人和不同动物 设为male或female不会影响选取
    public string professionCondition;
}
[System.Serializable]

[CreateAssetMenu(fileName = "RecordsSO", menuName = "Scriptable Objects/RecordsSO")]
public class RecordsSO : ScriptableObject
{
    public Record[] records; 
} 
