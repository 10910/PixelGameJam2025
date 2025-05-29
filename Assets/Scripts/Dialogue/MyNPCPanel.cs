using UnityEngine;

public class MyNPCPanel : MonoBehaviour
{
    [SerializeField]
    private BoolVariable isNPC;

    private void OnEnable()
    {
        isNPC.SetValue(true);
    }

    private void OnDisable()
    {
        isNPC.SetValue(false);
    }
}
