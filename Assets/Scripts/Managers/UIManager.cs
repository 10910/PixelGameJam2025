using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
public class UIManager : MonoBehaviour, IGameStateListener
{
    public static UIManager Instance;
    [Header("Elements")]
    private GameManager gameManager;
    [SerializeField] private GameObject Light;
    [SerializeField] private GameObject Background_Light;
    [SerializeField] private GameObject Background_Dark;

    [SerializeField] private GameObject DocumentPanel;
    [SerializeField] private GameObject IDPanel;
    [SerializeField] private GameObject PullToHellEffect;
    [SerializeField] private GameObject RebirthEffect;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject judgePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject FilesPanel;
    [SerializeField] private GameObject EndingPanel;
    [SerializeField] private GameObject ResultPanel;
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

        panels.Add(judgePanel);
        panels.Add(settingsPanel);
        panels.Add(menuPanel);
        panels.Add(gamePanel);
        panels.Add(creditsPanel);
        panels.Add(FilesPanel);
        panels.Add(EndingPanel);
        panels.Add(ResultPanel);


        gameManager.onGamePause += GamePausedCallback;
        gameManager.onGameResume += GameResumedCallback;
        JudgeManager.onStartNewJudge += StartNewJudgeCallback;
        JudgeManager.onJudgeEnd += JudgeEndCallback;
    }

    private void OnDestroy()
    {
        gameManager.onGamePause -= GamePausedCallback;
        gameManager.onGameResume -= GameResumedCallback;
        JudgeManager.onStartNewJudge -= StartNewJudgeCallback;
        JudgeManager.onJudgeEnd -= JudgeEndCallback;
    }

    void Start()
    {
        CloseLight();
        ClosePullToHellEffect();
        CloseRebirthEffect();
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
                ShowPanel(gamePanel);
                break;
            case GameState.Menu:
                ShowPanel(menuPanel);
                break;
        }
    }

    //显示自己 关闭其他
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
        judgePanel.SetActive(true);
    }

    public void CloseJudgePanel()
    {
        judgePanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }

    public void OpenFilesPanel()
    {
        Debug.Log("OpenFilesPanel");
        FilesPanel.SetActive(true);
        DocumentPanel.SetActive(true);
        IDPanel.SetActive(false);
    }

    public void OpenIDPanel()
    {
        Debug.Log("OpenIDPanel");
        FilesPanel.SetActive(true);
        IDPanel.SetActive(true);
        DocumentPanel.SetActive(false);
    }

    public void CloseFilesPanel()
    {
        FilesPanel.SetActive(false);
        DocumentPanel.SetActive(false);
        IDPanel.SetActive(false);
    }

    [Button]
    public void OpenEndingPanel()
    {
        EndingPanel.SetActive(true);
    }

    public void CloseEndingPanel()
    {
        EndingPanel.SetActive(false);
    }

    [Button]
    public void OpenResultPanel()
    {
        ResultPanel.SetActive(true);
    }

    public void CloseResultPanel()
    {
        ResultPanel.SetActive(false);
    }

    public void ToggleFilesPanel()
    {
        if (DocumentPanel.activeSelf)
        {
            OpenIDPanel();
        }
        else
        {
            OpenFilesPanel();
        }
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

    private void StartNewJudgeCallback()
    {
        //我需要先重置ghost的位置

        Invoke("OpenLight", 1f);
    }

    private void JudgeEndCallback()
    {
        ClosePullToHellEffect();
        CloseRebirthEffect();
        Invoke("CloseLight", 1f);
    }
}
