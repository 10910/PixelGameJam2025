using UnityEngine;

[CreateAssetMenu(fileName = "StringVariable", menuName = "Variable/StringVariable")]
public class StringVariable : ScriptableObject
{
    public string currentValue;
    public StringEventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    // 数据被更改时启动事件
    public void SetValue(string value)
    {
        currentValue = value;
        // 确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}
