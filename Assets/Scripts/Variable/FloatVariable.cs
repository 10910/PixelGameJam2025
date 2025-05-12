using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "Variable/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    public float maxValue;
    public float currentValue;
    public FloatEventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    // 数据被更改时启动事件
    public void SetValue(float value)
    {
        currentValue = value;
        // 确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}
