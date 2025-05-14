using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Record{
    public string description;
    public int goodness;   //�û��̶�
}

[CreateAssetMenu(fileName = "RecordsSO", menuName = "Scriptable Objects/RecordsSO")]
public class RecordsSO: ScriptableObject
{
    public Record[] Records;
}
