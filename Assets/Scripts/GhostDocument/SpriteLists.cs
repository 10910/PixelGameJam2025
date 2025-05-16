using UnityEngine;

[System.Serializable]
public class GhostSpriteList
{
    public GhostAge age;
    public GhostType type;
    public Sprite[] spriteList;
}
[System.Serializable]

[CreateAssetMenu(fileName = "SpriteSO", menuName = "Scriptable Objects/SpriteSO")]
public class SpriteLists : ScriptableObject
{
    public GhostSpriteList[] gsLists;
}
