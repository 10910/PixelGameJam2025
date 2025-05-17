using UnityEngine;

[System.Serializable]
public class DeathReason{
    string reason;
    GhostAge ageCondition;
    GhostType typeCondition;
    string professionCondition;
}

[CreateAssetMenu(fileName = "DeathReasonsSO", menuName = "Scriptable Objects/DeathReasonsSO")]
public class DeathReasonsSO : ScriptableObject
{
    public DeathReason[] reasons;
}
