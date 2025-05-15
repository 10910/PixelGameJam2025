using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Name
{
    public string _name;
    public GhostType _type;
}

[CreateAssetMenu(fileName = "NamesSO", menuName = "Scriptable Objects/NamesSO")]
public class NamesSO : ScriptableObject
{
    public Name[] names;
}

