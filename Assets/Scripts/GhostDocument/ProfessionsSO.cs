using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProfessionsSO", menuName = "Scriptable Objects/ProfessionsSO")]
public class ProfessionsSO: ScriptableObject
{
    public List<string> professions;
}
