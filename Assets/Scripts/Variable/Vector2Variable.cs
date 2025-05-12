using UnityEngine;

[CreateAssetMenu(fileName = "Vector2Variable", menuName = "Variable/Vector2Variable")]
public class Vector2Variable : ScriptableObject
{
    public Vector2 currentValue;
    public Vector2EventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    // 数据被更改时启动事件
    public void SetValue(Vector2 value)
    {
        currentValue = value;
        // 确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}
