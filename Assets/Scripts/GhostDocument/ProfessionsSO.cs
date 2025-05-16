using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(fileName = "ProfessionsSO", menuName = "Scriptable Objects/ProfessionsSO")]
public class ProfessionsSO : ScriptableObject
{
    public List<string> professions;
}
