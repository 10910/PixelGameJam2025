using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject
{

    public int maxValue;
    public int currentValue;
    public IntEventSO ValueChangedEvent;

    [TextArea]
    [SerializeField] private string description;

    //数据被更改时启动事件
    public void Setvalue(int value)
    {
        currentValue = value;
        //一旦我的数值变换我就呼叫一下 ?确保事件不为空才执行方法
        ValueChangedEvent?.RaiseEvent(value, this);
    }
}

