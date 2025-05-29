using UnityEngine;

public class MyPCPanel : MonoBehaviour
{
    [SerializeField]
    private BoolVariable isPC;

    private void OnEnable()
    {
        isPC.SetValue(true);
    }

    private void OnDisable()
    {
        isPC.SetValue(false);
    }
}
