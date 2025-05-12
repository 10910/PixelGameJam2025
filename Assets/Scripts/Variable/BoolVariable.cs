using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Variable/BoolVariable")]
public class BoolVariable : ScriptableObject
{
    public bool currentValue;
    public BoolEventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    // 数据被更改时启动事件
    public void SetValue(bool value)
    {
        currentValue = value;
        // 确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}
