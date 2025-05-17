using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(fileName = "GhostDialogueSO", menuName = "Scriptable Objects/GhostDialogueSO")]
public class GhostDialogueSO : ScriptableObject
{
    public List<string> dialogueNames;
}
