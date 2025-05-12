using UnityEngine;

[CreateAssetMenu(fileName = "Vector3IntVariable", menuName = "Variable/Vector3IntVariable")]
public class Vector3IntVariable : ScriptableObject
{
    public Vector3Int currentValue;
    public Vector3IntEventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    // 数据被更改时启动事件
    public void SetValue(Vector3Int value)
    {
        currentValue = value;
        // 确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}
