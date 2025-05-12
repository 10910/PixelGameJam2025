using UnityEngine;

[CreateAssetMenu(fileName = "Vector2IntVariable", menuName = "Variable/Vector2IntVariable")]
public class Vector2IntVariable : ScriptableObject
{
    public Vector2Int currentValue;
    public Vector2IntEventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    // 数据被更改时启动事件
    public void SetValue(Vector2Int value)
    {
        currentValue = value;
        // 确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}
