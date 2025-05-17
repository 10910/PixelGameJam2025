using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public static GhostManager Instance;
    [SerializeField] public Ghost currentGhost;
    [SerializeField] private UIManager uiManager;

    [SerializeField] private Transform ghostSpawnPoint;

    private PullToHellEffect pullToHellEffect;

    private Transform ghostStartPoint;

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
        uiManager = FindAnyObjectByType<UIManager>(FindObjectsInactive.Include);
        pullToHellEffect = FindAnyObjectByType<PullToHellEffect>(FindObjectsInactive.Include);

        uiManager.onLightStateChanged += OnLightStateChanged;
        // JudgeManager.onStartNewJudge += StartDialogue;
        ghostStartPoint = ghostSpawnPoint;
        JudgeManager.onJudgeEnd += ResetGhostPosition;
    }

    private void OnDestroy()
    {
        //注销事件
        uiManager.onLightStateChanged -= OnLightStateChanged;
        // JudgeManager.onStartNewJudge -= StartDialogue;
        JudgeManager.onJudgeEnd -= ResetGhostPosition;
    }

    private void OnLightStateChanged(bool isLightOn)
    {
        if (isLightOn)
        {
            //重置ghost位置
            // ResetGhostPosition();
            currentGhost.gameObject.SetActive(true);
        }
        else//关灯
        {
            currentGhost.gameObject.SetActive(false);
            //重置ghost位置
            // ResetGhostPosition();
            //开始新的一轮裁决
            JudgeManager.Instance.StartNewJudge();
        }
    }

    // public void StartDialogue()
    // {
    //     currentGhost.ghostDialogue.gameObject.SetActive(true);

    // currentGhost = Instantiate(ghostPrefab, transform);
    //}

    //什么时候用？
    //这个方法有问题 不能获得现在的准确位置
    public void ResetGhostPosition()
    {
        //pullToHellEffect.ResetGhost();
        //currentGhost.gameObject.SetActive(false);
    }

    public void GenerateNewGhost()
    {
        GetNewGhostDialogue();
    }

    public void GetNewGhostDialogue()
    {
        currentGhost.ghostDialogue.GetNewDialogue();
    }

}
