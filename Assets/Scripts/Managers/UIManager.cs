using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Elements")]
    private GameManager gameManager;
    [SerializeField] private GameObject Light;
    [SerializeField] private GameObject Background_Light;
    [SerializeField] private GameObject Background_Dark;
    [SerializeField] private GameObject PullToHellEffect;
    [SerializeField] private GameObject RebirthEffect;

    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject JudgePanel;
    private List<GameObject> panels = new List<GameObject>();

    [Header("Actions")]
    public Action<bool> onLightStateChanged;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameManager = FindAnyObjectByType<GameManager>();

        panels.Add(JudgePanel);
        panels.Add(SettingsPanel);


        gameManager.onGamePause += GamePausedCallback;
        gameManager.onGameResume += GameResumedCallback;
    }

    void Start()
    {
        ClosePullToHellEffect();
        CloseRebirthEffect();
    }

    public void ShowPanel(GameObject panel)
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(p == panel);
            // if (p == panel)
            // {

            // }
        }
    }

    public void OpenJudgePanel()
    {
        JudgePanel.SetActive(true);
    }

    public void CloseJudgePanel()
    {
        JudgePanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        SettingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    public void OpenPullToHellEffect()
    {
        PullToHellEffect.SetActive(true);
    }

    public void ClosePullToHellEffect()
    {
        PullToHellEffect.SetActive(false);
    }

    public void OpenRebirthEffect()
    {
        RebirthEffect.SetActive(true);
        GhostManager.Instance.currentGhost.gameObject.SetActive(false);
    }

    public void CloseRebirthEffect()
    {
        RebirthEffect.SetActive(false);
    }



    [Button("Open Light")]
    public void OpenLight()
    {
        Light.SetActive(true);
        Background_Light.SetActive(true);
        Background_Dark.SetActive(false);
        onLightStateChanged?.Invoke(true);
    }

    [Button("Close Light")]
    public void CloseLight()
    {
        Light.SetActive(false);
        Background_Light.SetActive(false);
        Background_Dark.SetActive(true);
        onLightStateChanged?.Invoke(false);
    }

    private void GamePausedCallback()
    {
        Debug.Log("Game paused");
        OpenSettingsPanel();
    }

    private void GameResumedCallback()
    {
        CloseSettingsPanel();
    }

}
