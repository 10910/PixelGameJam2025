using UnityEngine;

[System.Serializable]
public class DeathReason{
    public string reason;
    public GhostAge ageCondition;
    public GhostType typeCondition;
    public string professionCondition;
}

[CreateAssetMenu(fileName = "DeathReasonsSO", menuName = "Scriptable Objects/DeathReasonsSO")]
public class DeathReasonsSO : ScriptableObject 
{
    public DeathReason[] reasons;
}
