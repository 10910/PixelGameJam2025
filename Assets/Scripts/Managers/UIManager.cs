using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject Light;
    [SerializeField] private GameObject Background_Light;
    [SerializeField] private GameObject Background_Dark;

    [Header("Actions")]
    public Action<bool> onLightStateChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button("Open Light")]
    private void OpenLight()
    {
        Light.SetActive(true);
        Background_Light.SetActive(true);
        Background_Dark.SetActive(false);
        onLightStateChanged?.Invoke(true);
    }

    [Button("Close Light")]
    private void CloseLight()
    {
        Light.SetActive(false);
        Background_Light.SetActive(false);
        Background_Dark.SetActive(true);
        onLightStateChanged?.Invoke(false);
    }
}
