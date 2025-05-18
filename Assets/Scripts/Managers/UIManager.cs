using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IGameStateListener
{
    public static UIManager Instance;
    [Header("Elements")]
    private GameManager gameManager;
    // [SerializeField] private GameObject Light;
    [SerializeField] private GameObject Background_Light;
    [SerializeField] private GameObject Background_Dark;

    [SerializeField] private GameObject DocumentPanel;
    [SerializeField] private GameObject IDPanel;
    [SerializeField] private GameObject PullToHellEffect;
    [SerializeField] private GameObject RebirthEffect;
    [SerializeField] private TextMeshProUGUI IdTMP;
    [SerializeField] private TextMeshProUGUI RecordsTMP;
    [SerializeField] private GameObject Buttons;

    [Header("Result")]
    [SerializeField] private TextMeshProUGUI ResultRoundCounter;
    [SerializeField] private TextMeshProUGUI ResultGoodness;
    [SerializeField] private Slider ResultSlider;
    [SerializeField] private GameObject ResultLayoutGroup;

    [Header("Ending")]
    [SerializeField] private Image EndingBackground;
    [SerializeField] private TextMeshProUGUI EndingTMP;

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
        JudgeManager.onRoundEnd += RoundEndCallback;

        Buttons.SetActive(false);
    }

    private void OnDestroy()
    {
        gameManager.onGamePause -= GamePausedCallback;
        gameManager.onGameResume -= GameResumedCallback;
        JudgeManager.onStartNewJudge -= StartNewJudgeCallback;
        JudgeManager.onRoundEnd -= RoundEndCallback;
    }

    void Start()
    {
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
        Debug.Log("ShowPanel: " + panel.name);
        foreach (GameObject p in panels)
        {
            p.SetActive(p == panel);
            // if (p == panel)
            // {

            // }
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
    }

    public void CloseRebirthEffect()
    {
        RebirthEffect.SetActive(false);
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
        Buttons.SetActive(true);
        // 更新文件ui
        IdTMP.text = JudgeManager.Instance.idText;
        RecordsTMP.text = JudgeManager.Instance.recordsText;
    }

    private void JudgeEndCallback()
    {
        ClosePullToHellEffect();
        CloseRebirthEffect();
        // Invoke("CloseLight", 1f);
    }

    [Button("roundend")]
    private void RoundEndCallback()
    {
        Debug.Log("RoundEndCallback");
        print("set result UI");
        // 设置审判结果
        var history = JudgeManager.Instance.history;
        Transform human = ResultLayoutGroup.transform.Find("Human");
        Transform cat = ResultLayoutGroup.transform.Find("Cat");
        Transform dog = ResultLayoutGroup.transform.Find("Dog");
        Transform rat = ResultLayoutGroup.transform.Find("Rat");

        int[] cnts;
        int[] humanCnts = new int[2];
        int[] dogCnts = new int[2];
        int[] catCnts = new int[2];
        int[] ratCnts = new int[2];
        if (history.TryGetValue("male", out cnts))
        {
            humanCnts[0] += cnts[0];
            humanCnts[1] += cnts[1];
        }
        if (history.TryGetValue("female", out cnts))
        {
            humanCnts[0] += cnts[0];
            humanCnts[1] += cnts[1];
        }
        if (history.TryGetValue("cat", out cnts))
        {
            catCnts[0] += cnts[0];
            catCnts[1] += cnts[1];
        }
        if (history.TryGetValue("dot", out cnts))
        {
            dogCnts[0] += cnts[0];
            dogCnts[1] += cnts[1];
        }
        if (history.TryGetValue("rat", out cnts))
        {
            ratCnts[0] += cnts[0];
            ratCnts[1] += cnts[1];
        }
        SetResultUI(human, humanCnts[0], humanCnts[1]);
        SetResultUI(cat, catCnts[0], catCnts[1]);
        SetResultUI(dog, dogCnts[0], dogCnts[1]);
        SetResultUI(rat, ratCnts[0], ratCnts[1]);

        Debug.Log("humanCnts: " + humanCnts[0] + " " + humanCnts[1]);

        // 设置轮次文本
        ResultRoundCounter.text = "Trial " + GameManager.Instance.RoundsPlayed.ToString();

        // 设置功德值文本
        ResultGoodness.text = JudgeManager.Instance.currentGoodness.ToString();

        // 设置结局图片和文本
        EndingBackground.sprite = JudgeManager.Instance.currentEnding.Image;
        EndingTMP.text = JudgeManager.Instance.currentEnding.Description;
        // 打开结局面板
        Debug.Log("Opening EndingPanel");
        ShowPanel(EndingPanel);
    }



    private void SetResultUI(Transform counterUI, int nRebirth, int nHell)
    {
        // 一次都没有审判过时不显示这行UI
        if (nRebirth == 0 && nHell == 0)
        {
            counterUI.gameObject.SetActive(false);
            return;
        }
        // 设置文本
        counterUI.Find("Rebirth").GetComponent<TextMeshProUGUI>().text = nRebirth.ToString();
        counterUI.Find("Hell").GetComponent<TextMeshProUGUI>().text = nHell.ToString();
    }
}
