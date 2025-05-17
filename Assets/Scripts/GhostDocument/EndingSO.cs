using UnityEngine;

[System.Serializable]
public class Ending{
    string Title;
    string Description;
    Sprite Image;
}

[CreateAssetMenu(fileName = "EndingsSO", menuName = "Scriptable Objects/EndingsSO")]
public class EndingsSO: ScriptableObject 
{
    public Ending[] endings;
}
