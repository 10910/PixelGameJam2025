using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.ComponentModel;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{

    //获取都有谁订阅了这个事件
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if (baseEventSO == null)
        {
            baseEventSO = target as BaseEventSO<T>;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量：" + GetListeners().Count);


        foreach (var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());
        }

        // 显示 lastSendTime
        if (baseEventSO != null)
        {
            EditorGUILayout.LabelField("Last Send Time: " + baseEventSO.lastSendTime.ToString());
        }

    }
    //获取所有的监听
    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new();

        if (baseEventSO == null || baseEventSO.OnEventRaised == null)
        {
            return listeners;
        }

        var suscribers = baseEventSO.OnEventRaised.GetInvocationList();



        foreach (var subscriber in suscribers)
        {
            var obj = subscriber.Target as MonoBehaviour;
            if (obj != null)
            {
                listeners.Add(obj);
            }
        }
        return listeners;
    }
}