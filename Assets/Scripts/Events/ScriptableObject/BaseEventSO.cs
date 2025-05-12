using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string descroption;//用于添加事件描述
    public DateTime lastSendTime;

    public UnityAction<T> OnEventRaised;

    //谁启用了这个事件
    public string lastSender;//最后一个广播

    //启用事件

    public void RaiseEvent(T value, object sender)
    {
        lastSendTime = DateTime.Now;
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }

}

