using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Variable", menuName = "Variable/Vector3Variable")]
public class Vector3Variable : ScriptableObject
{
    public Vector3 currentValue;
    public Vector3EventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    // 数据被更改时启动事件
    public void SetValue(Vector3 value)
    {
        currentValue = value;
        // 确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}
