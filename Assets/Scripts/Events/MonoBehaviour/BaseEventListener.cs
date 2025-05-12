using UnityEngine;
using UnityEngine.Events;

//挂在到物品上监听对应的so事件
public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO;

    //反馈/启动 事件
    public UnityEvent<T> response;

    //注册事件方法
    private void OnEnable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised += OnEventRaised;
        }
    }

    //注销事件方法
    private void OnDisable()
    {
        if (eventSO != null) { }
        eventSO.OnEventRaised -= OnEventRaised;

    }


    //让response全部都启动 把对应的值传进去
    private void OnEventRaised(T value)
    {
        response.Invoke(value);
    }

}
